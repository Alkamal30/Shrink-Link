using MediatR;

namespace ShrinkLink.LinkService.Application.Features.Links.DeleteLink;

public record DeleteLinkCommand(long Id) : IRequest;
