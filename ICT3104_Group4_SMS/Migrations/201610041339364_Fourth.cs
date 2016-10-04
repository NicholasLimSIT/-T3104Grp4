namespace ICT3104_Group4_SMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recommendations", "recommendation", c => c.String());
            DropColumn("dbo.Recommendations", "recomendation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recommendations", "recomendation", c => c.String());
            DropColumn("dbo.Recommendations", "recommendation");
        }
    }
}
