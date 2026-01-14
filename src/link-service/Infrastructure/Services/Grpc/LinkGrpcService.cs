using Grpc.Core;
using ShrinkLink.LinkService.Application.Contracts;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Services;

namespace ShrinkLink.LinkService.Infrastructure.Services.Grpc;

public class LinkGrpcService(ILogger<LinkGrpcService> logger, ILinkServiceContext context, IShortCodeService shortCodeService)
    : Application.Contracts.LinkService.LinkServiceBase
{
    private ILogger<LinkGrpcService> _logger = logger;
    private ILinkServiceContext _context = context;
    private IShortCodeService _shortCodeService = shortCodeService;

    public override async Task<GetOriginalLinkResponse> GetOriginalLink(GetOriginalLinkRequest request, ServerCallContext callContext)
    {
        _logger.LogInformation("Retrieving original link for code: {Code}", request.Code);

        var linkId = _shortCodeService.ConvertToId(request.Code);
        
        var link = await _context.Links.FindAsync([linkId], callContext.CancellationToken);

        if (link is null)
        {
            _logger.LogWarning("Link with code {Code} was not found", request.Code);
            throw new RpcException(new Status(StatusCode.NotFound, "Link not found"));
        }

        _logger.LogInformation("Successfully retrieved original link for code: {Code}", request.Code);

        return new GetOriginalLinkResponse
        {
            OriginalLink = link.OriginalUrl
        };
    }
}