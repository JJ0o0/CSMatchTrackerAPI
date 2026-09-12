using Microsoft.EntityFrameworkCore;
using CSMatchTracker.Models;

namespace CSMatchTracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options) {

    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Partida> Partidas { get; set; }
    public DbSet<ParticipacaoPartida> ParticipacaoPartidas { get; set; }
    public DbSet<Campeonato> Campeonatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Jogador>()
            .ToTable("jogadores");

        modelBuilder.Entity<Jogador>()
            .Property(j => j.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Jogador>()
            .Property(j => j.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<Jogador>()
            .Property(j => j.Nickname)
            .HasColumnName("nickname");

        modelBuilder.Entity<Jogador>()
            .Property(j => j.Funcao)
            .HasColumnName("funcao")
            .HasConversion<string>();

        modelBuilder.Entity<Jogador>()
            .Property(j => j.DataCadastro)
            .HasColumnName("dataCadastro")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Campeonato>()
            .ToTable("campeonatos");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.DataInicio)
            .HasColumnName("dataInicio")
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.DataFim)
            .HasColumnName("dataFim")
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Campeonato>()
            .Property(c => c.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        modelBuilder.Entity<Partida>()
            .ToTable("partidas");

        modelBuilder.Entity<Partida>()
            .Property(p => p.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Partida>()
            .Property(p => p.CampeonatoId)
            .HasColumnName("campeonatoId");

        modelBuilder.Entity<Partida>()
            .Property(p => p.DataHora)
            .HasColumnName("dataHora")
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Partida>()
            .Property(p => p.Mapa)
            .HasColumnName("mapa");

        modelBuilder.Entity<Partida>()
            .Property(p => p.PlacarTR)
            .HasColumnName("placarTR");

        modelBuilder.Entity<Partida>()
            .Property(p => p.PlacarCT)
            .HasColumnName("placarCT");

        modelBuilder.Entity<Partida>()
            .Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        modelBuilder.Entity<Partida>()
            .HasOne(p => p.Campeonato)
            .WithMany(c => c.Partidas)
            .HasForeignKey(p => p.CampeonatoId)
            .IsRequired(false);

        modelBuilder.Entity<ParticipacaoPartida>()
            .ToTable("participacoespartida");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Id)
            .HasColumnName("id");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.JogadorId)
            .HasColumnName("jogadorId");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.PartidaId)
            .HasColumnName("partidaId");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Lado)
            .HasColumnName("lado")
            .HasConversion<string>();

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Kills)
            .HasColumnName("kills");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Deaths)
            .HasColumnName("deaths");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Assists)
            .HasColumnName("assists");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Headshots)
            .HasColumnName("headshots");

        modelBuilder.Entity<ParticipacaoPartida>()
            .Property(p => p.Mvp)
            .HasColumnName("mvp");

        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(p => p.Jogador)
            .WithMany()
            .HasForeignKey(p => p.JogadorId);

        modelBuilder.Entity<ParticipacaoPartida>()
            .HasOne(p => p.Partida)
            .WithMany()
            .HasForeignKey(p => p.PartidaId);

        modelBuilder.Entity<ParticipacaoPartida>()
            .HasIndex(p => new { p.JogadorId, p.PartidaId })
            .IsUnique()
            .HasDatabaseName("uq_jogador_partida");
    }
}