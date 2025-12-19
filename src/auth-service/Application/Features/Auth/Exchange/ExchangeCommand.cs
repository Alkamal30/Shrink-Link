using FluentResults;
using MediatR;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;
using System.Security.Claims;

namespace ShrinkLink.AuthService.Application.Features.Auth.Exchange;

public record ExchangeCommand(Identity User, ClaimsPrincipal Principal) : IRequest<Result<ClaimsIdentity>>;