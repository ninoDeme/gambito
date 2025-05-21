using Microsoft.EntityFrameworkCore;

namespace GambitoServer.GambitoContext;

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

    public virtual DbSet<LinhaProducaoDium> LinhaProducaoDia { get; set; }

    public virtual DbSet<LinhaProducaoHora> LinhaProducaoHoras { get; set; }

    public virtual DbSet<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; }

    public virtual DbSet<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("tipo_hora", new[] { "HORA_EXTRA", "BANCO_HORAS" });

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
        });

        modelBuilder.Entity<Etapa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("etapa_pkey");

            entity.ToTable("etapa");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Funcao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("funcao_pkey");

            entity.ToTable("funcao");

            entity.HasIndex(e => e.Nome, "funcao_nome_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
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
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
        });

        modelBuilder.Entity<LinhaProducaoDium>(entity =>
        {
            entity.HasKey(e => new { e.LinhaProducao, e.Data }).HasName("linha_producao_dia_pkey");

            entity.ToTable("linha_producao_dia");

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
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.HoraFim).HasColumnName("hora_fim");
            entity.Property(e => e.HoraIni).HasColumnName("hora_ini");
            entity.Property(e => e.LinhaProducao).HasColumnName("linha_producao");
            entity.Property(e => e.Paralizacao)
                .HasDefaultValue(false)
                .HasColumnName("paralizacao");
            entity.Property(e => e.Pedido).HasColumnName("pedido");
            entity.Property(e => e.QtdProduzido).HasColumnName("qtd_produzido");

            entity.HasOne(d => d.PedidoNavigation).WithMany(p => p.LinhaProducaoHoras)
                .HasForeignKey(d => d.Pedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("linha_producao_hora_pedido_fkey");

            entity.HasOne(d => d.LinhaProducaoDium).WithMany(p => p.LinhaProducaoHoras)
                .HasForeignKey(d => new { d.LinhaProducao, d.Data })
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

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Etapa).HasColumnName("etapa");
            entity.Property(e => e.LinhaProducao).HasColumnName("linha_producao");
            entity.Property(e => e.Ordem).HasColumnName("ordem");
            entity.Property(e => e.Segundos).HasColumnName("segundos");

            entity.HasOne(d => d.EtapaNavigation).WithMany(p => p.LinhaProducaoHoraEtapas)
                .HasForeignKey(d => d.Etapa)
                .HasConstraintName("linha_producao_hora_etapa_etapa_fkey");

            entity.HasOne(d => d.LinhaProducaoDium).WithMany(p => p.LinhaProducaoHoraEtapas)
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
                        j.IndexerProperty<int>("LinhaProducaoHoraEtapa").HasColumnName("linha_producao_hora_etapa");
                        j.IndexerProperty<int>("Funcionario").HasColumnName("funcionario");
                    });
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
            entity.Property(e => e.TempoPeca).HasColumnName("tempo_peca");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
