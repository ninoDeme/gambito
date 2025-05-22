using Microsoft.EntityFrameworkCore;

namespace GambitoServer.Db;

public partial class GambitoContext : DbContext
{
  public GambitoContext()
  {
  }

  public GambitoContext(DbContextOptions<GambitoContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Defeito> Defeitos { get; set; }

  public virtual DbSet<Etapa> Etapas { get; set; }

  public virtual DbSet<Funcao> Funcaos { get; set; }

  public virtual DbSet<Funcionario> Funcionarios { get; set; }

  public virtual DbSet<LinhaProducao> LinhaProducaos { get; set; }

  public virtual DbSet<LinhaProducaoDia> LinhaProducaoDia { get; set; }

  public virtual DbSet<LinhaProducaoHora> LinhaProducaoHoras { get; set; }

  public virtual DbSet<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; }

  public virtual DbSet<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; }

  public virtual DbSet<Pedido> Pedidos { get; set; }

  public virtual DbSet<Produto> Produtos { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .UseNpgsql("Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito", o => o.MapEnum<TipoHora>("tipo_hora"))
      .UseSnakeCaseNamingConvention();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Defeito>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("defeito_pkey");

      entity.ToTable("defeito");

      entity.HasIndex(e => e.Nome, "defeito_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Nome)
              .HasMaxLength(50);
    });

    modelBuilder.Entity<Etapa>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("etapa_pkey");

      entity.ToTable("etapa");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Nome)
              .HasMaxLength(100);
    });

    modelBuilder.Entity<Funcao>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("funcao_pkey");

      entity.ToTable("funcao");

      entity.HasIndex(e => e.Nome, "funcao_nome_key").IsUnique();

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Nome)
              .HasMaxLength(100);
    });

    modelBuilder.Entity<Funcionario>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("funcionario_pkey");

      entity.ToTable("funcionario");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Encarregado);
      entity.Property(e => e.Funcao);
      entity.Property(e => e.Invativo)
              .HasDefaultValue(false);
      entity.Property(e => e.Nome)
              .HasMaxLength(100);

      entity.HasOne(d => d.EncarregadoNavigation).WithMany(p => p.InverseEncarregadoNavigation)
              .HasForeignKey(d => d.Encarregado)
              .HasConstraintName("funcionario_encarregado_fkey");

      entity.HasOne(d => d.FuncaoNavigation).WithMany(p => p.Funcionarios)
              .HasForeignKey(d => d.Funcao)
              .HasConstraintName("funcionario_funcao_fkey");
    });

    modelBuilder.Entity<LinhaProducao>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("linha_producao_pkey");

      entity.ToTable("linha_producao");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Descricao);
    });

    modelBuilder.Entity<LinhaProducaoDia>(entity =>
    {
      entity.HasKey(e => new { e.LinhaProducao, e.Data }).HasName("linha_producao_dia_pkey");

      entity.ToTable("linha_producao_dia");

      entity.Property(e => e.LinhaProducao);
      entity.Property(e => e.Data);
      entity.Property(e => e.Invativo)
              .HasDefaultValue(false);

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
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Paralizacao)
              .HasDefaultValue(false);
      entity.Property(e => e.Pedido);
      entity.Property(e => e.QtdProduzido);

      entity.HasOne(d => d.PedidoNavigation).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => d.Pedido)
              .OnDelete(DeleteBehavior.ClientNoAction)
              .HasConstraintName("linha_producao_hora_pedido_fkey");

      entity.HasOne(d => d.LinhaProducaoDia).WithMany(p => p.LinhaProducaoHoras)
              .HasForeignKey(d => new { d.LinhaProducao, d.Data })
              .HasConstraintName("linha_producao_hora_linha_producao_data_fkey");
    });

    modelBuilder.Entity<LinhaProducaoHoraDefeito>(entity =>
    {
      entity.HasKey(e => new { e.LinhaProducaoHora, e.Retrabalhado, e.Defeito }).HasName("linha_producao_hora_defeito_pkey");

      entity.ToTable("linha_producao_hora_defeito");

      entity.Property(e => e.LinhaProducaoHora);
      entity.Property(e => e.Retrabalhado);
      entity.Property(e => e.Defeito);
      entity.Property(e => e.QtdPecas);

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

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Data);
      entity.Property(e => e.Etapa);
      entity.Property(e => e.LinhaProducao);
      entity.Property(e => e.Ordem);
      entity.Property(e => e.Segundos);

      entity.HasOne(d => d.EtapaNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => d.Etapa)
              .HasConstraintName("linha_producao_hora_etapa_etapa_fkey");

      entity.HasOne(d => d.LinhaProducaoDia).WithMany(p => p.LinhaProducaoHoraEtapas)
              .HasForeignKey(d => new { d.LinhaProducao, d.Data })
              .HasConstraintName("linha_producao_hora_etapa_linha_producao_data_fkey");

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
                    j.IndexerProperty<int>("LinhaProducaoHoraEtapa");
                    j.IndexerProperty<int>("Funcionario");
                  });
    });

    modelBuilder.Entity<Pedido>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("pedido_pkey");

      entity.ToTable("pedido");

      entity.Property(e => e.Id)
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Produto);
      entity.Property(e => e.QtdPecas);

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
              .UseIdentityAlwaysColumn();
      entity.Property(e => e.Nome)
              .HasMaxLength(100);
      entity.Property(e => e.TempoPeca);
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
