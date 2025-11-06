using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.GetLink;

public record GetLinkQuery(long Id) : IRequest<Link>;
