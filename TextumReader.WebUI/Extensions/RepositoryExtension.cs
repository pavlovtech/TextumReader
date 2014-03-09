using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextumReader.DataLayer.Abstract;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Extensions
{
    public static class RepositoryExtension
    {
        public static ICollection<Material> GetMaterialsByUserId(this IGenericRepository repo, string userId)
        {
            return repo.Get<Material>(m => m.UserId == userId).ToList();
        }
    }
}