using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

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

public partial class GambitoContext : DbContext
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

  public virtual DbSet<Pedido> Pedidos { get; set; }

  public virtual DbSet<Produto> Produtos { get; set; }

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
      b.HasKey(e => e.Id).HasName("user_pkey");

      // Indexes for "normalized" username and email, to allow efficient lookups
      b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
      b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

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
      b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

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
      entity.HasKey(e => e.Id).HasName("defeito_pkey");

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
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("defeito_organizacao_fkey");

      entity.HasQueryFilter((l) => l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Etapa>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("etapa_pkey");

      entity.ToTable("etapa");

      entity.HasIndex(e => new { e.Organizacao, e.Nome }, "etapa_organizacao_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Etapas)
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("etapa_organizacao_fkey");

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Funcao>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("funcao_pkey");

      entity.ToTable("funcao");

      entity.HasIndex(e => new { e.Organizacao, e.Nome }, "funcao_organizacao_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Funcaos)
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("funcao_organizacao_fkey");

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<Funcionario>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("funcionario_pkey");

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
              .HasForeignKey(d => d.Encarregado)
              .HasConstraintName("funcionario_encarregado_fkey");

      entity.HasOne(d => d.FuncaoNavigation).WithMany(p => p.Funcionarios)
              .HasForeignKey(d => d.Funcao)
              .HasConstraintName("funcionario_funcao_fkey");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Funcionarios)
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("funcionario_organizacao_fkey");

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<LinhaProducao>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("linha_producao_pkey");

      entity.ToTable("linha_producao");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Descricao).HasColumnName("descricao");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.LinhaProducaos)
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("linha_producao_organizacao_fkey");

      entity.HasQueryFilter((l) => _identity.GetOrg() == null || l.Organizacao == _identity.GetOrg());
    });

    modelBuilder.Entity<LinhaProducaoDia>(entity =>
    {
      entity.HasKey(e => e.LinhaProducao).HasName("linha_producao_dia_pkey");

      entity.ToTable("linha_producao_dia");

      entity.Property(e => e.LinhaProducao).UseIdentityAlwaysColumn().HasColumnName("linha_producao");

      entity.Property(e => e.LinhaProducao).HasColumnName("linha_producao");
      entity.Property(e => e.Data).HasColumnName("data");
      entity.Property(e => e.Invativo)
              .HasDefaultValue(false)
              .HasColumnName("invativo");

      entity.HasOne(d => d.LinhaProducaoNavigation).WithMany(p => p.LinhaProducaoDia)
              .HasForeignKey(d => d.LinhaProducao)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("linha_producao_dia_linha_producao_fkey");
    });

    modelBuilder.Entity<LinhaProducaoHora>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("linha_producao_hora_pkey");

      entity.ToTable("linha_producao_hora");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Data).HasColumnName("data");
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
      entity.Property(e => e.Pedido).HasColumnName("pedido");
      entity.Property(e => e.QtdProduzido).HasColumnName("qtd_produzido");

      entity.HasOne(d => d.PedidoNavigation).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => d.Pedido)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("linha_producao_hora_pedido_fkey");

      entity.HasOne(d => d.LinhaProducaoDiaNavigation).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => d.LinhaProducaoDia)
              .HasConstraintName("linha_producao_hora_linha_producao_data_fkey");
    });

    modelBuilder.Entity<LinhaProducaoHoraDefeito>(entity =>
    {
      entity.HasKey(e => new { e.LinhaProducaoHora, e.Retrabalhado, e.Defeito }).HasName("linha_producao_hora_defeito_pkey");

      entity.ToTable("linha_producao_hora_defeito");

      entity.Property(e => e.LinhaProducaoHora).HasColumnName("linha_producao_hora");
      entity.Property(e => e.Retrabalhado).HasColumnName("retrabalhado");
      entity.Property(e => e.Defeito).HasColumnName("defeito");
      entity.Property(e => e.QtdPecas).HasColumnName("qtd_pecas");

      entity.HasOne(d => d.DefeitoNavigation).WithMany(p => p.LinhaProducaoHoraDefeitos)
              .HasForeignKey(d => d.Defeito)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("linha_producao_hora_defeito_defeito_fkey");

      entity.HasOne(d => d.LinhaProducaoHoraNavigation).WithMany(p => p.LinhaProducaoHoraDefeitos)
              .HasForeignKey(d => d.LinhaProducaoHora)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("linha_producao_hora_defeito_linha_producao_hora_fkey");
    });

    modelBuilder.Entity<LinhaProducaoHoraEtapa>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("linha_producao_hora_etapa_pkey");

      entity.ToTable("linha_producao_hora_etapa");

      entity.HasIndex(e => new { e.LinhaProducaoHora, e.Etapa }, "linha_producao_hora_etapa_linha_producao_hora_etapa_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Etapa).HasColumnName("etapa");
      entity.Property(e => e.LinhaProducaoHora).HasColumnName("linha_producao_hora");
      entity.Property(e => e.Ordem).HasColumnName("ordem");
      entity.Property(e => e.Segundos).HasColumnName("segundos");

      entity.HasOne(d => d.EtapaNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => d.Etapa)
              .HasConstraintName("linha_producao_hora_etapa_etapa_fkey");

      entity.HasOne(d => d.LinhaProducaoHoraNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => d.LinhaProducaoHora)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("linha_producao_hora_etapa_linha_producao_hora_fkey");

      entity.HasMany(d => d.Funcionarios).WithMany(p => p.LinhaProducaoHoraEtapas)
              .UsingEntity<Dictionary<string, object>>(
                  "LinhaProducaoHoraEtapaFuncionario",
                  r => r.HasOne<Funcionario>().WithMany()
                      .HasForeignKey("Funcionario")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("linha_producao_hora_etapa_funcionario_funcionario_fkey"),
                  l => l.HasOne<LinhaProducaoHoraEtapa>().WithMany()
                      .HasForeignKey("LinhaProducaoHoraEtapa")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("linha_producao_hora_etapa_funcio_linha_producao_hora_etapa_fkey"),
                  j =>
                  {
                    j.HasKey("LinhaProducaoHoraEtapa", "Funcionario").HasName("linha_producao_hora_etapa_funcionario_pkey");
                    j.ToTable("linha_producao_hora_etapa_funcionario");
                    j.IndexerProperty<int>("LinhaProducaoHoraEtapa").HasColumnName("linha_producao_hora_etapa");
                    j.IndexerProperty<int>("Funcionario").HasColumnName("funcionario");
                  });
    });

    modelBuilder.Entity<Organizacao>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("organizacao_pkey");

      entity.ToTable("organizacao");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
    });

    modelBuilder.Entity<Pedido>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("pedido_pkey");

      entity.ToTable("pedido");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Produto).HasColumnName("produto");
      entity.Property(e => e.QtdPecas).HasColumnName("qtd_pecas");

      entity.HasOne(d => d.ProdutoNavigation).WithMany(p => p.Pedidos)
              .HasForeignKey(d => d.Produto)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("pedido_produto_fkey");
    });

    modelBuilder.Entity<Produto>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("produto_pkey");

      entity.ToTable("produto");

      entity.HasIndex(e => e.Nome, "produto_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn()
              .HasColumnName("id");
      entity.Property(e => e.Nome)
              .HasMaxLength(100)
              .HasColumnName("nome");
      entity.Property(e => e.Organizacao).HasColumnName("organizacao");
      entity.Property(e => e.TempoPeca).HasColumnName("tempo_peca");

      entity.HasOne(d => d.OrganizacaoNavigation).WithMany(p => p.Produtos)
              .HasForeignKey(d => d.Organizacao)
              .HasConstraintName("produto_organizacao_fkey");
    });

    modelBuilder.Entity<GUser>(entity =>
    {
      entity.HasMany(d => d.Organizacaos).WithMany(p => p.Usuarios)
              .UsingEntity<Dictionary<string, object>>(
                  "UserOrganizacao",
                  r => r.HasOne<Organizacao>().WithMany()
                      .HasForeignKey("Organizacao")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("user_organizacao_organizacao_fkey"),
                  l => l.HasOne<GUser>().WithMany()
                      .HasForeignKey("Usuario")
                      .HasConstraintName("user_organizacao_usuario_fkey"),
                  j =>
                  {
                    j.HasKey("Usuario", "Organizacao").HasName("user_organizacao_pkey");
                    j.ToTable("user_organizacao");
                    j.IndexerProperty<Guid>("Usuario").HasColumnName("usuario");
                    j.IndexerProperty<int>("Organizacao").HasColumnName("organizacao");
                  });
    });


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
    if (id_str is null || id_str.ToString() == "") return null;
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
