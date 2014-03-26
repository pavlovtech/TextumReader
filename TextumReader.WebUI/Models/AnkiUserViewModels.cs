using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TextumReader.WebUI.Models
{

    public class AnkiUserAggregateViewModel
    {
        public AnkiUserAggregateViewModel()
        {
            Step1 = new AnkiUserViewModel1();
            Step2 = new AnkiUserViewModel2();
        }

        public AnkiUserViewModel1 Step1 { get; set; }
        public AnkiUserViewModel2 Step2 { get; set; }
    }

    public class AnkiUserViewModel1
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int AnkiUserId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Required]
        [DisplayName("Anki Login")]
        public string Login { get; set; }

        [Required]
        [DisplayName("Anki Password")]
        public string Password { get; set; }
    }

    public class AnkiUserViewModel2
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string DeckName { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string CardId { get; set; }
        
        public IEnumerable<SelectListItem> AllDecks { get; set; }
        public IEnumerable<SelectListItem> AllCards { get; set; }
    }
}