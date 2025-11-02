using MediatR;

namespace ShrinkLink.LinkService.Application.Features.DeleteLink;

public record DeleteLinkCommand(long Id) : IRequest;
