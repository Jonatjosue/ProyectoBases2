using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using proyectoMMSBS2.DTOs;
using proyectoMMSBS2.Servicios;
using System.IdentityModel.Tokens.Jwt;


namespace proyectoMMSBS2.Controllers
{
    [ApiController]
    [Route("apiDb")]
    public class initDb : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<initDb> _logger;
        private readonly BContext db;
        private readonly autenticacionJwt autenticacionJwt;

        public initDb(ILogger<initDb> logger, BContext context)
        {
            _logger = logger;
            db = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost(), Route("/Login")]
        public ActionResult Post([FromBody] validarLogin login, [FromServices] autenticacionJwt jwtService)
        {
            var result = db.Database.SqlQueryRaw<int>(
                "exec ValidarLogin {0},{1},{2}",
                login.Usuario, login.idRole, login.parametro1
            ).AsEnumerable().FirstOrDefault();

            if (result == 1)
            {
                var user = new usuarioJwt
                {
                    Username = login.Usuario,
                    Role = login.idRole.ToString()
                };

                var token = jwtService.GenerateToken(user);

                return Ok(new
                {
                    success = true,
                    token
                });
            }

            return Unauthorized(new
            {
                success = false,
                message = "Credenciales inválidas"
            });
        }

        [HttpGet(), Route("/Roles")]
        public ActionResult<List<roleBd>> roles() {
            var result = db.Database.SqlQueryRaw<roleBd>("exec obtieneRoles").ToList();
            return Ok(result);
        }
    }
}
