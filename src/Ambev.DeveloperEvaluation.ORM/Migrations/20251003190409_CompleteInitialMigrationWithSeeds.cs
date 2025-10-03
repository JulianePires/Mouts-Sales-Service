using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class CompleteInitialMigrationWithSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Manager = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    MinStockLevel = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Draft"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "IsActive", "Manager", "Name", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Av. Paulista, 1234 - Bela Vista, São Paulo - SP, 01310-100", new DateTime(2024, 4, 3, 10, 0, 0, 0, DateTimeKind.Utc), "saopaulo.downtown@ambev.com", true, "Carlos Silva", "São Paulo Downtown", "+55 11 3456-7890", new DateTime(2024, 4, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Av. Nossa Senhora de Copacabana, 987 - Copacabana, Rio de Janeiro - RJ, 22070-012", new DateTime(2024, 5, 3, 10, 0, 0, 0, DateTimeKind.Utc), "rio.copacabana@ambev.com", true, "Ana Costa", "Rio de Janeiro Copacabana", "+55 21 2987-6543", new DateTime(2024, 5, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Rua da Bahia, 456 - Savassi, Belo Horizonte - MG, 30160-012", new DateTime(2024, 6, 3, 10, 0, 0, 0, DateTimeKind.Utc), "bh.savassi@ambev.com", true, "Roberto Santos", "Belo Horizonte Savassi", "+55 31 3321-9876", new DateTime(2024, 6, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Rua Padre Chagas, 789 - Moinhos de Vento, Porto Alegre - RS, 90570-080", new DateTime(2024, 7, 3, 10, 0, 0, 0, DateTimeKind.Utc), "poa.moinhos@ambev.com", true, "Fernanda Lima", "Porto Alegre Moinhos", "+55 51 3234-5678", new DateTime(2024, 7, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "SCS Quadra 02, Bloco A, Loja 15 - Asa Sul, Brasília - DF, 70318-900", new DateTime(2024, 8, 3, 10, 0, 0, 0, DateTimeKind.Utc), "brasilia.asasul@ambev.com", true, "João Oliveira", "Brasília Asa Sul", "+55 61 3445-6789", new DateTime(2024, 8, 3, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Email", "IsActive", "Name", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), "Rua das Flores, 123 - Vila Madalena, São Paulo - SP", new DateTime(1985, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc), "maria.silva@email.com", true, "Maria Silva", "+55 11 99876-5432", new DateTime(2024, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), "Av. Atlântica, 456 - Copacabana, Rio de Janeiro - RJ", new DateTime(1990, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 3, 10, 0, 0, 0, DateTimeKind.Utc), "joao.santos@email.com", true, "João Santos", "+55 21 98765-4321", new DateTime(2024, 3, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), "Rua Pampulha, 789 - Savassi, Belo Horizonte - MG", new DateTime(1988, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "ana.costa@email.com", true, "Ana Paula Costa", "+55 31 97654-3210", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), "Rua dos Andradas, 321 - Centro Histórico, Porto Alegre - RS", new DateTime(1992, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pedro.oliveira@email.com", true, "Pedro Oliveira", "+55 51 96543-2109", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d5555555-5555-5555-5555-555555555555"), "SHIS QI 15, Conjunto 12, Casa 5 - Lago Sul, Brasília - DF", new DateTime(1987, 9, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "carla.mendes@email.com", true, "Carla Mendes", "+55 61 95432-1098", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d6666666-6666-6666-6666-666666666666"), "Alameda Santos, 654 - Jardins, São Paulo - SP", new DateTime(1995, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "lucas.ferreira@email.com", true, "Lucas Ferreira", "+55 11 94321-0987", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d7777777-7777-7777-7777-777777777777"), "Rua Visconde de Pirajá, 987 - Ipanema, Rio de Janeiro - RJ", new DateTime(1993, 4, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "juliana.barbosa@email.com", true, "Juliana Barbosa", "+55 21 93210-9876", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d8888888-8888-8888-8888-888888888888"), "Av. Afonso Pena, 234 - Centro, Belo Horizonte - MG", new DateTime(1989, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "ricardo.almeida@email.com", true, "Ricardo Almeida", "+55 31 92109-8765", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Image", "IsActive", "MinStockLevel", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000000"), "Águas", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Água mineral Crystal em garrafa Pet de 500ml", "agua_crystal_pet_500ml.jpg", true, 60, "Água Crystal Pet 500ml", 2.20m, 600, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Cervejas", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Brahma Pilsen em lata de 350ml", "brahma_lata_350ml.jpg", true, 50, "Brahma Lata 350ml", 3.50m, 500, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Cervejas", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Skol Pilsen em lata de 269ml", "skol_lata_269ml.jpg", true, 75, "Skol Lata 269ml", 2.80m, 750, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), "Cervejas", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Antarctica Original em lata de 350ml", "antarctica_lata_350ml.jpg", true, 30, "Antarctica Original Lata 350ml", 3.80m, 300, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), "Cervejas Premium", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Stella Artois Premium em garrafa de 550ml", "stella_garrafa_550ml.jpg", true, 20, "Stella Artois Garrafa 550ml", 8.90m, 200, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), "Cervejas Importadas", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Corona Extra mexicana em garrafa de 355ml", "corona_garrafa_355ml.jpg", true, 15, "Corona Extra Garrafa 355ml", 12.50m, 150, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), "Refrigerantes", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Refrigerante Guaraná Antarctica em lata de 350ml", "guarana_lata_350ml.jpg", true, 40, "Guaraná Antarctica Lata 350ml", 4.20m, 400, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b7777777-7777-7777-7777-777777777777"), "Refrigerantes", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Refrigerante Pepsi Twist com limão em lata de 350ml", "pepsi_twist_lata_350ml.jpg", true, 35, "Pepsi Twist Lata 350ml", 3.90m, 350, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b8888888-8888-8888-8888-888888888888"), "Isotônicos", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Bebida isotônica H2OH! sabor limão em garrafa Pet de 500ml", "h2oh_limao_pet_500ml.jpg", true, 25, "H2OH! Limão Pet 500ml", 5.50m, 250, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b9999999-9999-9999-9999-999999999999"), "Energéticos", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Energético Fusion original em garrafa Pet de 500ml", "fusion_pet_500ml.jpg", true, 18, "Fusion Pet 500ml", 7.80m, 180, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Image", "MinStockLevel", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[] { new Guid("fa000000-0000-0000-0000-000000000000"), "Cervejas Premium", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Cerveja Bohemia Pilsen premium em garrafa de 600ml", "bohemia_garrafa_600ml.jpg", 12, "Bohemia Pilsen Garrafa 600ml", 9.50m, 120, new DateTime(2025, 9, 3, 19, 4, 8, 940, DateTimeKind.Utc).AddTicks(2781) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "Phone", "Role", "Status", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pedro.oliveira@email.com", "customer123", "+55 51 96543-2109", "Customer", "Suspended", new DateTime(2025, 9, 28, 19, 4, 8, 942, DateTimeKind.Utc).AddTicks(34), "customer.pedro" },
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "admin@ambev.com", "admin123", "+55 11 99999-9999", "Admin", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "admin" },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "carlos.silva@ambev.com", "manager123", "+55 11 98888-8888", "Manager", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "manager.carlos" },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "ana.costa@ambev.com", "manager123", "+55 21 97777-7777", "Manager", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "manager.ana" },
                    { new Guid("a4444444-4444-4444-4444-444444444444"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "roberto.santos@ambev.com", "manager123", "+55 31 96666-6666", "Manager", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "manager.roberto" },
                    { new Guid("a5555555-5555-5555-5555-555555555555"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "fernanda.lima@ambev.com", "manager123", "+55 51 95555-5555", "Manager", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "manager.fernanda" },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "joao.oliveira@ambev.com", "manager123", "+55 61 94444-4444", "Manager", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "manager.joao" },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "maria.silva@email.com", "customer123", "+55 11 99876-5432", "Customer", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "customer.maria" },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "joao.santos@email.com", "customer123", "+55 21 98765-4321", "Customer", "Active", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "customer.joao" },
                    { new Guid("a9999999-9999-9999-9999-999999999999"), new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "ana.costa@email.com", "customer123", "+55 31 97654-3210", "Customer", "Inactive", new DateTime(2025, 9, 23, 19, 4, 8, 942, DateTimeKind.Utc).AddTicks(30), "customer.ana" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "BranchId", "CreatedAt", "CustomerId", "SaleDate", "SaleNumber", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("e0000000-0000-0000-0000-000000000000"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 25, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1075), new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 25, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1073), "SAL-2024-010", "Cancelled", new DateTime(2025, 9, 26, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1077) },
                    { new Guid("e1111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 3, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(926), new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 3, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(921), "SAL-2024-001", "Confirmed", new DateTime(2025, 9, 3, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(926) },
                    { new Guid("e2222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 5, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(939), new Guid("d2222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 5, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(936), "SAL-2024-002", "Confirmed", new DateTime(2025, 9, 5, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(939) },
                    { new Guid("e3333333-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 9, 8, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(949), new Guid("d3333333-3333-3333-3333-333333333333"), new DateTime(2025, 9, 8, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(946), "SAL-2024-003", "Confirmed", new DateTime(2025, 9, 8, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(949) },
                    { new Guid("e4444444-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 9, 11, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1016), new Guid("d4444444-4444-4444-4444-444444444444"), new DateTime(2025, 9, 11, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1013), "SAL-2024-004", "Confirmed", new DateTime(2025, 9, 11, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1017) },
                    { new Guid("e5555555-5555-5555-5555-555555555555"), new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 9, 13, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1025), new Guid("d5555555-5555-5555-5555-555555555555"), new DateTime(2025, 9, 13, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1023), "SAL-2024-005", "Confirmed", new DateTime(2025, 9, 13, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1026) },
                    { new Guid("e6666666-6666-6666-6666-666666666666"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 15, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1035), new Guid("d6666666-6666-6666-6666-666666666666"), new DateTime(2025, 9, 15, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1033), "SAL-2024-006", "Confirmed", new DateTime(2025, 9, 15, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1036) },
                    { new Guid("e7777777-7777-7777-7777-777777777777"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 18, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1044), new Guid("d7777777-7777-7777-7777-777777777777"), new DateTime(2025, 9, 18, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1042), "SAL-2024-007", "Confirmed", new DateTime(2025, 9, 18, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1044) },
                    { new Guid("e8888888-8888-8888-8888-888888888888"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2025, 9, 21, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1052), new Guid("d8888888-8888-8888-8888-888888888888"), new DateTime(2025, 9, 21, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1050), "SAL-2024-008", "Confirmed", new DateTime(2025, 9, 21, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1054) }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "BranchId", "CreatedAt", "CustomerId", "SaleDate", "SaleNumber", "UpdatedAt" },
                values: new object[] { new Guid("e9999999-9999-9999-9999-999999999999"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 23, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1062), new Guid("d1111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 23, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1060), "SAL-2024-009", new DateTime(2025, 9, 23, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(1062) });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "DiscountPercent", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000000"), new DateTime(2025, 9, 18, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6648), 20m, new Guid("b7777777-7777-7777-7777-777777777777"), 15, new Guid("e7777777-7777-7777-7777-777777777777"), 46.80m, 3.90m, null },
                    { new Guid("f1111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 3, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6618), 10m, new Guid("b1111111-1111-1111-1111-111111111111"), 6, new Guid("e1111111-1111-1111-1111-111111111111"), 18.90m, 3.50m, null }
                });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[] { new Guid("f2222222-2222-2222-2222-222222222222"), new DateTime(2025, 9, 3, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6627), new Guid("b6666666-6666-6666-6666-666666666666"), 2, new Guid("e1111111-1111-1111-1111-111111111111"), 8.40m, 4.20m, null });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "DiscountPercent", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f3333333-3333-3333-3333-333333333333"), new DateTime(2025, 9, 5, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6630), 20m, new Guid("b2222222-2222-2222-2222-222222222222"), 12, new Guid("e2222222-2222-2222-2222-222222222222"), 26.88m, 2.80m, null },
                    { new Guid("f4444444-4444-4444-4444-444444444444"), new DateTime(2025, 9, 8, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6634), 10m, new Guid("b4444444-4444-4444-4444-444444444444"), 4, new Guid("e3333333-3333-3333-3333-333333333333"), 32.04m, 8.90m, null }
                });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[] { new Guid("f5555555-5555-5555-5555-555555555555"), new DateTime(2025, 9, 8, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6637), new Guid("b8888888-8888-8888-8888-888888888888"), 3, new Guid("e3333333-3333-3333-3333-333333333333"), 16.50m, 5.50m, null });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "DiscountPercent", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f6666666-6666-6666-6666-666666666666"), new DateTime(2025, 9, 11, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6639), 20m, new Guid("b5555555-5555-5555-5555-555555555555"), 20, new Guid("e4444444-4444-4444-4444-444444444444"), 200.00m, 12.50m, null },
                    { new Guid("f7777777-7777-7777-7777-777777777777"), new DateTime(2025, 9, 13, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6642), 20m, new Guid("b0000000-0000-0000-0000-000000000000"), 10, new Guid("e5555555-5555-5555-5555-555555555555"), 17.60m, 2.20m, null },
                    { new Guid("f8888888-8888-8888-8888-888888888888"), new DateTime(2025, 9, 13, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6644), 10m, new Guid("b9999999-9999-9999-9999-999999999999"), 5, new Guid("e5555555-5555-5555-5555-555555555555"), 35.10m, 7.80m, null },
                    { new Guid("f9999999-9999-9999-9999-999999999999"), new DateTime(2025, 9, 15, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6646), 10m, new Guid("b3333333-3333-3333-3333-333333333333"), 8, new Guid("e6666666-6666-6666-6666-666666666666"), 27.36m, 3.80m, null }
                });

            migrationBuilder.InsertData(
                table: "SaleItems",
                columns: new[] { "Id", "CreatedAt", "ProductId", "Quantity", "SaleId", "TotalPrice", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("fa000000-0000-0000-0000-000000000000"), new DateTime(2025, 9, 21, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6650), new Guid("b1111111-1111-1111-1111-111111111111"), 1, new Guid("e8888888-8888-8888-8888-888888888888"), 3.50m, 3.50m, null },
                    { new Guid("fb000000-0000-0000-0000-000000000000"), new DateTime(2025, 9, 21, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(6652), new Guid("b2222222-2222-2222-2222-222222222222"), 1, new Guid("e8888888-8888-8888-8888-888888888888"), 2.80m, 2.80m, null },
                    { new Guid("fc000000-0000-0000-0000-000000000000"), new DateTime(2025, 9, 23, 19, 4, 8, 941, DateTimeKind.Utc).AddTicks(7327), new Guid("b6666666-6666-6666-6666-666666666666"), 3, new Guid("e9999999-9999-9999-9999-999999999999"), 12.60m, 4.20m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Email",
                table: "Branches",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                table: "Branches",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                table: "Products",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BranchId",
                table: "Sales",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerId",
                table: "Sales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleDate",
                table: "Sales",
                column: "SaleDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleNumber",
                table: "Sales",
                column: "SaleNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
