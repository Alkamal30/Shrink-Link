using FluentResults;
using MediatR;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace ShrinkLink.AuthService.Application.Features.Auth.Authorize;

public record AuthorizeCommand(OpenIddictRequest Request, ClaimsPrincipal Principal) : IRequest<Result<ClaimsIdentity>>;