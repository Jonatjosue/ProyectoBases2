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
        [HttpPut(), Route("/post")]
        public ActionResult<String> postConsultaBD(String consulta) {


            var resultado = db.Database.SqlQueryRaw<string>(consulta).ToList();

            return Ok(resultado);
        }
        [HttpPost("run")]
        public async Task<IActionResult> RunQuery([FromBody] SqlQueryRequest request)
        {
            try
            {
                using var command = db.Database.GetDbConnection().CreateCommand();
                command.CommandText = request.Sql;
                db.Database.OpenConnection();

                using var reader = await command.ExecuteReaderAsync();

                var result = new List<Dictionary<string, object>>();
                var columns = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                    columns.Add(reader.GetName(i));

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[columns[i]] = reader.GetValue(i);
                    result.Add(row);
                }

                return Ok(new { columns, rows = result });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }

        public class SqlQueryRequest
        {
            public string Sql { get; set; }
        }

    }
}
