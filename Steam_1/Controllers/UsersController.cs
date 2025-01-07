using Microsoft.AspNetCore.Mvc;
using Steam_1.Models;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Steam_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<AppUser> Get()
        {
            return AppUser.Read();
        }


        [HttpPost("Register")]
        public IActionResult Post([FromBody] AppUser user)
        {
            try
            {
                int rowsAffected = user.Insert();
                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                else if (rowsAffected == 0)
                {
                    return Conflict(new { message = "Email is already registered" });
                }
                else
                {
                    return Conflict(new { message = "Failed to register user" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(string Email, string Password)
        {
            AppUser user = new AppUser();
            bool isLoggedIn = user.Login(Email, Password);

            if (isLoggedIn)
            {

                var (userId, name) = user.GetUserIdByEmail(Email);

                if (userId > 0)
                {
                    return Ok(new
                    {
                        message = "Login successful",
                        UserId = userId,
                        Name = name
                    });
                }

                return NotFound(new { message = "User ID not found" });
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }



        [HttpPut("UpdateUser/{id}")]
        public IActionResult Put(int id, [FromBody] AppUser user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { message = "User ID mismatch" });
            }

            AppUser updatedUser = user.Update();

            if (updatedUser == null)
            {
                return Conflict(new { message = "User not found or email is already in use" });
            }

            return Ok(updatedUser);
        }


    }
}
