namespace Diff_Service.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Differs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeftInput = c.String(),
                        RightInput = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Differs");
        }
    }
}
