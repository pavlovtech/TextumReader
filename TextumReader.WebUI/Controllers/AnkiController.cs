using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Linguistics.Anki;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TextumReader.DataLayer.Abstract;
using TextumReader.ProblemDomain;
using TextumReader.WebUI.Models;
using TextumReader.WebUI.Extensions;

namespace TextumReader.WebUI.Controllers
{
    [Authorize]
    public class AnkiController : Controller
    {
        AnkiWeb ankiWeb = new AnkiWeb();
        private readonly IGenericRepository _repository;

        public AnkiController(IGenericRepository repository)
        {
            _repository = repository;
        }

        public ActionResult RegisterStep1()
        {
            var viewModel = new AnkiUserAggregateViewModel();

            var user = _repository.GetSingle<AnkiUser>(u => u.UserId == User.Identity.GetUserId());
            if (!ankiWeb.IsAutorized)
            {
                if (user == null)
                    return View("RegisterStep1", viewModel);

                viewModel.Step1.Login = user.Login;
                viewModel.Step1.Password = user.Password;
            }

            return View("RegisterStep1", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterStep1(AnkiUserAggregateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            ankiWeb.Login = viewModel.Step1.Login;
            ankiWeb.Password = viewModel.Step1.Password;
            await ankiWeb.Autorize();

            if (ankiWeb.IsAutorized)
            {
                viewModel.Step2.AllCards = (await ankiWeb.GetCards()).CardsToSelectListItems();
                viewModel.Step2.AllDecks = (await ankiWeb.GetDecks()).DecksToSelectListItems();

                return View("RegisterStep2", viewModel);
            }
            else
            {
                ModelState.AddModelError("Data", "Incorrect username/email or password");
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult RegisterStep2(AnkiUserAggregateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.Step1.UserId = User.Identity.GetUserId();

            var ankiUser = Mapper.Map<AnkiUser>(viewModel);

            _repository.Add(ankiUser);
            _repository.SaveChanges();

            return View("AnkiRegistrationFinish");
        }
	}
}