using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.GetLink;

public record GetLinkQuery(long Id) : IRequest<Link>;
