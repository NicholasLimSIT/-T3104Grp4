namespace ICT3104_Group4_SMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seventh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RemovedUser",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Email = c.String(maxLength: 256),
                    PhoneNumber = c.String(),
                    UserName = c.String(nullable: false, maxLength: 256),
                    FullName = c.String(),
                    year = c.Int()
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
        }
    }
}
