
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.UpdateLink;

public record UpdateLinkCommand(long Id, string ShortCode, string OriginalUrl) : IRequest;
