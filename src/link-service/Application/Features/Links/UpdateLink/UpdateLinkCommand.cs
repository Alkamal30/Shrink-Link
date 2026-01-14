using MediatR;

namespace ShrinkLink.LinkService.Application.Features.Links.UpdateLink;

public record UpdateLinkCommand(long Id, string ShortCode, string OriginalUrl) : IRequest;
