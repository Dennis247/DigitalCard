namespace DigitalCard.Api.Helpers.Mapper;
using DIgitalCard.Lib.DTO;
using DIgitalCard.Lib.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<Card, CardDTO>();
        CreateMap<CardDTO, Card>();

        CreateMap<Customer, CustomerDTO>();
        CreateMap<CustomerDTO, Customer>();
    }






}