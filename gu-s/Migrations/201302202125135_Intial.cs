namespace gu_s.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        StartAddress = c.String(),
                        EndAddress = c.String(),
                        StartFirstOctet = c.Int(nullable: false),
                        StartSecondOctet = c.Int(nullable: false),
                        StartThirdOctet = c.Int(nullable: false),
                        EndFirstOctet = c.Int(nullable: false),
                        EndSecondOctet = c.Int(nullable: false),
                        EndThirdOctet = c.Int(nullable: false),
                        CountryName = c.String(),
                        CountryAbbr = c.String(),
                    })
                .PrimaryKey(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Countries");
        }
    }
}
