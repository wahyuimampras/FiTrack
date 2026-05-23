using AutoMapper;
using FiTrack.Domain.Entities;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.DTOs.Auth;

namespace FiTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();
        CreateMap<Account, AccountDto>();
        CreateMap<Budget, BudgetDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<RecurringBill, RecurringBillDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<SavingGoal, SavingGoalDto>();
        CreateMap<UserSession, SessionDto>();
    }
}
