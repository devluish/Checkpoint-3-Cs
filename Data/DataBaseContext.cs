using Microsoft.EntityFrameworkCore;
using CP3_C_.Models;

namespace checkpoint_DB.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<PratosModel> Pratos { get; set; }
    }
}
