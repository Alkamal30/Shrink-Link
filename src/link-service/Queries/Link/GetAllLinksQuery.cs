using System.Collections.Generic;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Queries;

public record GetAllLinksQuery() : IRequest<IEnumerable<Link>>;
