using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using NodaTime;

namespace GambitoServer.Db;

using GUser = User;
using GRole = IdentityRole<Guid>;
using GUserClaim = IdentityUserClaim<Guid>;
using GUserRole = IdentityUserRole<Guid>;
using GUserLogin = IdentityUserLogin<Guid>;
using GRoleClaim = IdentityRoleClaim<Guid>;
using GUserToken = IdentityUserToken<Guid>;


public interface IHasOrg
{
  int Organizacao { get; set; }
}

public class GambitoContext : DbContext
{

  private readonly IdentityService _identity;

  public GambitoContext(IdentityService identity)
  {
    _identity = identity;
  }

  public GambitoContext(DbContextOptions<GambitoContext> options, IdentityService identity)
      : base(options)
  {
    _identity = identity;
  }

  // public bool FiltraOrg(IHasOrg entity) => entity.Organizacao == _identity.GetOrg();

  public virtual DbSet<Defeito> Defeitos { get; set; }

  public virtual DbSet<Etapa> Etapas { get; set; }

  public virtual DbSet<Funcao> Funcaos { get; set; }

  public virtual DbSet<Funcionario> Funcionarios { get; set; }

  public virtual DbSet<LinhaProducao> LinhaProducaos { get; set; }

  public virtual DbSet<LinhaProducaoDia> LinhaProducaoDia { get; set; }

  public virtual DbSet<LinhaProducaoHora> LinhaProducaoHoras { get; set; }

  public virtual DbSet<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; }

  public virtual DbSet<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; }

  public virtual DbSet<Organizacao> Organizacaos { get; set; }

  public virtual DbSet<ProdutoConfig> Pedidos { get; set; }

  public virtual DbSet<Produto> Produtos { get; set; }

  public virtual DbSet<ProdutoConfig> ProdutoConfigs { get; set; }

  public virtual DbSet<GUser> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .UseNpgsql(
        "Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito",
        o => o.MapEnum<TipoHora>("tipo_hora").UseNodaTime())
      .UseSnakeCaseNamingConvention();
  }

  private void CreateIdentity(ModelBuilder builder)
  {
    builder.Entity<GUser>(b =>
    {
      // Primary key
      b.HasKey(e => e.Id);

      // Indexes for "normalized" username and email, to allow efficient lookups
      b.HasIndex(u => u.NormalizedUserName).IsUnique();
      b.HasIndex(u => u.NormalizedEmail);

      b.ToTable("user", "auth");

      // A concurrency token for use with the optimistic concurrency checking
      b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

      // Limit the size of columns to use efficient database types
      b.Property(u => u.UserName).HasMaxLength(256);
      b.Property(u => u.NormalizedUserName).HasMaxLength(256);
      b.Property(u => u.Email).HasMaxLength(256);
      b.Property(u => u.NormalizedEmail).HasMaxLength(256);

      // The relationships between User and other entity types
      // Note that these relationships are configured with no navigation properties

      // Each User can have many UserClaims
      b.HasMany<GUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

      // Each User can have many UserLogins
      b.HasMany<GUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

      // Each User can have many UserTokens
      b.HasMany<GUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

      // Each User can have many entries in the UserRole join table
      b.HasMany<GUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    });

    builder.Entity<GUserClaim>(b =>
    {
      // Primary key
      b.HasKey(uc => uc.Id);

      b.ToTable("user_claim", "auth");
    });

    builder.Entity<GUserLogin>(b =>
    {
      // Composite primary key consisting of the LoginProvider and the key to use
      // with that provider
      b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

      // Limit the size of the composite key columns due to common DB restrictions
      b.Property(l => l.LoginProvider).HasMaxLength(128);
      b.Property(l => l.ProviderKey).HasMaxLength(128);

      b.ToTable("user_login", "auth");
    });

    builder.Entity<GUserToken>(b =>
    {
      // Composite primary key consisting of the UserId, LoginProvider and Name
      b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

      // Limit the size of the composite key columns due to common DB restrictions
      b.Property(t => t.LoginProvider).HasMaxLength(256);
      b.Property(t => t.Name).HasMaxLength(256);

      b.ToTable("user_token", "auth");
    });

    builder.Entity<GRole>(b =>
    {
      // Primary key
      b.HasKey(r => r.Id);

      // Index for "normalized" role name to allow efficient lookups
      b.HasIndex(r => r.NormalizedName).IsUnique();

      b.ToTable("role", "auth");

      // A concurrency token for use with the optimistic concurrency checking
      b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

      // Limit the size of columns to use efficient database types
      b.Property(u => u.Name).HasMaxLength(256);
      b.Property(u => u.NormalizedName).HasMaxLength(256);

      // The relationships between Role and other entity types
      // Note that these relationships are configured with no navigation properties

      // Each Role can have many entries in the UserRole join table
      b.HasMany<GUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

      // Each Role can have many associated RoleClaims
      b.HasMany<GRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
    });

    builder.Entity<GRoleClaim>(b =>
    {
      // Primary key
      b.HasKey(rc => rc.Id);

      b.ToTable("role_claim", "auth");
    });

    builder.Entity<GUserRole>(b =>
    {
      // Primary key
      b.HasKey(r => new { r.UserId, r.RoleId });

      b.ToTable("user_role", "auth");
    });
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    CreateIdentity(modelBuilder);
    modelBuilder.Entity<Defeito>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("defeito");

      entity.HasIndex(e => e.Nome, "defeito_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(50)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Defeitos)
              .HasForeignKey(d => d.Organizacao);

      entity.HasQueryFilter((l) => l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Etapa>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("etapa");

      entity.HasIndex(e => new { e.Organizacao, e.Nome }).IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Etapas)
              .HasForeignKey(d => d.Organizacao);

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Funcao>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("funcao");

      entity.HasIndex(e => new { e.Organizacao, e.Nome }).IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Funcaos)
              .HasForeignKey(d => d.Organizacao);

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Funcionario>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("funcionario");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Encarregado).HasColumnName("encarregado");
      entity.Property(e => e.Funcao).HasColumnName("funcao");
      entity.Property(e => e.Invativo)
              .HasDefaultValue(false)
              .HasColumnName("invativo");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.EncarregadoNavigation).WithMany(p => p.InverseEncarregadoNavigation)
              .HasForeignKey(d => d.Encarregado);

      entity.HasOne(d => d.FuncaoNavigation).WithMany(p => p.Funcionarios)
              .HasForeignKey(d => d.Funcao);

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Funcionarios)
              .HasForeignKey(d => d.Organizacao);

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<LinhaProducao>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("linha_producao");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Descricao).HasColumnName("descricao");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.LinhaProducaos)
              .HasForeignKey(d => d.Organizacao);

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<LinhaProducaoDia>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.HasIndex(e => new {e.LinhaProducao, e.Data}).IsUnique();

      entity.ToTable("linha_producao_dia");

      entity.Property(e => e.LinhaProducao).UseIdentityAlwaysColumn().HasColumnName("linha_producao");

      entity.Property(e => e.LinhaProducao).HasColumnName("linha_producao");
      entity.Property(e => e.Data).HasColumnName("data");
      entity.Property(e => e.Invativo)
              .HasDefaultValue(false)
              .HasColumnName("invativo");

      entity.HasOne(d => d.LinhaProducaoNavigation).WithMany(p => p.LinhaProducaoDia)
              .HasForeignKey(d => d.LinhaProducao)
              .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.LinhaProducaoNavigation.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<LinhaProducaoHora>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("linha_producao_hora");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Hora)
              .HasColumnType("time without time zone")
              .HasColumnName("hora");
      entity.Property(e => e.HoraFim)
              .HasColumnType("time without time zone")
              .HasColumnName("hora_fim");
      entity.Property(e => e.HoraIni)
              .HasColumnType("time without time zone")
              .HasColumnName("hora_ini");
      entity.Property(e => e.LinhaProducaoDia).HasColumnName("linha_producao_dia");
      entity.Property(e => e.Paralizacao)
              .HasDefaultValue(false)
              .HasColumnName("paralizacao");
      entity.Property(e => e.ProdutoConfig).HasColumnName("produto_config");
      entity.Property(e => e.QtdProduzido).HasColumnName("qtd_produzido");

      entity.HasOne(d => d.ProdutoConfigNavigation).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => d.ProdutoConfig)
              .OnDelete(DeleteBehavior.NoAction);

      entity.HasOne(d => d.LinhaProducaoDiaNavigation).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => d.LinhaProducaoDia);
    });

    modelBuilder.Entity<LinhaProducaoHoraDefeito>(entity =>
    {
      entity.HasKey(e => new { e.LinhaProducaoHora, e.Retrabalhado, e.Defeito });

      entity.ToTable("linha_producao_hora_defeito");

      entity.Property(e => e.LinhaProducaoHora).HasColumnName("linha_producao_hora");
      entity.Property(e => e.Retrabalhado).HasColumnName("retrabalhado");
      entity.Property(e => e.Defeito).HasColumnName("defeito");
      entity.Property(e => e.QtdPecas).HasColumnName("qtd_pecas");

      entity.HasOne(d => d.DefeitoNavigation).WithMany(p => p.LinhaProducaoHoraDefeitos)
              .HasForeignKey(d => d.Defeito)
              .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasOne(d => d.LinhaProducaoHoraNavigation).WithMany(p => p.LinhaProducaoHoraDefeitos)
              .HasForeignKey(d => d.LinhaProducaoHora)
              .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<ProdutoConfigEtapa>(entity =>
    {
      entity.ToTable("produto_config_etapa");

      entity.HasKey(e => new { e.ProdutoConfig, e.Etapa });

      entity.Property(e => e.Etapa).HasColumnName("etapa");
      entity.Property(e => e.ProdutoConfig).HasColumnName("produto_config");
      entity.Property(e => e.Ordem).HasColumnName("ordem");
      entity.Property(e => e.Segundos).HasColumnName("segundos");

      entity.HasOne(d => d.EtapaNavigation).WithMany(p => p.ProdutoConfigEtapas)
              .HasForeignKey(d => d.Etapa);

      entity.HasOne(d => d.ProdutoConfigNavigation).WithMany(p => p.ProdutoConfigEtapas)
              .HasForeignKey(d => d.ProdutoConfig)
              .OnDelete(DeleteBehavior.ClientCascade);
    });

    modelBuilder.Entity<LinhaProducaoHoraEtapa>(entity =>
    {
      entity.ToTable("linha_producao_hora_etapa");

      entity.HasKey(e => new { e.LinhaProducaoHora, e.Etapa });

      entity.Property(e => e.Etapa).HasColumnName("etapa");
      entity.Property(e => e.LinhaProducaoHora).HasColumnName("linha_producao_hora");

      entity.HasOne(d => d.EtapaNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => d.Etapa);

      entity.HasOne(d => d.LinhaProducaoHoraNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => d.LinhaProducaoHora)
              .OnDelete(DeleteBehavior.ClientSetNull);

      entity.HasMany(d => d.Funcionarios).WithMany(p => p.LinhaProducaoHoraEtapas)
              .UsingEntity<Dictionary<string, object>>(
                  "LinhaProducaoHoraEtapaFuncionario",
                  r => r.HasOne<Funcionario>().WithMany()
                      .HasForeignKey("Funcionario")
                      .OnDelete(DeleteBehavior.ClientCascade),
                  l => l.HasOne<LinhaProducaoHoraEtapa>().WithMany()
                      .HasForeignKey("LinhaProducaoHora", "Etapa")
                      .OnDelete(DeleteBehavior.ClientCascade),
                  j =>
                  {
                    j.HasKey("LinhaProducaoHora", "Etapa", "Funcionario");
                    j.ToTable("linha_producao_hora_etapa_funcionario");
                    j.IndexerProperty<int>("LinhaProducaoHora").HasColumnName("linha_producao_hora");
                    j.IndexerProperty<int>("Etapa").HasColumnName("etapa");
                    j.IndexerProperty<int>("Funcionario").HasColumnName("funcionario");
                  });
    });

    modelBuilder.Entity<Organizacao>(entity =>
    {
      entity.ToTable("organizacao");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
    });

    modelBuilder.Entity<ProdutoConfig>(entity =>
    {
      entity.ToTable("produto_config");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Produto).HasColumnName("produto");
      // entity.Property(e => e.QtdPecas).HasColumnName("qtd_pecas");

      entity.HasOne(d => d.ProdutoNavigation).WithMany(p => p.Pedidos)
              .HasForeignKey(d => d.Produto)
              .OnDelete(DeleteBehavior.ClientSetNull);
    });

    modelBuilder.Entity<Produto>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.ToTable("produto");

      entity.HasIndex(e => e.Nome).IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");
      entity.Property(e => e.TempoPeca).HasColumnName("tempo_peca");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Produtos)
              .HasForeignKey(d => d.Organizacao);
    });

    modelBuilder.Entity<GUser>(entity =>
    {
      entity.HasMany(d => d.Organizacaos).WithMany(p => p.Usuarios)
              .UsingEntity<Dictionary<string, object>>(
                  "UserOrganizacao",
                  r => r.HasOne<Organizacao>().WithMany()
                      .HasForeignKey("Organizacao")
                      .OnDelete(DeleteBehavior.ClientSetNull),
                  l => l.HasOne<GUser>().WithMany()
                      .HasForeignKey("Usuario"),
                  j =>
                  {
                    j.HasKey("Usuario", "Organizacao");
                    j.ToTable("user_organizacao");
                    j.IndexerProperty<Guid>("Usuario").HasColumnName("usuario");
                    j.IndexerProperty<int>("Organizacao").HasColumnName("organizacao");
                  });
    });

    Seed(modelBuilder);

    base.OnModelCreating(modelBuilder);
  }


  // public override int SaveChanges()
  // {
  //   var modifiedEntities = ChangeTracker.Entries()
  //     .Where(e => e.State == EntityState.Modified);
  //
  //   foreach (var entity in modifiedEntities)
  //   {
  //     if (entity.Property("UpdatedAt") is not null)
  //     {
  //       entity.Property("UpdatedAt").CurrentValue = new ZonedDateTime(Instant.FromDateTimeUtc(DateTime.UtcNow), DateTimeZone.Utc);
  //     }
  //   }
  //
  //   return base.SaveChanges();
  // }

  public static void Seed(ModelBuilder modelBuilder)
  {
    // --- IDs ---
    var org1Id = 1;
    var org2Id = 2;

    var adminUserId = Guid.Parse("A0E2E2E7-2B7F-4D41-9C2A-6B7A1E18B9D1");
    var operatorUserId = Guid.Parse("B1F3F3F8-3C8E-5E52-AD3B-7C8B2F29C0E2");

    var adminRoleId = Guid.Parse("C2A4A4A9-4D9D-6F63-BE4C-8D9C3A30D1F3");
    var operatorRoleId = Guid.Parse("D3B5B5BA-5EAE-7A74-CF5D-9EAD4B41E2A4");

    var supervisorFuncId = 1;
    var operatorFuncId = 2;
    var managerFuncId = 3;

    var funcSupervisorId = 1;
    var funcOperator1Id = 2;
    var funcOperator2Id = 3;

    var productAId = 1;
    var productBId = 2;

    var prodConfigA1Id = 1;
    var prodConfigB1Id = 2;

    var etapaCorteId = 1;
    var etapaMontagemId = 2;
    var etapaQualidadeId = 4;

    var defeitoRiscoId = 1;
    var defeitoAmassadoId = 2;
    var defeitoPinturaId = 3;

    var linhaProd1Id = 1;
    var linhaProd2Id = 2;

    // LinhaProducaoDia PKs are same as LinhaProducao Ids
    var linhaProdDia1Id = 1;
    var linhaProdDia2Id = 3;

    var linhaProdHora1Id = 1;
    var linhaProdHora2Id = 2;
    var linhaProdHora3Id = 3;


    // --- Organizacao ---
    modelBuilder.Entity<Organizacao>().HasData(
        new Organizacao { Id = org1Id, Nome = "Org de desenvolvimento" },
        new Organizacao { Id = org2Id, Nome = "Org de testes" }
    );

    // --- Identity: GRole ---
    modelBuilder.Entity<GRole>().HasData(
        new GRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = null },
        new GRole { Id = operatorRoleId, Name = "Operator", NormalizedName = "OPERATOR", ConcurrencyStamp = null }
    );

    // --- Identity: GUser ---
    // For PasswordHash, use a proper hasher in a real scenario:
    var hasher = new PasswordHasher<GUser>();

    var admin_user = new GUser
    {
      Id = adminUserId,
      UserName = "admin@fabrica.com",
      NormalizedUserName = "ADMIN@FABRICA.COM",
      Email = "admin@fabrica.com",
      NormalizedEmail = "ADMIN@FABRICA.COM",
      EmailConfirmed = true,
      PasswordHash = "AQAAAAIAAYagAAAAEFUG/PMFzzxt549qLdgva0XLTbAIfMYCFXLSDDvZ/BIygz06h3ngihtvKAa/ryEvWg==",
      SecurityStamp = null,
      ConcurrencyStamp = null,
      LockoutEnabled = false,
      AccessFailedCount = 0,
    };

    var op_user = new GUser
    {
      Id = operatorUserId,
      UserName = "operator@fabrica.com",
      NormalizedUserName = "OPERATOR@FABRICA.COM",
      Email = "operator@fabrica.com",
      NormalizedEmail = "OPERATOR@FABRICA.COM",
      EmailConfirmed = true,
      PasswordHash = "AQAAAAIAAYagAAAAEIv3lOaj9hoHyu14WS9meY5CZoFq+ZKnMgJKM9zV/28dXFOLgo+EP+ZSuQym1FRBLg==",
      SecurityStamp = null,
      ConcurrencyStamp = null,
      LockoutEnabled = false,
      AccessFailedCount = 0,
    };

    // admin_user.PasswordHash = hasher.HashPassword(admin_user, "AdminPass123!");
    // op_user.PasswordHash = hasher.HashPassword(op_user, "OperatorPass123!");
    //
    // Console.WriteLine(admin_user.PasswordHash);
    // Console.WriteLine(op_user.PasswordHash);

    modelBuilder.Entity<GUser>().HasData(
      admin_user,
      op_user
    );

    // --- Identity: GUserRole ---
    modelBuilder.Entity<GUserRole>().HasData(
        new GUserRole { UserId = adminUserId, RoleId = adminRoleId },
        new GUserRole { UserId = operatorUserId, RoleId = operatorRoleId }
    );

    // --- Join Table: UserOrganizacao ---
    // This assumes GUser.Organizacaos and Organizacao.Usuarios setup the M-M
    // The table name is "user_organizacao"
    modelBuilder.Entity("UserOrganizacao").HasData(
        new { Usuario = adminUserId, Organizacao = org1Id },
        new { Usuario = adminUserId, Organizacao = org2Id }, // Admin has access to both
        new { Usuario = operatorUserId, Organizacao = org1Id }
    );

    // --- Defeito ---
    modelBuilder.Entity<Defeito>().HasData(
        new Defeito { Id = defeitoRiscoId, Nome = "Risco Profundo", Organizacao = org1Id },
        new Defeito { Id = defeitoAmassadoId, Nome = "Amassado Leve", Organizacao = org1Id },
        new Defeito { Id = defeitoPinturaId, Nome = "Falha na Pintura", Organizacao = org1Id },
        new Defeito { Id = 4, Nome = "Risco Superficial", Organizacao = org2Id }
    );

    // --- Etapa ---
    modelBuilder.Entity<Etapa>().HasData(
        new Etapa { Id = etapaCorteId, Nome = "Corte", Organizacao = org1Id },
        new Etapa { Id = etapaMontagemId, Nome = "Montagem", Organizacao = org1Id },
        new Etapa { Id = etapaQualidadeId, Nome = "Controle de Qualidade", Organizacao = org1Id },
        new Etapa { Id = 5, Nome = "Embalagem", Organizacao = org2Id }
    );

    // --- Funcao ---
    modelBuilder.Entity<Funcao>().HasData(
        new Funcao { Id = supervisorFuncId, Nome = "Supervisor de Produção", Organizacao = org1Id },
        new Funcao { Id = operatorFuncId, Nome = "Operador de Máquina", Organizacao = org1Id },
        new Funcao { Id = managerFuncId, Nome = "Gerente de Qualidade", Organizacao = org1Id },
        new Funcao { Id = 4, Nome = "Auxiliar de Produção", Organizacao = org2Id }
    );

    // --- Produto ---
    modelBuilder.Entity<Produto>().HasData(
        new Produto { Id = productAId, Nome = "Produto Alfa", Organizacao = org1Id, TempoPeca = 120 }, // 120 segundos
        new Produto { Id = productBId, Nome = "Produto Beta", Organizacao = org1Id, TempoPeca = 180 },
        new Produto { Id = 3, Nome = "Produto Gama", Organizacao = org2Id, TempoPeca = 90 }
    );

    // --- Funcionario ---
    modelBuilder.Entity<Funcionario>().HasData(
        new Funcionario { Id = funcSupervisorId, Nome = "Carlos Silva", Funcao = supervisorFuncId, Organizacao = org1Id, Invativo = false, Encarregado = null },
        new Funcionario { Id = funcOperator1Id, Nome = "Ana Pereira", Funcao = operatorFuncId, Organizacao = org1Id, Invativo = false, Encarregado = funcSupervisorId },
        new Funcionario { Id = funcOperator2Id, Nome = "João Costa", Funcao = operatorFuncId, Organizacao = org1Id, Invativo = true, Encarregado = funcSupervisorId }, // Inativo
        new Funcionario { Id = 4, Nome = "Mariana Lima", Funcao = managerFuncId, Organizacao = org1Id, Invativo = false, Encarregado = null },
        new Funcionario { Id = 5, Nome = "Pedro Alves", Funcao = 4, Organizacao = org2Id, Invativo = false, Encarregado = null }
    );

    // --- LinhaProducao ---
    modelBuilder.Entity<LinhaProducao>().HasData(
        new LinhaProducao { Id = linhaProd1Id, Descricao = "Linha de Montagem A", Organizacao = org1Id },
        new LinhaProducao { Id = linhaProd2Id, Descricao = "Linha de Pintura B", Organizacao = org1Id },
        new LinhaProducao { Id = 3, Descricao = "Linha de Testes C", Organizacao = org2Id }
    );

    // --- ProdutoConfig (Pedidos) ---
    modelBuilder.Entity<ProdutoConfig>().HasData(
        new ProdutoConfig { Id = prodConfigA1Id, Produto = productAId },
        new ProdutoConfig { Id = prodConfigB1Id, Produto = productBId },
        new ProdutoConfig { Id = 3, Produto = 3 } // Produto Gama da Org 2
    );

    // --- ProdutoConfigEtapa ---
    modelBuilder.Entity<ProdutoConfigEtapa>().HasData(
        // Produto Alfa (prodConfigA1Id)
        new ProdutoConfigEtapa { ProdutoConfig = prodConfigA1Id, Etapa = etapaMontagemId, Ordem = 2, Segundos = 60 },
        new ProdutoConfigEtapa { ProdutoConfig = prodConfigA1Id, Etapa = etapaQualidadeId, Ordem = 3, Segundos = 30 },
        // Produto Beta (prodConfigB1Id)
        new ProdutoConfigEtapa { ProdutoConfig = prodConfigB1Id, Etapa = etapaCorteId, Ordem = 1, Segundos = 40 },
        new ProdutoConfigEtapa { ProdutoConfig = prodConfigB1Id, Etapa = etapaMontagemId, Ordem = 2, Segundos = 70 },
        new ProdutoConfigEtapa { ProdutoConfig = prodConfigB1Id, Etapa = etapaQualidadeId, Ordem = 4, Segundos = 30 }
    );

    // --- LinhaProducaoDia ---
    // PK is LinhaProducao (FK to LinhaProducao.Id)
    // Ensure DateTime is handled correctly for NodaTime LocalDate (use Date part)
    var today = new LocalDate(2025, 5, 25);
    var yesterday = today.PlusDays(-1);

    modelBuilder.Entity<LinhaProducaoDia>().HasData(
        new LinhaProducaoDia { Id = linhaProdDia1Id, LinhaProducao = linhaProd1Id, Data = yesterday, Invativo = false },
        new LinhaProducaoDia { Id = 2, LinhaProducao = linhaProd1Id, Data = today, Invativo = false },
        new LinhaProducaoDia { Id = linhaProdDia2Id, LinhaProducao = linhaProd2Id, Data = today, Invativo = false }
    );

    // --- LinhaProducaoHora ---
    // LinhaProducaoDia here is the PK of LinhaProducaoDia, which is LinhaProducao.Id
    modelBuilder.Entity<LinhaProducaoHora>().HasData(
        new LinhaProducaoHora
        {
          Id = linhaProdHora1Id,
          LinhaProducaoDia = linhaProdDia1Id, // Refers to LinhaProducao 1, today
          Hora = new LocalTime(8, 0, 0),
          ProdutoConfig = prodConfigA1Id, // Produto Alfa
          QtdProduzido = 10,
          Paralizacao = false
        },
        new LinhaProducaoHora
        {
          Id = linhaProdHora2Id,
          LinhaProducaoDia = linhaProdDia1Id, // Refers to LinhaProducao 1, today
          Hora = new LocalTime(9, 0, 0), // Example of 'Hora' field
          ProdutoConfig = prodConfigA1Id, // Produto Alfa
          QtdProduzido = 12,
          Paralizacao = false
        },
        new LinhaProducaoHora
        {
          Id = linhaProdHora3Id,
          LinhaProducaoDia = linhaProdDia1Id, // Refers to LinhaProducao 1, today
          Hora = new LocalTime(10, 0, 0),
          ProdutoConfig = prodConfigA1Id,
          QtdProduzido = 0, // No production during full stop
          Paralizacao = true
        },
         new LinhaProducaoHora // For Linha Producao 2
         {
           Id = 4,
           LinhaProducaoDia = linhaProdDia2Id, // Refers to LinhaProducao 2, today
           Hora = new LocalTime(8, 0, 0),
           ProdutoConfig = prodConfigB1Id, // Produto Beta
           QtdProduzido = 8,
           Paralizacao = false
         }
    );

    // --- LinhaProducaoHoraDefeito ---
    modelBuilder.Entity<LinhaProducaoHoraDefeito>().HasData(
        new LinhaProducaoHoraDefeito { LinhaProducaoHora = linhaProdHora1Id, Retrabalhado = false, Defeito = defeitoRiscoId, QtdPecas = 2 },
        new LinhaProducaoHoraDefeito { LinhaProducaoHora = linhaProdHora1Id, Retrabalhado = true, Defeito = defeitoAmassadoId, QtdPecas = 1 },
        new LinhaProducaoHoraDefeito { LinhaProducaoHora = linhaProdHora2Id, Retrabalhado = false, Defeito = defeitoPinturaId, QtdPecas = 1 }
    );

    // --- LinhaProducaoHoraEtapa ---
    modelBuilder.Entity<LinhaProducaoHoraEtapa>().HasData(
        // For LinhaProducaoHora1 (Produto Alfa)
        new LinhaProducaoHoraEtapa { LinhaProducaoHora = linhaProdHora1Id, Etapa = etapaCorteId },
        new LinhaProducaoHoraEtapa { LinhaProducaoHora = linhaProdHora1Id, Etapa = etapaMontagemId },
        // For LinhaProducaoHora2 (Produto Alfa)
        new LinhaProducaoHoraEtapa { LinhaProducaoHora = linhaProdHora2Id, Etapa = etapaMontagemId },
        new LinhaProducaoHoraEtapa { LinhaProducaoHora = linhaProdHora2Id, Etapa = etapaQualidadeId }
    );

    // --- Join Table: LinhaProducaoHoraEtapaFuncionario ---
    // Table name: "linha_producao_hora_etapa_funcionario"
    // Keys: "LinhaProducaoHora", "Etapa", "Funcionario"
    modelBuilder.Entity("LinhaProducaoHoraEtapaFuncionario").HasData(
        // Hora 1, Etapa Corte, Funcionario Operator1
        new { LinhaProducaoHora = linhaProdHora1Id, Etapa = etapaCorteId, Funcionario = funcOperator1Id },
        // Hora 1, Etapa Montagem, Funcionario Operator2
        new { LinhaProducaoHora = linhaProdHora1Id, Etapa = etapaMontagemId, Funcionario = funcOperator2Id },
        // Hora 1, Etapa Montagem, Funcionario Operator1 (another operator on same etapa/hora)
        new { LinhaProducaoHora = linhaProdHora1Id, Etapa = etapaMontagemId, Funcionario = funcOperator1Id },

        new { LinhaProducaoHora = linhaProdHora2Id, Etapa = etapaMontagemId, Funcionario = funcOperator1Id },
        // Hora 2, Etapa Qualidade, Funcionario Supervisor (assuming supervisor can also be linked)
        new { LinhaProducaoHora = linhaProdHora2Id, Etapa = etapaQualidadeId, Funcionario = funcSupervisorId }
    );
  }
}
//
// public class SessionIterceptor(IServiceProvider serviceProvider): DbConnectionInterceptor
// {
//     // readonly IHttpContextAccessor _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()!;
//     public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
//     {
//         var conn = (NpgsqlConnection)connection;
//         var qry = conn.CreateCommand();
//         qry.CommandText = "TODO! SET SESSION AUTH";
//         qry.ExecuteNonQuery();
//         base.ConnectionOpened(connection, eventData);
//     }
// }

public class IdentityService(IServiceProvider serviceProvider)
{
  readonly IHttpContextAccessor _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()!;
  public int? GetOrg()
  {
    var id_str = _httpContextAccessor.HttpContext?.Request.Headers["org"];
    // if (id_str is null || id_str.ToString() == "") return null;
    if (id_str is null || id_str.ToString() == "") return 1;
    if (id_str is null || !int.TryParse(id_str, out int id))
    {
      throw new Exception("Organização não definida ou inválida");
    }
    return id;
  }

  // public string? GetUserId()
  // {
  //   if (httpContextAccessor.HttpContext?.User is not null)
  //   {
  //     return userManager.GetUserId(httpContextAccessor.HttpContext.User);
  //   }
  //   return null;
  // }
}
