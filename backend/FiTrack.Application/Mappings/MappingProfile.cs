using AutoMapper;
using FiTrack.Domain.Entities;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();
    }
}
