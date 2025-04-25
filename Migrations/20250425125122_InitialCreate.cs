using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArawanMarbleApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customerid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customername = table.Column<string>(type: "TEXT", nullable: false),
                    phone = table.Column<string>(type: "TEXT", nullable: true),
                    customeremail = table.Column<string>(type: "TEXT", nullable: true),
                    customersubject = table.Column<string>(type: "TEXT", nullable: true),
                    customermessage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customerid);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    productname = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    productimg = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productid);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    projectid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    projectname = table.Column<string>(type: "TEXT", nullable: false),
                    projectimg = table.Column<string>(type: "TEXT", nullable: true),
                    projectplace = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.projectid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
