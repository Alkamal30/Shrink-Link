using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Queries;

public record GetLinkQuery(long Id) : IRequest<Link>;
