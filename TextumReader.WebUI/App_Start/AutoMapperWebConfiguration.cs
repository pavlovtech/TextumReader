using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TextumReader.ProblemDomain;
using TextumReader.WebUI.Models;

namespace TextumReader.WebUI.App_Start
{
    public class AutoMapperWebConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MaterialProfile());
            });
        }
    }

    public class MaterialProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MaterialViewModel, Material>();
            Mapper.CreateMap<Material, MaterialViewModel>();
        }
    }
}