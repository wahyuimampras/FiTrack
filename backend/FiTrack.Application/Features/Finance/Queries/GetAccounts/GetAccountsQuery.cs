// GetAccountsQuery.cs
using FiTrack.Application.DTOs.Finance;
using MediatR;
using System.Collections.Generic;

namespace FiTrack.Application.Features.Finance.Queries.GetAccounts;

public record GetAccountsQuery : IRequest<IEnumerable<AccountDto>>;