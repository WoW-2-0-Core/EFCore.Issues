using EfCoreIssue.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreIssue.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController(GuestFeedbackService guestFeedbackService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetFeedbacks()
    {
        return Ok(guestFeedbackService.Get());
    }
    
    [HttpDelete]
    public async ValueTask<IActionResult> RemoveGuestFeedbackByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        return Ok(await guestFeedbackService.DeleteByIdAsync(id, cancellationToken: cancellationToken));
    }
}