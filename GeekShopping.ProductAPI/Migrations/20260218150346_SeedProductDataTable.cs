using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeekShopping.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "id", "category_name", "description", "image_url", "name", "price" },
                values: new object[,]
                {
                    { 1L, "Bebidas", "Refrigerante sabor cola", "https://www.coca-cola.com.br/content/dam/one-brasil/coca-cola/pt-br/desktop/packshot-coca-cola-350ml.png", "Coca-cola", 5.00m },
                    { 2L, "Bebidas", "Refrigerante sabor cola", "https://www.pepsico.com.br/uploads/images/products/pepsi-lata-350ml.png", "Pepsi", 4.50m },
                    { 3L, "Bebidas", "Refrigerante sabor laranja", "https://www.coca-cola.com.br/content/dam/one-brasil/fanta/pt-br/desktop/packshot-fanta-laranja-350ml.png", "Fanta Laranja", 4.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 3L);
        }
    }
}
