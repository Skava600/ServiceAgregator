// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceAggregator.Data;

#nullable disable

namespace ServiceAggregator.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceAggregator.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronym")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Doer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("DoerDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DoerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OrderCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Doers");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.DoerWorkSection", b =>
                {
                    b.Property<int>("WorkSectionId")
                        .HasColumnType("integer");

                    b.Property<int>("DoerId")
                        .HasColumnType("integer");

                    b.HasKey("WorkSectionId", "DoerId");

                    b.HasIndex("DoerId");

                    b.ToTable("DoerWorkSections");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorkSectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("WorkSectionId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountAuthorId")
                        .HasColumnType("integer");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("DoerId")
                        .HasColumnType("integer");

                    b.Property<byte>("Grade")
                        .HasColumnType("smallint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountAuthorId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DoerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.WorkSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ServiceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("WorkSections");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Customer", b =>
                {
                    b.HasOne("ServiceAggregator.Entities.Account", "Account")
                        .WithOne("Customer")
                        .HasForeignKey("ServiceAggregator.Entities.Customer", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Doer", b =>
                {
                    b.HasOne("ServiceAggregator.Entities.Account", "Account")
                        .WithOne("Service")
                        .HasForeignKey("ServiceAggregator.Entities.Doer", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.DoerWorkSection", b =>
                {
                    b.HasOne("ServiceAggregator.Entities.Doer", "Doer")
                        .WithMany("ServiceWorkSections")
                        .HasForeignKey("DoerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAggregator.Entities.WorkSection", "WorkSection")
                        .WithMany("DoerWorkSections")
                        .HasForeignKey("WorkSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doer");

                    b.Navigation("WorkSection");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Order", b =>
                {
                    b.HasOne("ServiceAggregator.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAggregator.Entities.WorkSection", "WorkSection")
                        .WithMany("Orders")
                        .HasForeignKey("WorkSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("WorkSection");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Review", b =>
                {
                    b.HasOne("ServiceAggregator.Entities.Account", "AccountAuthor")
                        .WithMany()
                        .HasForeignKey("AccountAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAggregator.Entities.Customer", "ReviewedCustomer")
                        .WithMany("Reviews")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceAggregator.Entities.Doer", "ReviewedDoer")
                        .WithMany("Reviews")
                        .HasForeignKey("DoerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountAuthor");

                    b.Navigation("ReviewedCustomer");

                    b.Navigation("ReviewedDoer");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Account", b =>
                {
                    b.Navigation("Customer")
                        .IsRequired();

                    b.Navigation("Service")
                        .IsRequired();
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Customer", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.Doer", b =>
                {
                    b.Navigation("Reviews");

                    b.Navigation("ServiceWorkSections");
                });

            modelBuilder.Entity("ServiceAggregator.Entities.WorkSection", b =>
                {
                    b.Navigation("DoerWorkSections");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
