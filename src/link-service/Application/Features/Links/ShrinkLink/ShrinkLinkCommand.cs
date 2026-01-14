using MediatR;

namespace ShrinkLink.LinkService.Application.Features.Links.ShrinkLink;

public record ShrinkLinkCommand(string OriginalUrl) : IRequest<string>;
