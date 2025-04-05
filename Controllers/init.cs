using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


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

        public initDb(ILogger<initDb> logger, BContext context)
        {
            _logger = logger;
            db = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost(), Route("/Login")]
        public ActionResult<int> Post(int idUsuario,int idLogin,string parametro1)
        {
            var result =    db.Database.SqlQueryRaw<int>("exec ValidarLogin {0},{1},{2}",idUsuario,idLogin,parametro1).AsEnumerable().FirstOrDefault();
            return Ok(result);
        }
    }
}
