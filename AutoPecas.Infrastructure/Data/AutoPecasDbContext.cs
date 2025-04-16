using AutoPecas.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoPecas.Infrastructure.Data;

public class AutoPecasDbContext : DbContext
{
    public AutoPecasDbContext(DbContextOptions<AutoPecasDbContext> options) : base(options)
    {
    }

    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Vendedor> Vendedores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<VendaItem> VendaItens { get; set; }
    public DbSet<EstoqueNotificacao> EstoqueNotificacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração do relacionamento muitos-para-muitos entre Veiculo e Produto
        modelBuilder.Entity<Veiculo>()
            .HasMany(v => v.ProdutosCompativeis)
            .WithMany(p => p.VeiculosCompativeis)
            .UsingEntity<Dictionary<string, object>>(
                "VeiculoProduto",
                j => j.HasOne<Produto>().WithMany().HasForeignKey("ProdutoId"),
                j => j.HasOne<Veiculo>().WithMany().HasForeignKey("VeiculoId"),
                j => j.ToTable("VeiculoProduto"));

        // Configuração de índices
        modelBuilder.Entity<Produto>()
            .HasIndex(p => p.Nome)
            .IsUnique(false);

        modelBuilder.Entity<Fornecedor>()
            .HasIndex(f => f.Nome)
            .IsUnique(false);

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Documento)
            .IsUnique();

        // Configurações específicas para PostgreSQL
        modelBuilder.Entity<Produto>()
            .Property(p => p.Preco)
            .HasColumnType("numeric(18,2)");

        // Configuração de valores padrão
        modelBuilder.Entity<Produto>()
            .Property(p => p.QuantidadeEstoque)
            .HasDefaultValue(0);

        modelBuilder.Entity<Produto>()
            .Property(p => p.QuantidadeMinima)
            .HasDefaultValue(5); // Valor padrão para quantidade mínima

        modelBuilder.Entity<Venda>()
            .Property(v => v.Data)
            .HasDefaultValueSql("now()"); // PostgreSQL usa 'now()' em vez de 'CURRENT_TIMESTAMP'

        modelBuilder.Entity<EstoqueNotificacao>()
            .Property(e => e.DataNotificacao)
            .HasDefaultValueSql("now()");

        // Configuração para campos de texto
        modelBuilder.Entity<Produto>()
            .Property(p => p.Imagem)
            .HasMaxLength(500);

        // Configuração de deleção em cascata
        modelBuilder.Entity<Venda>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Vendas)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Venda>()
            .HasOne(v => v.Vendedor)
            .WithMany(v => v.Vendas)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração para notificações de estoque
        modelBuilder.Entity<EstoqueNotificacao>()
            .HasOne(e => e.Produto)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}