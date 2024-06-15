using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.Data
{
    public class PAWS_NDV_PetLoversContext:DbContext
    {
        public PAWS_NDV_PetLoversContext(DbContextOptions<PAWS_NDV_PetLoversContext> options) 
            :base (options)
        {
            
        }

        //dbset -  represents a collection of entities of a specific type
        public DbSet<Owner> Owners { get; set; } = default!;

        public DbSet<Pet> Pets { get; set; } = default!;
    }
}
