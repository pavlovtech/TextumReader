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
                .Select(Mapper.Map<WordViewModel>).ToList();

            return View(words);
        }

        [HttpPost]
        public void AddWord(string word, string translation, int dictionaryId)
        {
            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word foundWords = dict.Words.FirstOrDefault(w => w.WordName == word);

            if (foundWords != null)
            {
                if (foundWords.Translations.Any(t => t.Value == translation))
                    return;

                foundWords.Translations.Add(new Translation() { WordId = foundWords.WordId, Value = translation });
            }
            else
            {
                Word newWord = new Word() { WordName = word, DictionaryId = dictionaryId };
                newWord.AddDate = DateTime.Now;

                _repository.Add<Word>(newWord);

                newWord.Translations.Add(new Translation() { Value = translation });
            }
            _repository.SaveChanges();
        }

        [HttpPost]
        public JsonResult GetSavedTranslations(string word, int dictionaryId, string inputLang)
        {
            if (currentLanguage != inputLang.ToEnum<LanguagePrebuilt>())
            {
                lmtz = new LemmatizerPrebuiltCompact(inputLang.ToEnum<LanguagePrebuilt>());
            }

            word = lmtz.Lemmatize(word);

            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word findedWord = dict.Words.FirstOrDefault(w => w.WordName == word);

            if (findedWord != null)
                return Json(findedWord.Translations.Select(t => t.Value).ToArray());

            //return Json(Enumerable.Empty<string>());
            return null;
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
            if (!ankiWeb.IsAutorized)
            {
                if (user == null)
                    RedirectToAction("WordList", new { dictionaryId = dictId });

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
            }

            if (success)
                TempData["message"] = "The words have successfully been added to Anki";
            else
                TempData["alert"] = "The words have not been added to Anki";

            return RedirectToAction("WordList", new { dictionaryId = dictId });
        }    
	}
}