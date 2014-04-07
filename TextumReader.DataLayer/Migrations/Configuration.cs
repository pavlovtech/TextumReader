 namespace TextumReader.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TextumReader.ProblemDomain;

    public sealed class Configuration : DbMigrationsConfiguration<TextumReader.DataLayer.Concrete.TextumReaderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TextumReader.DataLayer.Concrete.TextumReaderDbContext context)
        {
#if false
            context.Categories.AddOrUpdate(p => p.Name,
                new Category { Name = "Books" },
                new Category { Name = "Articles" },
                new Category { Name = "Subtitles" },
                new Category { Name = "Other" });

            context.SaveChanges();

            context.Materials.AddOrUpdate(p => p.Title,
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == " ниги").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == " ниги"),
                    Title = "Yakov Fain Methodologies in Software and Medicine",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                },
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == " ниги").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == " ниги"),
                    Title = "Record 1",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                },
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == "—татьи").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == "—татьи"),
                    Title = "Record 2",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                },
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == "—татьи").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == "—татьи"),
                    Title = "Record 3",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                },
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == "—убтитры").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == "—убтитры"),
                    Title = "Record 4",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                },
                new Material
                {
                    CategoryId = context.Categories.FirstOrDefault(x => x.Name == "ƒругое").CategoryId,
                    Category = context.Categories.FirstOrDefault(x => x.Name == "ƒругое"),
                    Title = "Record 4",
                    Text = "When a person gets into an emergency room in a hospital, he or she gets treated by regular doctors, not geniuses. I believe, this is the goal of the modern medical science Ц to makes sure that lots of doctors are available to provide medical help to patients. IТd assume that ER personnel has some well defined and strict procedures as to what to do when a person shows up with specific symptoms.",
                });

            context.SaveChanges();
#endif
        }
    }
}
