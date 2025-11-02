using System.Collections.Generic;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.GetAllLinks;

public record GetAllLinksQuery() : IRequest<IEnumerable<Link>>;
