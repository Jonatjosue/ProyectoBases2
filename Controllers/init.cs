using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace proyectoMMSBS2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class init : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<init> _logger;
        private readonly BContext db;

        public init(ILogger<init> logger, BContext context)
        {
            _logger = logger;
            db = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet(), Route("/Login")]
        public ActionResult<int> Get()
        {
            var result =    db.Database.SqlQueryRaw<int>("exec PRUEBASP").AsEnumerable().FirstOrDefault();
            return Ok(result);
        }
    }
}
