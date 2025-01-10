using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;




[ApiController]
[Route("Webapi")]
public class A2Controller : Controller
{
    private readonly A2DbContext _repo;

    public A2Controller(A2DbContext repo)
    {
        _repo = repo;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> RegisterUserAsync(Users user)
    {
        if (_repo.Users.Any(e => e.UserName == user.UserName))
        {
            return Ok($"UserName {user.UserName} is not available.");
        }

        await _repo.Users.AddAsync(user);
        await _repo.SaveChangesAsync();
        return Ok("User successfully registered.");


    }

    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "UserOnly")]
    [HttpPost("PurchaseItem")]
    public async Task<ActionResult> PurchaseItemAsync(int productId)
    {
        var ci = HttpContext.User.Identities.FirstOrDefault();
        var c = ci.FindFirst("userName");
        if (c == null)
        {
            return Forbid();
        }
        string username = c.Value;

        var product = await _repo.Products.FindAsync(productId);
        if (product == null)
        {
            return BadRequest($"Product {productId} not found");
        }

        // If user is registered and product exists, return the required information
        var result = new PurchaseOutput
        {
            UserName = username,
            ProductID = productId
        };

        return Ok(result);
    }


    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [Authorize(Policy = "OrganizerOnly")]
    [HttpPost("AddEvent")]
    public async Task<ActionResult> AddEventAsync(EventInput eventInput)
    {

        // Validate date format using regex for Start and End
        Regex regex = new Regex(@"\d{8}T\d{6}Z");

        if (!regex.IsMatch(eventInput.Start) && !regex.IsMatch(eventInput.End))
        {
            return BadRequest("The format of Start and End should be yyyyMMddTHHmmssZ.");
        }

        if (!regex.IsMatch(eventInput.Start))
        {
            return BadRequest("The format of Start should be yyyyMMddTHHmmssZ.");
        }

        if (!regex.IsMatch(eventInput.End))
        {
            return BadRequest("The format of End should be yyyyMMddTHHmmssZ.");
        }

        // Add event to the database
        Event newEvent = new Event
        {
            Start = eventInput.Start,
            End = eventInput.End,
            Summary = eventInput.Summary,
            Description = eventInput.Description,
            Location = eventInput.Location
        };

        await _repo.Events.AddAsync(newEvent);
        await _repo.SaveChangesAsync();

        return Ok("Success");
    }

    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [HttpGet("EventCount")]
    public async Task<ActionResult<int>> GetEventCountAsync()
    {
        int eventCount = await _repo.Events.CountAsync();
        return Ok(eventCount);
    }

    [Authorize(AuthenticationSchemes = "MyAuthentication")]
    [HttpGet("Event/{eventId}")]
    [Produces("text/calendar")]
    public async Task<IActionResult> GetEventDetailsAsync(int eventId)
    {
        // Fetch event from database
        Event ourevent = await _repo.Events.FindAsync(eventId);

        if (ourevent == null)
        {
            return BadRequest($"Event {eventId} does not exist.");
        }

        return Ok(ourevent);
    }









}