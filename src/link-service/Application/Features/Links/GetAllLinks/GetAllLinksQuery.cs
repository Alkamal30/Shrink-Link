using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.GetAllLinks;

public record GetAllLinksQuery() : IRequest<IEnumerable<Link>>;
