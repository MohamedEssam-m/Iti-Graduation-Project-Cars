using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cars.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAppUserRepo _userRepo;

        public UsersController(IAppUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            try
            {
                var users = _userRepo.GetAll();
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
                {
                    return BadRequest("ID cannot be null or empty");
                }

                var user = _userRepo.GetById(id);

                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    return NotFound($"User with ID {id} not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public ActionResult<AppUser> CreateUser(AppUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _userRepo.Add(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, AppUser user)
        {
            try
            {
                if (string.IsNullOrEmpty(id) || user == null)
                {
                    return BadRequest("ID or user object is invalid");
                }

                if (id != user.Id)
                {
                    return BadRequest("ID in route doesn't match user ID");
                }

                var existingUser = _userRepo.GetById(id);
                if (existingUser == null || string.IsNullOrEmpty(existingUser.Id))
                {
                    return NotFound($"User with ID {id} not found");
                }

                _userRepo.Update(user);

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
                {
                    return BadRequest("ID cannot be null or empty");
                }

                var user = _userRepo.GetById(id);
                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    return NotFound($"User with ID {id} not found");
                }

                _userRepo.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}