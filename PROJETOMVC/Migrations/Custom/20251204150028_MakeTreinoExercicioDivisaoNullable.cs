using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROJETOMVC.Migrations.Custom
{
    /// <inheritdoc />
    public partial class MakeTreinoExercicioDivisaoNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Divisao",
                table: "TreinoExercicios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "admin123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Divisao",
                table: "TreinoExercicios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$10$CwTycUXWue0Thq9StjUM0uJ8b6v8zjYh7rYqJv1J8fYQkYQkYQkYQ");
        }
    }
}
