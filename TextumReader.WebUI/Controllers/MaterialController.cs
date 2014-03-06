using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using TextumReader.ProblemDomain;
using TextumReader.DataLayer.Abstract;
using Linguistics.Dictionary;
using TextumReader.WebUI.Extensions;
using TextumReader.WebUI.Models;
using TextumReader.Utilities;

namespace TextumReader.WebUI.Controllers
{
    public class MaterialController : Controller
    {
        readonly IDictionary _dictionary = new BablaDictionary();
        private readonly IGenericRepository _repository;

        public MaterialController(IGenericRepository repository)
        {
            _repository = repository;
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

        public ViewResult Material(int id, int page = 1)
        {
            var material = _repository.GetSingle<Material>(p => p.MaterialId == id);

            int size = 40;
            var sencences = material.ForeignText.Split(new char[] {'.'});
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

            viewModel.ForeignText = sb.ToString();

            viewModel.AllDictionaries =
                _repository.Get<Dictionary>()
                    .DictionariesToSelectListItems(
                        _repository.GetSingle<Material>(m => m.MaterialId == material.MaterialId).MaterialId);

            return View(viewModel);
        }

        public PartialViewResult WordList(int dictionaryId)
        {
            var words = _repository.Get<Word>(w => w.DictionaryId == dictionaryId);
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

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(MaterialViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var material = _repository.GetSingle<Material>(m => m.MaterialId == viewModel.MaterialId);

            material.CategoryId = viewModel.CategoryId;
            material.ForeignText = viewModel.ForeignText;
            material.NativeText = viewModel.NativeText;
            material.Title = viewModel.Title;

            _repository.SaveChanges();

            TempData["message"] = string.Format("Material has been saved");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(MaterialViewModel viewModel)
        {
            var material = Mapper.Map<Material>(viewModel);

            material.DictionaryId = _repository.Get<Dictionary>().First().DictionaryId;

            _repository.Add(material);
            _repository.SaveChanges();

            TempData["message"] = string.Format("Material has been saved");
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            var categories = _repository.Get<Category>();
            ViewData["Categories"] = new SelectList(categories, "CategoryId", "Name");
            return View("Create", new MaterialViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> GetTranslation(string word)
        {
            WordTranslation translation;

            try
            {
                translation = await _dictionary.GetTranslation(word, "английский-русский");
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(translation, JsonRequestBehavior.AllowGet);
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
                materials = _repository.Get<Material>();
            else
                materials = _repository.Get<Material>(m => m.Category.Name == category);

            return materials.ToList().Select(Mapper.Map<MaterialViewModel>);
        }

        #endregion
    }
}
