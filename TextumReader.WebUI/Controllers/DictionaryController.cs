using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextumReader.DataLayer.Abstract;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IGenericRepository _repository;

        public DictionaryController(IGenericRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var dictionaries = _repository.Get<Dictionary>().Select(d => d).ToList();
            return View(dictionaries);
        }

        public ActionResult WordList(int dictionaryId)
        {
            var words = _repository.Get<Word>().Where(w => w.DictionaryId == dictionaryId).AsEnumerable().Reverse();
            return View(words);
        }

        [HttpPost]
        public void AddWord(string word, string translation, int dictionaryId)
        {
            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word findedWord = dict.Words.FirstOrDefault(w => w.WordName == word);

            if (findedWord != null)
            {
                if (findedWord.Translations.Any(t => t.Value == translation))
                    return;

                findedWord.Translations.Add(new Translation() { WordId = findedWord.WordId, Value = translation });
            }
            else
            {
                Word newWord = new Word() { WordName = word, DictionaryId = dictionaryId };
                _repository.Add<Word>(newWord);

                newWord.Translations.Add(new Translation() { Value = translation });
            }
            _repository.SaveChanges();
        }

        [HttpPost]
        public JsonResult GetTranslations(string word, int dictionaryId)
        {
            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word findedWord = dict.Words.FirstOrDefault(w => w.WordName == word);

            if (findedWord != null)
                return Json(findedWord.Translations.Select(t => t.Value).ToArray());
            
            return null;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dict = _repository.GetSingle<Dictionary>(m => m.DictionaryId == id);
            _repository.Remove(dict);
            _repository.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteWord(int id)
        {
            var word = _repository.GetSingle<Word>(m => m.WordId == id);
            _repository.Remove(word);
            _repository.SaveChanges();

            return RedirectToAction("WordList");
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
	}
}