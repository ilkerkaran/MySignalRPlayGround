using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRServer.API.Services;
using SignalRServer.API.ViewModels;
using SignalRServer.API.Security;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System;

namespace SignalRServer.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly UserService userService;
        private readonly JwtFactory jwtFactory;

        public AuthController(UserService userService, JwtFactory jwtFactory)
        {
            this.userService = userService;
            this.jwtFactory = jwtFactory;
        }



        [HttpPost]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!userService.IsAuthorized(credentials.UserName, credentials.Password))
            {
                return BadRequest("login failed");
            }

            // Serialize and return the response
            try
            {
                var response = new
                {
                    access_token = jwtFactory.GenerateEncodedToken(credentials.UserName)
                };

                var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
                return Ok(json);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetBaseException().Message);
                throw;
            }

        }

    }
}