using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAppUserService _userService;

    public UsersController(IAppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        try
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            var user = _userService.GetById(id);

            if (user == null)
                return NotFound($"User with ID {id} not found");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public ActionResult<AppUser> CreateUser(CreateUserVM model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _userService.Add(model);

            return Ok("User created successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(UpdateUserVM model)
    {
        try
        {
            if (model == null || string.IsNullOrEmpty(model.Id))
                return BadRequest("User object or ID is invalid");

            var existingUser = _userService.GetById(model.Id);
            if (existingUser == null)
                return NotFound($"User with ID {model.Id} not found");

            _userService.UpdateUser(model);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteUser(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            var user = _userService.GetById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found");

            _userService.DeleteUser(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
