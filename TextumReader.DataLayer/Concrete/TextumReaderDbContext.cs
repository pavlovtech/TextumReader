using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TextumReader.DataLayer.Configurations;

namespace TextumReader.DataLayer.Concrete
{
    public class TextumReaderDbContext: DbContext
    {
        static TextumReaderDbContext()
        {
            Database.SetInitializer<TextumReaderDbContext>(null);
        }

        public TextumReaderDbContext(string connectionString) 
            :base(connectionString)
        {
        }

        public TextumReaderDbContext()
            : base("TextumReaderDbContext")
        {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new WordConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new MaterialConfiguration());
            modelBuilder.Configurations.Add(new TranslationConfiguration());
        }
    }
}
