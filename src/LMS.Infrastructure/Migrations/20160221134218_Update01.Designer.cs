using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using LMS.Infrastructure;

namespace LMS.Infrastructure.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20160221134218_Update01")]
    partial class Update01
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("LMS.Core.Models.CalendarTask", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("GoalId")
                        .HasAnnotation("MaxLength", 32);

                    b.Property<int>("TimeSpentMin");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("LMS.Core.Models.Goal", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("AreaId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("Priority");

                    b.Property<int>("StateId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("LMS.Core.Models.GoalState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("LMS.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("LMS.Core.Models.UserArea", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("Color")
                        .HasAnnotation("MaxLength", 7);

                    b.Property<int>("Priority");

                    b.Property<DateTime>("Timestamp");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("LMS.Infrastructure.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("LMS.Core.Models.CalendarTask", b =>
                {
                    b.HasOne("LMS.Core.Models.Goal")
                        .WithMany()
                        .HasForeignKey("GoalId");
                });

            modelBuilder.Entity("LMS.Core.Models.Goal", b =>
                {
                    b.HasOne("LMS.Core.Models.UserArea")
                        .WithMany()
                        .HasForeignKey("AreaId");

                    b.HasOne("LMS.Core.Models.GoalState")
                        .WithMany()
                        .HasForeignKey("StateId");
                });

            modelBuilder.Entity("LMS.Core.Models.UserArea", b =>
                {
                    b.HasOne("LMS.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LMS.Infrastructure.ApplicationUser", b =>
                {
                    b.HasOne("LMS.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LMS.Infrastructure.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LMS.Infrastructure.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("LMS.Infrastructure.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
