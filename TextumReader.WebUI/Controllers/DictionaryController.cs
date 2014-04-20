using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoMapper;
using Linguistics.Anki;
using Linguistics.Models;
using Microsoft.AspNet.Identity;
using TextumReader.DataLayer.Abstract;
using TextumReader.ProblemDomain;
using TextumReader.WebUI.Models;
using TextumReader.WebUI.Extensions;
using LemmaSharp;

namespace TextumReader.WebUI.Controllers
{
    [Authorize]
    public class DictionaryController : Controller
    {
        private readonly IGenericRepository _repository;
        private AnkiWeb ankiWeb = new AnkiWeb();


        // TODO: get rid of it
        private LanguagePrebuilt currentLanguage = LanguagePrebuilt.English;
        private ILemmatizer lmtz;

        public DictionaryController(IGenericRepository repository)
        {
            lmtz = new LemmatizerPrebuiltCompact(currentLanguage);
            _repository = repository;
        }

        public ActionResult Index()
        {
            var dictionaries = _repository.GetDictionariesByUserId(User.Identity.GetUserId());
            return View(dictionaries);
        }

        public ActionResult WordList(int dictionaryId)
        {
            var words = _repository.Get<Word>()
                .Where(w => w.DictionaryId == dictionaryId)
                .ToList()
                .Select(Mapper.Map<WordViewModel>).OrderByDescending(w => w.AddDate).ToList();

            return View(words);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dict = _repository.GetSingle<Dictionary>(m => m.DictionaryId == id);

            if (dict.Title == "Default")
            {
                TempData["alert"] = "Sorry, but you cannot delete the default dictionary";
                return RedirectToAction("Index");
            }

            var defaultDict = _repository.GetSingle<Dictionary>(m => m.Title == "Default");
            var materials = _repository.Get<Material>(x => x.DictionaryId == dict.DictionaryId);
            foreach (var material in materials)
            {
                material.DictionaryId = defaultDict.DictionaryId;
            }
            _repository.SaveChanges();
            _repository.Remove(dict);
            _repository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteWord(int id)
        {
            var word = _repository.GetSingle<Word>(m => m.WordId == id);

            int dictId = word.DictionaryId;

            _repository.Remove(word);
            _repository.SaveChanges();

            return RedirectToAction("WordList", new { DictionaryId = dictId });
        }

        [HttpPost]
        public ActionResult DeleteTranslation(int id)
        {
            var translation = _repository.GetSingle<Translation>(m => m.TranslationId == id);

            int dictId = translation.Word.DictionaryId;

            if (translation.Word.Translations.Count == 1)
            {
                DeleteWord(translation.Word.WordId);
                return RedirectToAction("WordList", new { DictionaryId = dictId });
            }

            _repository.Remove(translation);
            _repository.SaveChanges();

            return RedirectToAction("WordList", new { DictionaryId = dictId });
        }

        public ViewResult Edit(int id)
        {
            var dict = _repository.GetSingle<Dictionary>(m => m.DictionaryId == id);
            return View(dict);
        }

        [HttpPost]
        public ActionResult Edit(Dictionary dict)
        {
            if (!ModelState.IsValid)
                return View(dict);

            var oldDict = _repository.GetSingle<Dictionary>(m => m.DictionaryId == dict.DictionaryId);
            oldDict.Title = dict.Title;

            _repository.SaveChanges();

            TempData["message"] = string.Format("Dictionary has been saved");
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Create", new Dictionary());
        }

        [HttpPost]
        public ActionResult Create(Dictionary dictionary)
        {
            dictionary.UserId = User.Identity.GetUserId();

            _repository.Add(dictionary);
            _repository.SaveChanges();

            TempData["message"] = string.Format("Dictionary has been added");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteWords(IEnumerable<WordViewModel> words)
        {
            int dictId = 0;
            if (words.Any())
            {
                dictId = words.ToList()[0].DictionaryId;
            }

            foreach (var wordViewModel in words.Where(m => m.IsSelected))
            {
                var word = _repository.GetSingle<Word>(w => w.WordId == wordViewModel.WordId);
                _repository.Remove<Word>(word);
            }
            _repository.SaveChanges();

            return RedirectToAction("WordList", new { dictionaryId = dictId });
        }

        [HttpPost]
        public async Task<ActionResult> AddToAnki(IEnumerable<WordViewModel> words)
        {
            int dictId = 0;
            if (words.Any())
            {
                dictId = words.ToList()[0].DictionaryId;
            }

            var user = _repository.GetSingle<AnkiUser>(u => u.UserId == User.Identity.GetUserId());

            if (user == null)
            {
                return RedirectToAction("RegisterStep1", "Anki");
            }

            if (!ankiWeb.IsAutorized)
            {
                ankiWeb.Login = user.Login;
                ankiWeb.Password = user.Password;
                await ankiWeb.Autorize();
            }

            bool success = false;
            foreach (var wordViewModel in words.Where(m => m.IsSelected))
            {
                var word = _repository.GetSingle<Word>(w => w.WordId == wordViewModel.WordId);

                string translations = "";

                foreach (var translation in word.Translations)
                {
                    translations += translation.Value + "<br>";
                }

                success = await ankiWeb.AddWord(word.WordName, translations, user.DeckName, user.CardId);

                if (success)
                {
                    word.IsAddedToAnki = true;
                }
            }

            _repository.SaveChanges();

            if (success)
                TempData["message"] = "The words have successfully been added to Anki";
            else
                TempData["alert"] = "The words have not been added to Anki";

            return RedirectToAction("WordList", new { dictionaryId = dictId });
        }    
	}
}