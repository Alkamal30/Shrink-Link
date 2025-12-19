using FluentResults;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLink.AuthService.Application.Features.Auth.SignIn;

public record SignInCommand(
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,

    [Required]
    [MaxLength(256)]
    string Password,

    string? ReturnUrl
) : IRequest<Result>;