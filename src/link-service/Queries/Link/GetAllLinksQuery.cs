using System.Collections.Generic;
using MediatR;
using ShrinkLink.LinkService.Models;

namespace ShrinkLink.LinkService.Queries;

public record GetAllLinksQuery() : IRequest<IEnumerable<Link>>;
