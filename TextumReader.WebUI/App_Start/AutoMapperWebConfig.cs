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
            Mapper.Initialize(cfg => cfg.AddProfile(new Profile()));
        }
    }

    public class Profile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MaterialViewModel, Material>();
            Mapper.CreateMap<Material, MaterialViewModel>();

            Mapper.CreateMap<AnkiUserAggregateViewModel, AnkiUser>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Step1.Login))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Step1.Password))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Step1.UserId))
                .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.Step2.CardId))
                .ForMember(dest => dest.DeckName, opt => opt.MapFrom(src => src.Step2.DeckName));
        }
    }
}