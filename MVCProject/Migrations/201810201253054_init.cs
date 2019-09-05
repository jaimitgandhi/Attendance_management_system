namespace MVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Admin_Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Admin_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Dept_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Dept_Id);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        Fact_Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Dept_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Fact_Id)
                .ForeignKey("dbo.Departments", t => t.Dept_Id, cascadeDelete: true)
                .Index(t => t.Dept_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Stud_Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Dept_Id = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                        TotalDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Stud_Id)
                .ForeignKey("dbo.Departments", t => t.Dept_Id, cascadeDelete: true)
                .Index(t => t.Dept_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Dept_Id", "dbo.Departments");
            DropForeignKey("dbo.Faculties", "Dept_Id", "dbo.Departments");
            DropIndex("dbo.Students", new[] { "Dept_Id" });
            DropIndex("dbo.Faculties", new[] { "Dept_Id" });
            DropTable("dbo.Students");
            DropTable("dbo.Faculties");
            DropTable("dbo.Departments");
            DropTable("dbo.Admins");
        }
    }
}
