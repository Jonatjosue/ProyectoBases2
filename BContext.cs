using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace proyectoMMSBS2
{
    public class BContext : DbContext
    {

        public BContext(DbContextOptions<BContext> options) : base (options) { }



    }
}
