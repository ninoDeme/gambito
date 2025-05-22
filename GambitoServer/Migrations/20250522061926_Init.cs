using System;
using GambitoServer.Db;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GambitoServer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:tipo_hora", "banco_horas,hora_extra");

            migrationBuilder.CreateTable(
                name: "defeitos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("defeito_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "etapa",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("etapa_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "funcao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("funcao_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "linha_producao",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("linha_producao_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tempo_peca = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("produto_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    funcao = table.Column<int>(type: "integer", nullable: true),
                    encarregado = table.Column<int>(type: "integer", nullable: true),
                    invativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("funcionario_pkey", x => x.id);
                    table.ForeignKey(
                        name: "funcionario_encarregado_fkey",
                        column: x => x.encarregado,
                        principalTable: "funcionario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "funcionario_funcao_fkey",
                        column: x => x.funcao,
                        principalTable: "funcao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_dia",
                columns: table => new
                {
                    linha_producao = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<DateOnly>(type: "date", nullable: false),
                    invativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("linha_producao_dia_pkey", x => new { x.linha_producao, x.data });
                    table.ForeignKey(
                        name: "linha_producao_dia_linha_producao_fkey",
                        column: x => x.linha_producao,
                        principalTable: "linha_producao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    produto = table.Column<int>(type: "integer", nullable: false),
                    qtd_pecas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pedido_pkey", x => x.id);
                    table.ForeignKey(
                        name: "pedido_produto_fkey",
                        column: x => x.produto,
                        principalTable: "produto",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora_etapa",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    linha_producao = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<DateOnly>(type: "date", nullable: true),
                    etapa = table.Column<int>(type: "integer", nullable: true),
                    ordem = table.Column<int>(type: "integer", nullable: false),
                    segundos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("linha_producao_hora_etapa_pkey", x => x.id);
                    table.ForeignKey(
                        name: "linha_producao_hora_etapa_etapa_fkey",
                        column: x => x.etapa,
                        principalTable: "etapa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "linha_producao_hora_etapa_linha_producao_data_fkey",
                        columns: x => new { x.linha_producao, x.data },
                        principalTable: "linha_producao_dia",
                        principalColumns: new[] { "linha_producao", "data" });
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    linha_producao = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<DateOnly>(type: "date", nullable: true),
                    hora = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    pedido = table.Column<int>(type: "integer", nullable: false),
                    qtd_produzido = table.Column<int>(type: "integer", nullable: true),
                    paralizacao = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    hora_ini = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    hora_fim = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    tipo = table.Column<TipoHora>(type: "tipo_hora", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("linha_producao_hora_pkey", x => x.id);
                    table.ForeignKey(
                        name: "linha_producao_hora_linha_producao_data_fkey",
                        columns: x => new { x.linha_producao, x.data },
                        principalTable: "linha_producao_dia",
                        principalColumns: new[] { "linha_producao", "data" });
                    table.ForeignKey(
                        name: "linha_producao_hora_pedido_fkey",
                        column: x => x.pedido,
                        principalTable: "pedido",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "linha_producao_hora_etapa_funcionario",
                columns: table => new
                {
                    linha_producao_hora_etapa = table.Column<int>(type: "integer", nullable: false),
                    funcionario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("linha_producao_hora_etapa_funcionario_pkey", x => new { x.linha_producao_hora_etapa, x.funcionario });
                    table.ForeignKey(
                        name: "linha_producao_hora_etapa_funcio_linha_producao_hora_etapa_fkey",
                        column: x => x.linha_producao_hora_etapa,
                        principalTable: "linha_producao_hora_etapa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "linha_producao_hora_etapa_funcionario_funcionario_fkey",
                        column: x => x.funcionario,
                        principalTable: "funcionario",
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
                    table.PrimaryKey("linha_producao_hora_defeito_pkey", x => new { x.linha_producao_hora, x.retrabalhado, x.defeito });
                    table.ForeignKey(
                        name: "linha_producao_hora_defeito_defeito_fkey",
                        column: x => x.defeito,
                        principalTable: "defeitos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "linha_producao_hora_defeito_linha_producao_hora_fkey",
                        column: x => x.linha_producao_hora,
                        principalTable: "linha_producao_hora",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_defeitos_nome",
                table: "defeitos",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_funcao_nome",
                table: "funcao",
                column: "nome",
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
                name: "ix_linha_producao_hora_linha_producao_data",
                table: "linha_producao_hora",
                columns: new[] { "linha_producao", "data" });

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_pedido",
                table: "linha_producao_hora",
                column: "pedido");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_defeito_defeito",
                table: "linha_producao_hora_defeito",
                column: "defeito");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_etapa_etapa",
                table: "linha_producao_hora_etapa",
                column: "etapa");

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_etapa_linha_producao_data",
                table: "linha_producao_hora_etapa",
                columns: new[] { "linha_producao", "data" });

            migrationBuilder.CreateIndex(
                name: "ix_linha_producao_hora_etapa_funcionario_funcionario",
                table: "linha_producao_hora_etapa_funcionario",
                column: "funcionario");

            migrationBuilder.CreateIndex(
                name: "ix_pedido_produto",
                table: "pedido",
                column: "produto");

            migrationBuilder.CreateIndex(
                name: "ix_produto_nome",
                table: "produto",
                column: "nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "linha_producao_hora_defeito");

            migrationBuilder.DropTable(
                name: "linha_producao_hora_etapa_funcionario");

            migrationBuilder.DropTable(
                name: "defeitos");

            migrationBuilder.DropTable(
                name: "linha_producao_hora");

            migrationBuilder.DropTable(
                name: "linha_producao_hora_etapa");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "etapa");

            migrationBuilder.DropTable(
                name: "linha_producao_dia");

            migrationBuilder.DropTable(
                name: "funcao");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "linha_producao");
        }
    }
}
