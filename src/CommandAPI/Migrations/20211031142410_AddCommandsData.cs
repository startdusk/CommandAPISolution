using Microsoft.EntityFrameworkCore.Migrations;

namespace CommandAPI.Migrations
{
    public partial class AddCommandsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ref: https://stackoverflow.com/questions/52382691/add-data-using-migration-entity-framework-core-without-specifying-the-id
            migrationBuilder.InsertData(
                "command", // tableName
            new string[] { // columns
                "how_to",
                "platform",
                "command_line"
            },
            new object[]{ // values
                "Create an EF migration",
                "Entity Framework Core Command Line",
                "dotnet ef migrations add"
            });
            migrationBuilder.InsertData(
                "command", // tableName
            new string[] { // columns
                "how_to",
                "platform",
                "command_line"
            },
            new object[]{ // values
                "Apply Migrations to DB",
                "Entity Framework Core Command Line",
                "dotnet ef database update"
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
