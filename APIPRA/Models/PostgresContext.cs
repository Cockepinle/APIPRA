using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace APIPRA.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutprojectsection> Aboutprojectsections { get; set; }

    public virtual DbSet<Forumpost> Forumposts { get; set; }

    public virtual DbSet<Forumreply> Forumreplies { get; set; }

    public virtual DbSet<Housingarticle> Housingarticles { get; set; }

    public virtual DbSet<Housingarticletype> Housingarticletypes { get; set; }

    public virtual DbSet<Housingdocument> Housingdocuments { get; set; }

    public virtual DbSet<Jobvacancy> Jobvacancies { get; set; }

    public virtual DbSet<Languagetest> Languagetests { get; set; }
    public  DbSet<TestQuestion> TestQuestions { get; set; }

    public DbSet<UserAnswer> UserAnswers { get; set; }

    public virtual DbSet<Legalarticle> Legalarticles { get; set; }

    public virtual DbSet<Medicalarticle> Medicalarticles { get; set; }

    public virtual DbSet<Medicalclinic> Medicalclinics { get; set; }

    public virtual DbSet<Migrationcenter> Migrationcenters { get; set; }

    public virtual DbSet<Socialhelparticle> Socialhelparticles { get; set; }

    public virtual DbSet<Testimage> Testimages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userquestion> Userquestions { get; set; }

    public virtual DbSet<Usertestresult> Usertestresults { get; set; }

    public virtual DbSet<Workarticle> Workarticles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.icnzyalprjcuvbodkdwj;Password=Lepilina12345678!;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Aboutprojectsection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aboutprojectsections_pkey");

            entity.ToTable("aboutprojectsections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Section)
                .HasMaxLength(255)
                .HasColumnName("section");
        });

        modelBuilder.Entity<Forumpost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forumposts_pkey");

            // Явно указываем название таблицы
            entity.ToTable("forumposts");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn(); // Для SERIAL в PostgreSQL

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnName("content");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            entity.HasOne(d => d.User)
                .WithMany(p => p.Forumposts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("forumposts_user_id_fkey");
        });

        modelBuilder.Entity<UserAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("useranswers_pkey");

            entity.ToTable("useranswers");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn(); // Для SERIAL в PostgreSQL

            entity.Property(e => e.UserTestResultId)
                .HasColumnName("user_test_result_id");

            entity.Property(e => e.QuestionId)
                .HasColumnName("question_id");

            entity.Property(e => e.UserAnswerText)
                .IsRequired()
                .HasColumnName("user_answer");

            entity.Property(e => e.IsCorrect)
                .IsRequired()
                .HasColumnName("is_correct");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            entity.HasOne(d => d.UserTestResult)
                .WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.UserTestResultId)
                .HasConstraintName("useranswers_user_test_result_id_fkey");

            entity.HasOne(d => d.Question)
                .WithMany()
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("useranswers_question_id_fkey");
        });



        modelBuilder.Entity<Forumreply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forumreplies_pkey");

            entity.ToTable("forumreplies");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();

            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnName("content");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            entity.Property(e => e.PostId)
                .HasColumnName("post_id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Housingarticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("housingarticles_pkey");
            entity.ToTable("housingarticles");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnName("content");

            entity.Property(e => e.TypeId)
                .HasColumnName("type_id");

            // Добавляем связь
            entity.HasOne(a => a.ArticleType)
                .WithMany()
                .HasForeignKey(a => a.TypeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Housingarticletype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("housingarticletypes_pkey");
            entity.ToTable("housingarticletypes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Housingdocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("housingdocuments_pkey");

            entity.ToTable("housingdocuments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Jobvacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobvacancies_pkey");

            entity.ToTable("jobvacancies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(255)
                .HasColumnName("contact_info");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EmployerName)
                .HasMaxLength(255)
                .HasColumnName("employer_name");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Languagetest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("languagetests_pkey");
            entity.ToTable("languagetests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type")
                .IsRequired();

            // Конфигурация для вопросов
            entity.HasMany(t => t.TestQuestions)
                .WithOne(q => q.Test)
                .HasForeignKey(q => q.TestId)
                .HasConstraintName("fk_testquestion_languagetest") // Добавляем имя constraint
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация для изображений (у вас было дублирование)
            entity.HasMany(t => t.Testimages)
                .WithOne(i => i.Test) // Добавляем навигационное свойство
                .HasForeignKey(i => i.TestId)
                .HasConstraintName("fk_testimage_languagetest")
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Legalarticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("legalarticles_pkey");

            entity.ToTable("legalarticles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Medicalarticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicalarticles_pkey");

            entity.ToTable("medicalarticles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Topic)
                .HasMaxLength(255)
                .HasColumnName("topic");
        });

        modelBuilder.Entity<Medicalclinic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicalclinics_pkey");

            entity.ToTable("medicalclinics");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.IsFree)
                .HasDefaultValue(false)
                .HasColumnName("is_free");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Migrationcenter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("migrationcenters_pkey");

            entity.ToTable("migrationcenters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Region)
                .HasMaxLength(255)
                .HasColumnName("region");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(255)
                .HasColumnName("working_hours");
        });

        modelBuilder.Entity<Socialhelparticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("socialhelparticles_pkey");

            entity.ToTable("socialhelparticles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Topic)
                .HasMaxLength(255)
                .HasColumnName("topic");
        });

        modelBuilder.Entity<Testimage>(entity =>
        {
            entity.ToTable("testimages");

            // Первичный ключ
            entity.HasKey(e => e.Id)
                .HasName("testimages_pkey");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // Внешний ключ на таблицу tests (предположительно Languagetest)
            entity.Property(e => e.TestId)
                .HasColumnName("test_id");

            entity.HasOne(e => e.Test)
                .WithMany(t => t.Testimages)
                .HasForeignKey(e => e.TestId)
                .HasConstraintName("fk_testimage_test")
                .OnDelete(DeleteBehavior.Cascade);

            // Поле image_url
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(255)
                .IsRequired();

            // Поле metadata (jsonb)
            entity.Property(e => e.Metadata)
                .HasColumnName("metadata")
                .HasColumnType("jsonb")
                .IsRequired();

            // Поле created_at
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.ToTable("testquestions");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");

            entity.Property(e => e.Question)
                .HasColumnName("question")
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.Answer)
                .HasColumnName("answer")
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.QuestionType)
                .HasColumnName("question_type")
                .IsRequired()
                .HasMaxLength(255);

            // Добавляем конфигурацию для Options (храним как JSON в базе)
            entity.Property(e => e.Options)
            .HasColumnName("options")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // сериализация в JSON строку
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)) // десериализация из JSON строки
            .HasColumnType("jsonb");

            // Настройка связей
            entity.HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestions)
                .HasForeignKey(tq => tq.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(tq => tq.UserAnswers)
                .WithOne(ua => ua.Question)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
        });

       

        modelBuilder.Entity<Userquestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userquestions_pkey");

            entity.ToTable("userquestions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

        
        });

        modelBuilder.Entity<Usertestresult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usertestresults_pkey");

            entity.ToTable("usertestresults");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            // Добавляем отношения
            entity.HasOne(d => d.Test)
                .WithMany()
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("fk_usertestresult_test");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_usertestresult_user");
        });

        modelBuilder.Entity<Workarticle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workarticles_pkey");

            entity.ToTable("workarticles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
