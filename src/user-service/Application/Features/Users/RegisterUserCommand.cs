using FluentResults;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLink.UserService.Application.Features.Users;

public record RegisterUserCommand(
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,
    [Required]
    [MinLength(8)]
    [MaxLength(128)]
    string Password
) : IRequest<Result>;