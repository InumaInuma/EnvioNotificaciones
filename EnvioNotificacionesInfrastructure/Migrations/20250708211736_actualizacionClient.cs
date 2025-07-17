using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnvioNotificacionesInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    CodCli = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEmp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.CodCli);
                });

            migrationBuilder.CreateTable(
                name: "LoginUsuarios",
                columns: table => new
                {
                    LogUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "LoginUsuario"),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginUsuarios", x => x.LogUId);
                });

            migrationBuilder.CreateTable(
                name: "TipoChequeo",
                columns: table => new
                {
                    CodTCh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesTCh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoChequeo", x => x.CodTCh);
                });

            migrationBuilder.CreateTable(
                name: "Cita",
                columns: table => new
                {
                    CodCit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodTDo = table.Column<int>(type: "int", nullable: true),
                    CodCPa = table.Column<int>(type: "int", nullable: true),
                    CodCli = table.Column<int>(type: "int", nullable: false),
                    FecCit = table.Column<DateTime>(type: "datetime", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    ApePat = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    ApeMat = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    SexPac = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    FecNac = table.Column<DateTime>(type: "datetime", nullable: true),
                    NumDId = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: true),
                    CodEmp = table.Column<int>(type: "int", nullable: true),
                    CodSed = table.Column<int>(type: "int", nullable: true),
                    CodTCl = table.Column<int>(type: "int", nullable: true),
                    NumOrd = table.Column<int>(type: "int", nullable: true),
                    NumTic = table.Column<int>(type: "int", nullable: true),
                    FecTic = table.Column<DateTime>(type: "datetime", nullable: true),
                    CodDes = table.Column<int>(type: "int", nullable: true),
                    PueAct = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CenCos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ZonLab = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AreTra = table.Column<int>(type: "int", nullable: true),
                    CodTCh = table.Column<int>(type: "int", nullable: false),
                    Observ = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    SubCon = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FactuA = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    EdaPac = table.Column<int>(type: "int", nullable: true),
                    Responsable = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Gerenc = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IndCon = table.Column<bool>(type: "bit", nullable: true),
                    IndReg = table.Column<bool>(type: "bit", nullable: true),
                    AudCre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AudMod = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AudCon = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TipPag = table.Column<int>(type: "int", nullable: true),
                    NroCPa = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NroReq = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TipoCarga = table.Column<int>(type: "int", nullable: true),
                    PruCov = table.Column<bool>(type: "bit", nullable: true),
                    CodFic = table.Column<int>(type: "int", nullable: true),
                    CodHor = table.Column<int>(type: "int", nullable: true),
                    CorElec = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    InHouse = table.Column<bool>(type: "bit", nullable: true),
                    DesDir = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    EmoCov = table.Column<bool>(type: "bit", nullable: true),
                    FlagCPac = table.Column<bool>(type: "bit", nullable: true),
                    PruMol = table.Column<bool>(type: "bit", nullable: true),
                    EmoMol = table.Column<bool>(type: "bit", nullable: true),
                    PruAnt = table.Column<bool>(type: "bit", nullable: true),
                    EmoAnt = table.Column<bool>(type: "bit", nullable: true),
                    PruEclia = table.Column<bool>(type: "bit", nullable: true),
                    EmoEclia = table.Column<bool>(type: "bit", nullable: true),
                    PruElisa = table.Column<bool>(type: "bit", nullable: true),
                    EmoElisa = table.Column<bool>(type: "bit", nullable: true),
                    CodTEx = table.Column<int>(type: "int", nullable: true),
                    IndEnv = table.Column<int>(type: "int", nullable: true),
                    ExaAdi = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Campania = table.Column<bool>(type: "bit", nullable: true),
                    IndEnv48 = table.Column<bool>(type: "bit", nullable: true),
                    FecEnv48 = table.Column<DateTime>(type: "datetime", nullable: true),
                    IndEnv24 = table.Column<bool>(type: "bit", nullable: true),
                    FecEnv24 = table.Column<DateTime>(type: "datetime", nullable: true),
                    IndECM = table.Column<bool>(type: "bit", nullable: true),
                    RefCodCli = table.Column<int>(type: "int", nullable: true),
                    FecReg = table.Column<DateTime>(type: "datetime", nullable: true),
                    NoAsis = table.Column<int>(type: "int", nullable: true),
                    FecNAs = table.Column<DateTime>(type: "datetime", nullable: true),
                    IndAAs = table.Column<int>(type: "int", nullable: true),
                    NroCel = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    NroTlf = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    CodOri = table.Column<int>(type: "int", nullable: true),
                    IndEWa = table.Column<bool>(type: "bit", nullable: true),
                    FecEWa = table.Column<DateTime>(type: "datetime", nullable: true),
                    MsjErrW = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IndRWa = table.Column<bool>(type: "bit", nullable: true),
                    FecRWa = table.Column<DateTime>(type: "datetime", nullable: true),
                    MsjERW = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    NroInt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cita", x => x.CodCit);
                    table.ForeignKey(
                        name: "FK_Cita_Cliente",
                        column: x => x.CodCli,
                        principalTable: "Cliente",
                        principalColumn: "CodCli");
                    table.ForeignKey(
                        name: "FK__Cita_TipoChequeo",
                        column: x => x.CodTCh,
                        principalTable: "TipoChequeo",
                        principalColumn: "CodTCh");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cita_CodCli",
                table: "Cita",
                column: "CodCli");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_CodTCh",
                table: "Cita",
                column: "CodTCh");

            migrationBuilder.CreateIndex(
                name: "IX_LoginUsuarios_Username",
                table: "LoginUsuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cita");

            migrationBuilder.DropTable(
                name: "LoginUsuarios");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "TipoChequeo");
        }
    }
}
