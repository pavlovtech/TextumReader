using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> CategoriesToSelectListItems(this IEnumerable<Category> categories)
        {
            return
                categories.Select(cat =>
                          new SelectListItem
                          {
                              Text = cat.Name
                          });
        }

        public static IEnumerable<SelectListItem> DictionariesToSelectListItems(this IEnumerable<Dictionary> categories,
            int selectedId)
        {
            return
                categories.Select(dictionary =>
                          new SelectListItem
                          {
                              Selected = (dictionary.DictionaryId == selectedId),
                              Text = dictionary.Title,
                              Value = dictionary.DictionaryId.ToString()
                          });
        }
    }
}