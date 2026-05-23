using System;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto>;