namespace TextumReader.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        MaterialId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ForignText = c.String(),
                        NativeText = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        MaterialId = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MaterialId)
                .ForeignKey("dbo.Materials", t => t.MaterialId)
                .Index(t => t.MaterialId);
            
            CreateTable(
                "dbo.Dictionaries",
                c => new
                    {
                        MaterialId = c.Int(nullable: false),
                        DictionaryId = c.Int(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.MaterialId)
                .ForeignKey("dbo.Materials", t => t.MaterialId)
                .Index(t => t.MaterialId);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        WordID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SelectedTranslation = c.String(),
                        Dictionary_MaterialId = c.Int(),
                    })
                .PrimaryKey(t => t.WordID)
                .ForeignKey("dbo.Dictionaries", t => t.Dictionary_MaterialId)
                .Index(t => t.Dictionary_MaterialId);
            
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        TranslationId = c.Int(nullable: false, identity: true),
                        WordId = c.Int(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.TranslationId)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.WordId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Translations", new[] { "WordId" });
            DropIndex("dbo.Words", new[] { "Dictionary_MaterialId" });
            DropIndex("dbo.Dictionaries", new[] { "MaterialId" });
            DropIndex("dbo.Categories", new[] { "MaterialId" });
            DropIndex("dbo.Materials", new[] { "User_UserId" });
            DropForeignKey("dbo.Translations", "WordId", "dbo.Words");
            DropForeignKey("dbo.Words", "Dictionary_MaterialId", "dbo.Dictionaries");
            DropForeignKey("dbo.Dictionaries", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.Categories", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.Materials", "User_UserId", "dbo.Users");
            DropTable("dbo.Translations");
            DropTable("dbo.Words");
            DropTable("dbo.Dictionaries");
            DropTable("dbo.Categories");
            DropTable("dbo.Materials");
            DropTable("dbo.Users");
        }
    }
}
