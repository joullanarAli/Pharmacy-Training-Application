﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PharmacyDB.Models;

#nullable disable

namespace PharmacyDB.Migrations
{
    [DbContext(typeof(PharmacyDbContext))]
    [Migration("20230719200748_choiceTextAttribute")]
    partial class choiceTextAttribute
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PharmacyDB.Models.ActiveIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("ActiveIngredient", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AnswerText")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ChoiceId")
                        .HasColumnType("int");

                    b.Property<int>("UserQuestionIsd")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChoiceId");

                    b.HasIndex("UserQuestionIsd");

                    b.ToTable("Answer", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Brand", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ChoiceText")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Choice");

                    b.Property<float>("Mark")
                        .HasColumnType("real");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Choice", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Drug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ArabicName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("SideEffects")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Drug", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.DrugActiveIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ActiveIngredientId")
                        .HasColumnType("int");

                    b.Property<int>("DrugId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActiveIngredientId");

                    b.HasIndex("DrugId");

                    b.ToTable("DrugActiveIngredient", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.DrugForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<float>("Dose")
                        .HasColumnType("real");

                    b.Property<int>("DrugId")
                        .HasColumnType("int");

                    b.Property<int>("FormId")
                        .HasColumnType("int");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("DrugId");

                    b.HasIndex("FormId");

                    b.ToTable("DrugForm", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExamDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Exam", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.ExamQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ExamQuestion", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Form", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("NoAnswerMark")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("WrongAnswerMark")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Question", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.UserExam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.HasIndex("UserId");

                    b.ToTable("UserExam", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.UserQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ExamQuestionId")
                        .HasColumnType("int");

                    b.Property<float>("Mark")
                        .HasColumnType("real");

                    b.Property<int>("UserExamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExamQuestionId");

                    b.HasIndex("UserExamId");

                    b.ToTable("UserQuestion", (string)null);
                });

            modelBuilder.Entity("PharmacyDB.Models.Answer", b =>
                {
                    b.HasOne("PharmacyDB.Models.Choice", "Choice")
                        .WithMany("Answers")
                        .HasForeignKey("ChoiceId")
                        .HasConstraintName("FK_Answers_Choices");

                    b.HasOne("PharmacyDB.Models.UserQuestion", "UserQuestionIsdNavigation")
                        .WithMany("Answers")
                        .HasForeignKey("UserQuestionIsd")
                        .IsRequired()
                        .HasConstraintName("FK_Answers_UserQuestions");

                    b.Navigation("Choice");

                    b.Navigation("UserQuestionIsdNavigation");
                });

            modelBuilder.Entity("PharmacyDB.Models.Choice", b =>
                {
                    b.HasOne("PharmacyDB.Models.Question", "Question")
                        .WithMany("Choices")
                        .HasForeignKey("QuestionId")
                        .IsRequired()
                        .HasConstraintName("FK_Choices_Questions");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("PharmacyDB.Models.Drug", b =>
                {
                    b.HasOne("PharmacyDB.Models.Brand", "Brand")
                        .WithMany("Drugs")
                        .HasForeignKey("BrandId")
                        .IsRequired()
                        .HasConstraintName("FK_Drugs_Brands");

                    b.HasOne("PharmacyDB.Models.Category", "Category")
                        .WithMany("Drugs")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Drugs_Categories");

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PharmacyDB.Models.DrugActiveIngredient", b =>
                {
                    b.HasOne("PharmacyDB.Models.ActiveIngredient", "ActiveIngredient")
                        .WithMany("DrugActiveIngredients")
                        .HasForeignKey("ActiveIngredientId")
                        .IsRequired()
                        .HasConstraintName("FK_CategoriesEffectiveMaterials_EffectiveMaterials");

                    b.HasOne("PharmacyDB.Models.Drug", "Drug")
                        .WithMany("DrugActiveIngredients")
                        .HasForeignKey("DrugId")
                        .IsRequired()
                        .HasConstraintName("FK_CategoriesEffectiveMaterials_Drugs");

                    b.Navigation("ActiveIngredient");

                    b.Navigation("Drug");
                });

            modelBuilder.Entity("PharmacyDB.Models.DrugForm", b =>
                {
                    b.HasOne("PharmacyDB.Models.Drug", "Drug")
                        .WithMany("DrugForms")
                        .HasForeignKey("DrugId")
                        .IsRequired()
                        .HasConstraintName("FK_DrugsForm_Drugs");

                    b.HasOne("PharmacyDB.Models.Form", "Form")
                        .WithMany("DrugForms")
                        .HasForeignKey("FormId")
                        .IsRequired()
                        .HasConstraintName("FK_DrugsForm_Forms");

                    b.Navigation("Drug");

                    b.Navigation("Form");
                });

            modelBuilder.Entity("PharmacyDB.Models.ExamQuestion", b =>
                {
                    b.HasOne("PharmacyDB.Models.Exam", "Exam")
                        .WithMany("ExamQuestions")
                        .HasForeignKey("ExamId")
                        .IsRequired()
                        .HasConstraintName("FK_ExamQuestions_Exams");

                    b.HasOne("PharmacyDB.Models.Question", "Question")
                        .WithMany("ExamQuestions")
                        .HasForeignKey("QuestionId")
                        .IsRequired()
                        .HasConstraintName("FK_ExamQuestions_Questions");

                    b.Navigation("Exam");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("PharmacyDB.Models.Image", b =>
                {
                    b.HasOne("PharmacyDB.Models.Question", "Question")
                        .WithMany("Images")
                        .HasForeignKey("QuestionId")
                        .IsRequired()
                        .HasConstraintName("FK_Images_Questions");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("PharmacyDB.Models.Question", b =>
                {
                    b.HasOne("PharmacyDB.Models.Course", "Course")
                        .WithMany("Questions")
                        .HasForeignKey("CourseId")
                        .IsRequired()
                        .HasConstraintName("FK_Questions_Courses");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("PharmacyDB.Models.UserExam", b =>
                {
                    b.HasOne("PharmacyDB.Models.Exam", "Exam")
                        .WithMany("UserExams")
                        .HasForeignKey("ExamId")
                        .IsRequired()
                        .HasConstraintName("FK_UserExams_Exams");

                    b.HasOne("PharmacyDB.Models.User", "User")
                        .WithMany("UserExams")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserExams_Users");

                    b.Navigation("Exam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PharmacyDB.Models.UserQuestion", b =>
                {
                    b.HasOne("PharmacyDB.Models.ExamQuestion", "ExamQuestion")
                        .WithMany("UserQuestions")
                        .HasForeignKey("ExamQuestionId")
                        .IsRequired()
                        .HasConstraintName("FK_UserQuestions_ExamQuestions");

                    b.HasOne("PharmacyDB.Models.UserExam", "UserExam")
                        .WithMany("UserQuestions")
                        .HasForeignKey("UserExamId")
                        .IsRequired()
                        .HasConstraintName("FK_UserQuestions_UserExams");

                    b.Navigation("ExamQuestion");

                    b.Navigation("UserExam");
                });

            modelBuilder.Entity("PharmacyDB.Models.ActiveIngredient", b =>
                {
                    b.Navigation("DrugActiveIngredients");
                });

            modelBuilder.Entity("PharmacyDB.Models.Brand", b =>
                {
                    b.Navigation("Drugs");
                });

            modelBuilder.Entity("PharmacyDB.Models.Category", b =>
                {
                    b.Navigation("Drugs");
                });

            modelBuilder.Entity("PharmacyDB.Models.Choice", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("PharmacyDB.Models.Course", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("PharmacyDB.Models.Drug", b =>
                {
                    b.Navigation("DrugActiveIngredients");

                    b.Navigation("DrugForms");
                });

            modelBuilder.Entity("PharmacyDB.Models.Exam", b =>
                {
                    b.Navigation("ExamQuestions");

                    b.Navigation("UserExams");
                });

            modelBuilder.Entity("PharmacyDB.Models.ExamQuestion", b =>
                {
                    b.Navigation("UserQuestions");
                });

            modelBuilder.Entity("PharmacyDB.Models.Form", b =>
                {
                    b.Navigation("DrugForms");
                });

            modelBuilder.Entity("PharmacyDB.Models.Question", b =>
                {
                    b.Navigation("Choices");

                    b.Navigation("ExamQuestions");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("PharmacyDB.Models.User", b =>
                {
                    b.Navigation("UserExams");
                });

            modelBuilder.Entity("PharmacyDB.Models.UserExam", b =>
                {
                    b.Navigation("UserQuestions");
                });

            modelBuilder.Entity("PharmacyDB.Models.UserQuestion", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
