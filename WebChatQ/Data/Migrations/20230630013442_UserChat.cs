using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChatQ.Data.Migrations
{
    public partial class UserChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserChat",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    FromId = table.Column<string>(nullable: true),
                    ToId = table.Column<string>(nullable: true),
                    ReadStatus = table.Column<bool>(nullable: false),
                    Waktu = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChat", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChat");
        }
    }
}
