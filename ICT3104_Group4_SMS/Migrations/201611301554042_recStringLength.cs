namespace ICT3104_Group4_SMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recommendations", "recommendation", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recommendations", "recommendation", c => c.String());
        }
    }
}
