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
            migrationBuilder.EnsureSchema(
                name: "qweqwe");

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "qweqwe",
                columns: table => new
                {
                    customerid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customername = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    customeremail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    customersubject = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    customermessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__customer__B61ED7F57451DEB7", x => x.customerid);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "qweqwe",
                columns: table => new
                {
                    productid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productname = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    productimg = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products__2D172D32E28A3E37", x => x.productid);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                schema: "qweqwe",
                columns: table => new
                {
                    projectid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectname = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    projectimg = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    projectplace = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__projects__11EC39DD7720BDC2", x => x.projectid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "qweqwe",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    password = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__CBA1B25754546766", x => x.userid);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC5724A16B652",
                schema: "qweqwe",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers",
                schema: "qweqwe");

            migrationBuilder.DropTable(
                name: "products",
                schema: "qweqwe");

            migrationBuilder.DropTable(
                name: "projects",
                schema: "qweqwe");

            migrationBuilder.DropTable(
                name: "users",
                schema: "qweqwe");
        }
    }
}
