using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Domain.Entities;

namespace SpendingTracker.Application.Services
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Accounts, AccountsDto>().ReverseMap();


            });
            return mappingConfig;
        }
    }
}
