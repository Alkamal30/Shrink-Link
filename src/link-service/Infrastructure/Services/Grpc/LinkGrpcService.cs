using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Application.Contracts;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Services;

namespace ShrinkLink.LinkService.Infrastructure.Services.Grpc;

public class LinkGrpcService(ILinkServiceContext context, IShortCodeService shortCodeService)
    : LinkService.Application.Contracts.LinkService.LinkServiceBase
{
    public override async Task<GetOriginalLinkResponse> GetOriginalLink(GetOriginalLinkRequest request, ServerCallContext callContext)
    {
        var linkId = shortCodeService.ConvertToId(request.Code);
        
        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == linkId);

        if (link is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Link not found"));
        }
        
        return new GetOriginalLinkResponse
        {
            OriginalLink = link.OriginalUrl
        };
    }
}