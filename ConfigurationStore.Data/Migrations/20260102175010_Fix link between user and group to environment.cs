using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigurationStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixlinkbetweenuserandgrouptoenvironment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_ProjectEnvironments_ProjectEnvironmentId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProjectEnvironments_ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_ProjectEnvironmentId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "ProjectEnvironmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProjectEnvironmentId",
                table: "UserGroups");

            migrationBuilder.CreateTable(
                name: "ProjectEnvironmentUser",
                columns: table => new
                {
                    ProjectEnvironmentsId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEnvironmentUser", x => new { x.ProjectEnvironmentsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProjectEnvironmentUser_ProjectEnvironments_ProjectEnvironme~",
                        column: x => x.ProjectEnvironmentsId,
                        principalTable: "ProjectEnvironments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectEnvironmentUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEnvironmentUserGroup",
                columns: table => new
                {
                    ProjectEnvironmentsId = table.Column<int>(type: "integer", nullable: false),
                    UserGroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEnvironmentUserGroup", x => new { x.ProjectEnvironmentsId, x.UserGroupsId });
                    table.ForeignKey(
                        name: "FK_ProjectEnvironmentUserGroup_ProjectEnvironments_ProjectEnvi~",
                        column: x => x.ProjectEnvironmentsId,
                        principalTable: "ProjectEnvironments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectEnvironmentUserGroup_UserGroups_UserGroupsId",
                        column: x => x.UserGroupsId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEnvironmentUser_UsersId",
                table: "ProjectEnvironmentUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEnvironmentUserGroup_UserGroupsId",
                table: "ProjectEnvironmentUserGroup",
                column: "UserGroupsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectEnvironmentUser");

            migrationBuilder.DropTable(
                name: "ProjectEnvironmentUserGroup");

            migrationBuilder.AddColumn<int>(
                name: "ProjectEnvironmentId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectEnvironmentId",
                table: "UserGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectEnvironmentId",
                table: "Users",
                column: "ProjectEnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_ProjectEnvironmentId",
                table: "UserGroups",
                column: "ProjectEnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_ProjectEnvironments_ProjectEnvironmentId",
                table: "UserGroups",
                column: "ProjectEnvironmentId",
                principalTable: "ProjectEnvironments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProjectEnvironments_ProjectEnvironmentId",
                table: "Users",
                column: "ProjectEnvironmentId",
                principalTable: "ProjectEnvironments",
                principalColumn: "Id");
        }
    }
}
