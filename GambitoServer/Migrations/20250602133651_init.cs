using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GambitoServer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:tipo_hora", "banco_horas,hora_extra");

            migrationBuilder.CreateTable(
                name: "organizacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizacao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "defeito",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_defeito", x => x.id);
                    table.ForeignKey(
                        name: "fk_defeito_organizacaos_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "etapa",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_etapa", x => x.id);
                    table.ForeignKey(
                        name: "fk_etapa_organizacaos_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "funcao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_funcao", x => x.id);
                    table.ForeignKey(
                        name: "fk_funcao_organizacaos_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "linha_producao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao", x => x.id);
                    table.ForeignKey(
                        name: "fk_linha_producao_organizacaos_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tempo_peca = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produto", x => x.id);
                    table.ForeignKey(
                        name: "fk_produto_organizacao_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_claim",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claim_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claim",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claim_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                schema: "auth",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_login_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_organizacao",
                columns: table => new
                {
                    usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    organizacao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_organizacao", x => new { x.usuario, x.organizacao });
                    table.ForeignKey(
                        name: "fk_user_organizacao_organizacao_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_organizacao_user_usuario",
                        column: x => x.usuario,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_token_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    organizacao = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    funcao = table.Column<int>(type: "integer", nullable: false),
                    encarregado = table.Column<int>(type: "integer", nullable: true),
                    invativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_funcionario", x => x.id);
                    table.ForeignKey(
                        name: "fk_funcionario_funcao_funcao",
                        column: x => x.funcao,
                        principalTable: "funcao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_funcionario_funcionario_encarregado",
                        column: x => x.encarregado,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_funcionario_organizacaos_organizacao",
                        column: x => x.organizacao,
                        principalTable: "organizacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_dia",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    linha_producao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    data = table.Column<LocalDate>(type: "date", nullable: true),
                    invativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao_dia", x => x.id);
                    table.ForeignKey(
                        name: "fk_linha_producao_dia_linha_producao_linha_producao",
                        column: x => x.linha_producao,
                        principalTable: "linha_producao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "produto_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    produto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produto_config", x => x.id);
                    table.ForeignKey(
                        name: "fk_produto_config_produtos_produto",
                        column: x => x.produto,
                        principalTable: "produto",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    linha_producao_dia = table.Column<int>(type: "integer", nullable: false),
                    hora = table.Column<LocalTime>(type: "time without time zone", nullable: false),
                    produto_config = table.Column<int>(type: "integer", nullable: false),
                    qtd_produzido = table.Column<int>(type: "integer", nullable: true),
                    paralizacao = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    hora_ini = table.Column<LocalTime>(type: "time without time zone", nullable: true),
                    hora_fim = table.Column<LocalTime>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao_hora", x => x.id);
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_linha_producao_dia_linha_producao_dia",
                        column: x => x.linha_producao_dia,
                        principalTable: "linha_producao_dia",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_produto_config_produto_config",
                        column: x => x.produto_config,
                        principalTable: "produto_config",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "produto_config_etapa",
                columns: table => new
                {
                    produto_config = table.Column<int>(type: "integer", nullable: false),
                    etapa = table.Column<int>(type: "integer", nullable: false),
                    segundos = table.Column<int>(type: "integer", nullable: false),
                    ordem = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produto_config_etapa", x => new { x.produto_config, x.etapa });
                    table.ForeignKey(
                        name: "fk_produto_config_etapa_etapa_etapa",
                        column: x => x.etapa,
                        principalTable: "etapa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_produto_config_etapa_produto_config_produto_config",
                        column: x => x.produto_config,
                        principalTable: "produto_config",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora_defeito",
                columns: table => new
                {
                    linha_producao_hora = table.Column<int>(type: "integer", nullable: false),
                    retrabalhado = table.Column<bool>(type: "boolean", nullable: false),
                    defeito = table.Column<int>(type: "integer", nullable: false),
                    qtd_pecas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao_hora_defeito", x => new { x.linha_producao_hora, x.retrabalhado, x.defeito });
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_defeito_defeito_defeito",
                        column: x => x.defeito,
                        principalTable: "defeito",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_defeito_linha_producao_hora_linha_produ",
                        column: x => x.linha_producao_hora,
                        principalTable: "linha_producao_hora",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora_etapa",
                columns: table => new
                {
                    linha_producao_hora = table.Column<int>(type: "integer", nullable: false),
                    etapa = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao_hora_etapa", x => new { x.linha_producao_hora, x.etapa });
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_etapa_etapa_etapa",
                        column: x => x.etapa,
                        principalTable: "etapa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_etapa_linha_producao_hora_linha_produca",
                        column: x => x.linha_producao_hora,
                        principalTable: "linha_producao_hora",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora_etapa_funcionario",
                columns: table => new
                {
                    linha_producao_hora = table.Column<int>(type: "integer", nullable: false),
                    etapa = table.Column<int>(type: "integer", nullable: false),
                    funcionario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_linha_producao_hora_etapa_funcionario", x => new { x.linha_producao_hora, x.etapa, x.funcionario });
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_etapa_funcionario_funcionario_funcionar",
                        column: x => x.funcionario,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_linha_producao_hora_etapa_funcionario_linha_producao_hora_e",
                        columns: x => new { x.linha_producao_hora, x.etapa },
                        principalTable: "linha_producao_hora_etapa",
                        principalColumns: new[] { "linha_producao_hora", "etapa" });
                });

            migrationBuilder.InsertData(
                table: "organizacao",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Org de desenvolvimento" },
                    { 2, "Org de testes" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "role",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("c2a4a4a9-4d9d-6f63-be4c-8d9c3a30d1f3"), null, "Admin", "ADMIN" },
                    { new Guid("d3b5b5ba-5eae-7a74-cf5d-9ead4b41e2a4"), null, "Operator", "OPERATOR" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("a0e2e2e7-2b7f-4d41-9c2a-6b7a1e18b9d1"), 0, null, "admin@fabrica.com", true, false, null, "ADMIN@FABRICA.COM", "ADMIN@FABRICA.COM", "AQAAAAIAAYagAAAAEFUG/PMFzzxt549qLdgva0XLTbAIfMYCFXLSDDvZ/BIygz06h3ngihtvKAa/ryEvWg==", null, false, null, false, "admin@fabrica.com" },
                    { new Guid("b1f3f3f8-3c8e-5e52-ad3b-7c8b2f29c0e2"), 0, null, "operator@fabrica.com", true, false, null, "OPERATOR@FABRICA.COM", "OPERATOR@FABRICA.COM", "AQAAAAIAAYagAAAAEIv3lOaj9hoHyu14WS9meY5CZoFq+ZKnMgJKM9zV/28dXFOLgo+EP+ZSuQym1FRBLg==", null, false, null, false, "operator@fabrica.com" }
                });

            migrationBuilder.InsertData(
                table: "defeito",
                columns: new[] { "id", "nome", "organizacao" },
                values: new object[,]
                {
                    { 1, "Risco Profundo", 1 },
                    { 2, "Amassado Leve", 1 },
                    { 3, "Falha na Pintura", 1 },
                    { 4, "Risco Superficial", 2 }
                });

            migrationBuilder.InsertData(
                table: "etapa",
                columns: new[] { "id", "nome", "organizacao" },
                values: new object[,]
                {
                    { 1, "Corte", 1 },
                    { 2, "Montagem", 1 },
                    { 4, "Controle de Qualidade", 1 },
                    { 5, "Embalagem", 2 }
                });

            migrationBuilder.InsertData(
                table: "funcao",
                columns: new[] { "id", "nome", "organizacao" },
                values: new object[,]
                {
                    { 1, "Supervisor de Produção", 1 },
                    { 2, "Operador de Máquina", 1 },
                    { 3, "Gerente de Qualidade", 1 },
                    { 4, "Auxiliar de Produção", 2 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao",
                columns: new[] { "id", "descricao", "organizacao" },
                values: new object[,]
                {
                    { 1, "Linha de Montagem A", 1 },
                    { 2, "Linha de Pintura B", 1 },
                    { 3, "Linha de Testes C", 2 }
                });

            migrationBuilder.InsertData(
                table: "produto",
                columns: new[] { "id", "nome", "organizacao", "tempo_peca" },
                values: new object[,]
                {
                    { 1, "Produto Alfa", 1, 120 },
                    { 2, "Produto Beta", 1, 180 },
                    { 3, "Produto Gama", 2, 90 }
                });

            migrationBuilder.InsertData(
                table: "user_organizacao",
                columns: new[] { "organizacao", "usuario" },
                values: new object[,]
                {
                    { 1, new Guid("a0e2e2e7-2b7f-4d41-9c2a-6b7a1e18b9d1") },
                    { 2, new Guid("a0e2e2e7-2b7f-4d41-9c2a-6b7a1e18b9d1") },
                    { 1, new Guid("b1f3f3f8-3c8e-5e52-ad3b-7c8b2f29c0e2") }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("c2a4a4a9-4d9d-6f63-be4c-8d9c3a30d1f3"), new Guid("a0e2e2e7-2b7f-4d41-9c2a-6b7a1e18b9d1") },
                    { new Guid("d3b5b5ba-5eae-7a74-cf5d-9ead4b41e2a4"), new Guid("b1f3f3f8-3c8e-5e52-ad3b-7c8b2f29c0e2") }
                });

            migrationBuilder.InsertData(
                table: "funcionario",
                columns: new[] { "id", "encarregado", "funcao", "nome", "organizacao" },
                values: new object[,]
                {
                    { 1, null, 1, "Carlos Silva", 1 },
                    { 4, null, 3, "Mariana Lima", 1 },
                    { 5, null, 4, "Pedro Alves", 2 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao_dia",
                columns: new[] { "id", "data", "linha_producao" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDate(2025, 5, 24), 1 },
                    { 2, new NodaTime.LocalDate(2025, 5, 25), 1 },
                    { 3, new NodaTime.LocalDate(2025, 5, 25), 2 }
                });

            migrationBuilder.InsertData(
                table: "produto_config",
                columns: new[] { "id", "produto" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "funcionario",
                columns: new[] { "id", "encarregado", "funcao", "nome", "organizacao" },
                values: new object[] { 2, 1, 2, "Ana Pereira", 1 });

            migrationBuilder.InsertData(
                table: "funcionario",
                columns: new[] { "id", "encarregado", "funcao", "invativo", "nome", "organizacao" },
                values: new object[] { 3, 1, 2, true, "João Costa", 1 });

            migrationBuilder.InsertData(
                table: "linha_producao_hora",
                columns: new[] { "id", "hora", "hora_fim", "hora_ini", "linha_producao_dia", "produto_config", "qtd_produzido" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalTime(8, 0), null, null, 1, 1, 10 },
                    { 2, new NodaTime.LocalTime(9, 0), null, null, 1, 1, 12 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao_hora",
                columns: new[] { "id", "hora", "hora_fim", "hora_ini", "linha_producao_dia", "paralizacao", "produto_config", "qtd_produzido" },
                values: new object[] { 3, new NodaTime.LocalTime(10, 0), null, null, 1, true, 1, 0 });

            migrationBuilder.InsertData(
                table: "linha_producao_hora",
                columns: new[] { "id", "hora", "hora_fim", "hora_ini", "linha_producao_dia", "produto_config", "qtd_produzido" },
                values: new object[] { 4, new NodaTime.LocalTime(8, 0), null, null, 3, 2, 8 });

            migrationBuilder.InsertData(
                table: "produto_config_etapa",
                columns: new[] { "etapa", "produto_config", "ordem", "segundos" },
                values: new object[,]
                {
                    { 2, 1, 2, 60 },
                    { 4, 1, 3, 30 },
                    { 1, 2, 1, 40 },
                    { 2, 2, 2, 70 },
                    { 4, 2, 4, 30 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao_hora_defeito",
                columns: new[] { "defeito", "linha_producao_hora", "retrabalhado", "qtd_pecas" },
                values: new object[,]
                {
                    { 1, 1, false, 2 },
                    { 2, 1, true, 1 },
                    { 3, 2, false, 1 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao_hora_etapa",
                columns: new[] { "etapa", "linha_producao_hora" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "linha_producao_hora_etapa_funcionario",
                columns: new[] { "etapa", "funcionario", "linha_producao_hora" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 2, 1 },
                    { 2, 3, 1 },
                    { 2, 2, 2 },
                    { 4, 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_defeito_nome",
                table: "defeito",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_defeito_organizacao",
                table: "defeito",
                column: "organizacao");

            migrationBuilder.CreateIndex(
                name: "ix_etapa_organizacao_nome",
                table: "etapa",
                columns: new[] { "organizacao", "nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_funcao_organizacao_nome",
                table: "funcao",
                columns: new[] { "organizacao", "nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_funcionario_encarregado",
                table: "funcionario",
                column: "encarregado");

            migrationBuilder.CreateIndex(
                name: "ix_funcionario_funcao",
                table: "funcionario",
                column: "funcao");

            migrationBuilder.CreateIndex(
                name: "ix_funcionario_organizacao",
                table: "funcionario",
                column: "organizacao");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_organizacao",
                table: "linha_producao",
                column: "organizacao");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_dia_linha_producao_data",
                table: "linha_producao_dia",
                columns: new[] { "linha_producao", "data" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_linha_producao_dia",
                table: "linha_producao_hora",
                column: "linha_producao_dia");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_produto_config",
                table: "linha_producao_hora",
                column: "produto_config");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_defeito_defeito",
                table: "linha_producao_hora_defeito",
                column: "defeito");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_etapa_etapa",
                table: "linha_producao_hora_etapa",
                column: "etapa");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_etapa_funcionario_funcionario",
                table: "linha_producao_hora_etapa_funcionario",
                column: "funcionario");

            migrationBuilder.CreateIndex(
                name: "ix_produto_nome",
                table: "produto",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_produto_organizacao",
                table: "produto",
                column: "organizacao");

            migrationBuilder.CreateIndex(
                name: "ix_produto_config_produto",
                table: "produto_config",
                column: "produto");

            migrationBuilder.CreateIndex(
                name: "ix_produto_config_etapa_etapa",
                table: "produto_config_etapa",
                column: "etapa");

            migrationBuilder.CreateIndex(
                name: "ix_role_normalized_name",
                schema: "auth",
                table: "role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_claim_role_id",
                schema: "auth",
                table: "role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_normalized_email",
                schema: "auth",
                table: "user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_user_normalized_user_name",
                schema: "auth",
                table: "user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_claim_user_id",
                schema: "auth",
                table: "user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_login_user_id",
                schema: "auth",
                table: "user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_organizacao_organizacao",
                table: "user_organizacao",
                column: "organizacao");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_role_id",
                schema: "auth",
                table: "user_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "linha_producao_hora_defeito");

            migrationBuilder.DropTable(
                name: "linha_producao_hora_etapa_funcionario");

            migrationBuilder.DropTable(
                name: "produto_config_etapa");

            migrationBuilder.DropTable(
                name: "role_claim",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_claim",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_login",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_organizacao");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_token",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "defeito");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "linha_producao_hora_etapa");

            migrationBuilder.DropTable(
                name: "role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "funcao");

            migrationBuilder.DropTable(
                name: "etapa");

            migrationBuilder.DropTable(
                name: "linha_producao_hora");

            migrationBuilder.DropTable(
                name: "linha_producao_dia");

            migrationBuilder.DropTable(
                name: "produto_config");

            migrationBuilder.DropTable(
                name: "linha_producao");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "organizacao");
        }
    }
}
