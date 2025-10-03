
using MediatR;

namespace ShrinkLink.LinkService.Application.Features.ShrinkLink;

public record ShrinkLinkCommand(string OriginalUrl) : IRequest<string>;
