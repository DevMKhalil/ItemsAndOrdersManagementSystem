using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemsAndOrdersManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('B6E3AF0D-9AAC-43C5-8A2A-9A28A0110799', 'admin', 'ADMIN')");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('DDA1F267-EF3D-4AD2-8777-FB5B042DCB16', 'RegisteredUser', 'REGISTEREDUSER')");

            migrationBuilder.Sql("INSERT INTO AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount) VALUES ( N'359b5d89-2934-472f-8973-ff95db750c82', N'admin@admin.com', N'ADMIN@ADMIN.COM', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAENbV0D0QmUsowu7m3QSn5I/JihE8ojO8KSshfgou4lggtYI/HnbWky0BrAXhLn2kRA==', N'HTM6FT32MKXRUA7L2CAW7GOCF6A62GRD', N'73483de7-bad6-45ec-a69f-ad90e29cc051', NULL, 0, 0, NULL, 0, 0 )");
            migrationBuilder.Sql("INSERT INTO AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount) VALUES ( N'5844ce13-5d45-4937-a982-060af6806d86', N'user@user.com', N'USER@USER.COM', N'user@user.com', N'USER@USER.COM', 0, N'AQAAAAIAAYagAAAAEEQ56seyOwOTQJzf5f7RQ8hwdb11Qke1kx6edZXDWRKnzPRIgySxyEtxnVKazA0xsA==', N'CAOYD47FYMG32XSTFBBIUH3C52QLEXQ7', N'f6448b06-b4f0-4171-afed-0c3a9e7588fc', NULL, 0, 0, NULL, 0, 0 )");

            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('359b5d89-2934-472f-8973-ff95db750c82', 'B6E3AF0D-9AAC-43C5-8A2A-9A28A0110799')");
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('5844ce13-5d45-4937-a982-060af6806d86', 'DDA1F267-EF3D-4AD2-8777-FB5B042DCB16')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
