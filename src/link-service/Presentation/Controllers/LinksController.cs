using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShrinkLink.LinkService.Application.Features.Links.GetAllLinks;
using ShrinkLink.LinkService.Application.Features.Links.GetLink;
using ShrinkLink.LinkService.Application.Features.Links.UpdateLink;
using ShrinkLink.LinkService.Application.Features.Links.DeleteLink;
using ShrinkLink.LinkService.Application.Features.Links.ShrinkLink;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Application.Features.Links.AddLink;

namespace ShrinkLink.LinkService.Presentation.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class LinksController(ILogger<LinksController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<LinksController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("shrink")]
    public async Task<ActionResult<string>> Shrink(string url, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shrinking URL: {Url}", url);
        var result = await _mediator.Send(new ShrinkLinkCommand(url), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Link>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all links");
        var result = await _mediator.Send(new GetAllLinksQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Link>> Get(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting link with ID: {LinkId}", id);
        var result = await _mediator.Send(new GetLinkQuery(id), cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Link with ID: {LinkId} not found", id);
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Link>> Add(AddLinkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new link for URL: {Url}", request.OriginalUrl);
        var result = await _mediator.Send(request, cancellationToken);

        if (result is null)
        {
            _logger.LogError("Failed to add link for URL: {Url}", request.OriginalUrl);
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult> Update(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating link with ID: {LinkId}", request.Id);
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting link with ID: {LinkId}", id);
        await _mediator.Send(new DeleteLinkCommand(id), cancellationToken);

        return Ok();
    }
}
