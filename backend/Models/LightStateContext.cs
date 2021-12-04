using Microsoft.EntityFrameworkCore;

namespace LightWebAPI.Models
{
    public class LightStateContext : DbContext
    {
        // Constructor
        public LightStateContext(DbContextOptions<LightStateContext> options)
            :base(options)
        {
            //Ensure db is created
            Database.EnsureCreated();
        }
        
        // Represents a collections of light states
        public DbSet<LightState> LightStates { get; set; }
        
    }
}