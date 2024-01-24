using Microsoft.AspNetCore.Mvc;
using UpdateIssue.Api.Persistence.DataContexts;

namespace UpdateIssue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController(AppDbContext appDbContext) : ControllerBase
{
    [HttpGet]
    public IActionResult GetFeedbacks()
    {
        return Ok(appDbContext.GuestFeedbacks.ToList());
    }

    [HttpDelete("{feedbackId:guid}")]
    public async ValueTask<IActionResult> RemoveGuestFeedbackByIdAsync([FromRoute] Guid feedbackId, CancellationToken cancellationToken = default)
    {
        var foundEntity = appDbContext.GuestFeedbacks.FirstOrDefault(feedback => feedback.Id == feedbackId);

        if (foundEntity is null) return NotFound();

        appDbContext.GuestFeedbacks.Remove(foundEntity);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}