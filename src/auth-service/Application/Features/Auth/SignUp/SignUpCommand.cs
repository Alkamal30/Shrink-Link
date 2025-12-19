using FluentResults;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLink.AuthService.Application.Features.Auth.SignUp;

public record SignUpCommand(
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    string Email,

    [Required]
    [MaxLength(256)]
    string UserName,

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(256)]
    string Password
) : IRequest<Result>;
