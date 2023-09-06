using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PharmacyDB.Models
{
    public partial class PharmacyDbContext : IdentityDbContext
    {
        public PharmacyDbContext()
        {
        }

        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActiveIngredient> ActiveIngredients { get; set; } = null!;
    //    public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Choice> Choices { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Drug> Drugs { get; set; } = null!;
        public virtual DbSet<DrugActiveIngredient> DrugActiveIngredients { get; set; } = null!;
        public virtual DbSet<DrugForm> DrugForms { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual DbSet<Form> Forms { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<UserExam> UserExam{ get; set; } = null!;
        public virtual DbSet<UserQuestion> UserQuestions { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=ASUS\\SQLEXPRESS;database= Pharmacy;Trusted_Connection=True;  Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActiveIngredient>(entity =>
            {
                entity.ToTable("ActiveIngredient");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            /*modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerText)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.HasOne(d => d.Choice)
                   .WithMany(p => p.Answers)
                   .HasForeignKey(d => d.ChoiceId)
                   .HasConstraintName("FK_Answers_Choices");

                *//*entity.HasOne(d => d.Choice)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.ChoiceId)
                    .HasConstraintName("FK_Answers_Choices");*//*
                entity.HasOne(d => d.UserQuestion)
                 .WithMany(p => p.Answers)
                 .HasForeignKey(d => d.UserQuestionId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

                *//* entity.HasOne(d => d.UserQuestionIsdNavigation)
                     .WithMany(p => p.Answers)
                     .HasForeignKey(d => d.UserQuestionsId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Answers_UserQuestions");*//*
            });
*/
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Choice>(entity =>
            {
                entity.ToTable("Choice");
                entity.Property(e => e.Score)
                    .HasColumnName("Score");
                entity.Property(e => e.ChoiceText)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Choice");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Choices)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Choices_Questions");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Drug>(entity =>
            {
                entity.ToTable("Drug");

                entity.Property(e => e.ArabicName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SideEffects).HasColumnType("text");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Drugs)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drugs_Brands");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Drugs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drugs_Categories");
            });

            modelBuilder.Entity<DrugActiveIngredient>(entity =>
            {
                entity.ToTable("DrugActiveIngredient");

                entity.HasOne(d => d.ActiveIngredient)
                    .WithMany(p => p.DrugActiveIngredients)
                    .HasForeignKey(d => d.ActiveIngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoriesEffectiveMaterials_EffectiveMaterials");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugActiveIngredients)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoriesEffectiveMaterials_Drugs");
            });

            modelBuilder.Entity<DrugForm>(entity =>
            {
                entity.ToTable("DrugForm");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugForms)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugsForm_Drugs");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.DrugForms)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugsForm_Forms");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

             //   entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.ToTable("ExamQuestion");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestions_Exams");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestions_Questions");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("Form");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");
                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Path)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Images_Questions");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionText)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_Courses");
            });
            modelBuilder.Entity<UserQuestion>(entity =>
            {
                entity.ToTable("UserQuestion");
/*
                entity.HasMany(d => d.Answers) // Configure the one-to-many relationship
            .WithOne(p => p.UserQuestion)
            .HasForeignKey(d => d.UserQuestionId)
            .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UserExam)
                    .WithMany(p => p.UserQuestions)
                    .HasForeignKey(d => d.UserExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                // ...*/
            });
            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
