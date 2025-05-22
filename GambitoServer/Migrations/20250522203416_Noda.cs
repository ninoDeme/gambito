using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace GambitoServer.Migrations
{
    /// <inheritdoc />
    public partial class Noda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalTime>(
                name: "hora_ini",
                table: "linha_producao_hora",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalTime>(
                name: "hora_fim",
                table: "linha_producao_hora",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalTime>(
                name: "hora",
                table: "linha_producao_hora",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "hora_ini",
                table: "linha_producao_hora",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(LocalTime),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "hora_fim",
                table: "linha_producao_hora",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(LocalTime),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "hora",
                table: "linha_producao_hora",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(LocalTime),
                oldType: "time");
        }
    }
}
