
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.DeleteLink;

public record DeleteLinkCommand(long Id) : IRequest;
