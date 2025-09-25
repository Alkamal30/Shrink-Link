using MediatR;
using ShrinkLink.LinkService.Models;

namespace ShrinkLink.LinkService.Queries;

public record GetLinkQuery(long Id) : IRequest<Link>;
