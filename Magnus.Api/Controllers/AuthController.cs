using Microsoft.AspNetCore.Mvc;

namespace Magnus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] dynamic credentials)
        {
            // Ejemplo simple, sin JWT por ahora
            if (credentials.username == "admin" && credentials.password == "123")
                return Ok(new { token = "fake-jwt-token", user = credentials.username });

            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}