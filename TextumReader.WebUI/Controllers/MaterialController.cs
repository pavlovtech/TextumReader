using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Linguistics.Models;
using Microsoft.AspNet.Identity;
using TextumReader.ProblemDomain;
using TextumReader.DataLayer.Abstract;
using Linguistics.Dictionary;
using TextumReader.WebUI.Extensions;
using TextumReader.WebUI.Models;

namespace TextumReader.WebUI.Controllers
{
    [Authorize]
    public class MaterialController : Controller
    {
        readonly IWebDictionary _webDictionary;
        private readonly IGenericRepository _repository;

        public MaterialController(IGenericRepository repository, IWebDictionary webDictionary)
        {
            _repository = repository;
            _webDictionary = webDictionary;
        }

        public ViewResult Index(string category = "all")
        {
            var viewModel = new MaterialsListViewModel
            {
                Categories = _repository.Get<Category>().CategoriesToSelectListItems(),
                CurrentCategory = category,
                Materials = GetMaterialsWithCategory(category)
            };

            return View(viewModel);
        }

        public PartialViewResult MaterialList(string category = "all")
        {
            var materials = GetMaterialsWithCategory(category);

            return PartialView("MaterialListPartial", materials.Reverse());
        }

        public ActionResult Material(int id, int page = 1)
        {
            var material = _repository.GetSingle<Material>(p => p.MaterialId == id && p.UserId == User.Identity.GetUserId());

            if (material == null)
                return RedirectToAction("Index");

            int size = 40;
            var sencences = material.Text.Split(new char[] {'.'});
            var selectedSentences = sencences.Skip((page - 1) * size).Take(size).ToArray();

            StringBuilder sb = new StringBuilder();
            Parallel.ForEach(selectedSentences, sencence =>
            {
                lock (sb) { sb.Append(sencence + '.'); }
            });

            var viewModel = Mapper.Map<MaterialViewModel>(material);

            viewModel.PagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = size,
                TotalItems = sencences.Count()
            };

            viewModel.Text = sb.ToString();

            viewModel.AllDictionaries =
                _repository.GetDictionariesByUserId(User.Identity.GetUserId())
                    .DictionariesToSelectListItems(id);

            return View(viewModel);
        }

        public PartialViewResult WordList(int dictionaryId)
        {
            var words = _repository.Get<Word>(w => w.DictionaryId == dictionaryId).Reverse();
            return PartialView("_WordListPartial", words);
        }

        [HttpPost]
        public PartialViewResult ChangeDictionary(int materialId, int dictionaryId)
        {
            var single = _repository.GetSingle<Material>(m => m.MaterialId == materialId);
            single.DictionaryId = dictionaryId;
            _repository.SaveChanges();

            var words = _repository.Get<Word>(w => w.DictionaryId == dictionaryId);

            return PartialView("_WordListPartial", words);
        }

        public ViewResult Edit(int id)
        {
            var material = _repository.GetSingle<Material>(m => m.MaterialId == id);

            var viewModel = Mapper.Map<MaterialViewModel>(material);

            var categories = _repository.Get<Category>();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name");

            viewModel.InputLanguage = (Language)material.InputLanguage;
            viewModel.OutputLanguage = (Language)material.OutputLanguage;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(MaterialViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var material = _repository.GetSingle<Material>(m => m.MaterialId == viewModel.MaterialId);
            AutoMapper.Mapper.Map(viewModel, material);

            _repository.SaveChanges();

            TempData["message"] = string.Format("Material has been saved");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(MaterialViewModel viewModel)
        {
            var material = Mapper.Map<Material>(viewModel);

            material.DictionaryId = _repository.GetSingle<Dictionary>(d => d.Title == "Default").DictionaryId;
            material.UserId = User.Identity.GetUserId();
            material.AddDate = material.AddDate = DateTime.Now;

            _repository.Add(material);
            _repository.SaveChanges();

            TempData["message"] = string.Format("Material has been saved");
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            var categories = _repository.Get<Category>();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name");

            var outputLnaguage = DetectCurrentLanguage();

            var viewModel = new MaterialViewModel()
            {
                InputLanguage = Language.English,
                OutputLanguage = outputLnaguage
            };

            return View("Create", viewModel);
        }

        private static Language DetectCurrentLanguage()
        {
            var culture = CultureInfo.CurrentCulture;
            string englishName = culture.EnglishName;
            string outputLang = englishName.Split(new char[] {' '})[0];

            Language outputLnaguage;
            try
            {
                outputLnaguage = outputLang.ToEnum<Language>();
            }
            catch
            {
                outputLnaguage = Language.English;
            }
            return outputLnaguage;
        }

        [HttpPost]
        public async Task<JsonResult> GetTranslations(string word, string inputLanguage, string outputLanguage)
        {
            WordTranslations translations;
      
            try
            {
                translations = await _webDictionary.GetTranslations(word,
                    inputLanguage.ToEnum<Language>(),
                    outputLanguage.ToEnum<Language>());
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var wordFreq = _repository.GetSingle<WordFrequency>(x => x.Word == translations.WordName);
            if (wordFreq != null)
                translations.WordFrequencyIndex = wordFreq.Position;

            translations.Translations = translations.Translations.Distinct().ToArray();

            return Json(translations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var material = _repository.GetSingle<Material>(m => m.MaterialId == id);
            _repository.Remove(material);
            _repository.SaveChanges();

            return RedirectToAction("MaterialList");
        }

        #region Utility methods

        protected IEnumerable<MaterialViewModel> GetMaterialsWithCategory(string category)
        {
            IEnumerable<Material> materials;
            if (category == "all" || string.IsNullOrEmpty(category))
                materials = _repository.GetMaterialsByUserId(User.Identity.GetUserId());
            else
                materials = _repository.GetMaterialsByUserId(User.Identity.GetUserId()).Where(m => m.Category.Name == category);

            return materials.ToList().Select(Mapper.Map<MaterialViewModel>);
        }

        #endregion
    }
}
