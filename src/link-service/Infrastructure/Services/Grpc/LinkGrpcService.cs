using Grpc.Core;
using ShrinkLink.LinkService.Application.Contracts;

namespace ShrinkLink.LinkService.Infrastructure.Services.Grpc;

public class LinkGrpcService : LinkService.Application.Contracts.LinkService.LinkServiceBase
{
    public override Task<GetOriginalLinkResponse> GetOriginalLink(GetOriginalLinkRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetOriginalLinkResponse
        {
            OriginalLink = $"localhost/result/{request.Code}"
        });
    }
}