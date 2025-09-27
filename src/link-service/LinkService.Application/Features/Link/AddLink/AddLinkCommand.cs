
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.AddLink;

public record AddLinkCommand(string ShortCode, string OriginalUrl) : IRequest<Link>;
