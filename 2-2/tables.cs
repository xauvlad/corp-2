using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public DbSet<Tip> Tips { get; set; } = null!;
    public DbSet<Region> Regions { get; set; } = null!;
    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<Estate> Estates { get; set; } = null!;
    public DbSet<Criterion> Criteria { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;
    public DbSet<Agent> Agents { get; set; } = null!;
    public DbSet<Deal> Deals { get; set; } = null!;

    public AppDbContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tip>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Region>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Material>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Estate>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Criterion>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Rating>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Agent>()
            .HasKey(t => t.ID); 
        modelBuilder.Entity<Deal>()
            .HasKey(t => t.ID);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test;Username=postgres;Password=keklolkek666");
    }
}

public class Tip
{
    public long ID { get; set; }
    public string? Name { get; set; } 
}
public class Region
{
    public long ID { get; set; }
    public string? Name { get; set; } 
}

public class Material
{
    public long ID { get; set; } 
    public string Name { get; set; }
}

public class Estate
{
    public long ID { get; set; }
    public Region? Region { get; set; }
    public string Address { get; set; }
    public int Level { get; set; }
    public int RoomNum { get; set; }
    public Tip? Tip { get; set; }
    public bool Status { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }
    public Material? Material { get; set; }
    public int Area { get; set; }
    public DateOnly Data { get; set; }
}

public class Criterion 
{
    public long ID { get; set; }
    public string Name { get; set; }
}

public class Rating 
{
    public long ID { get; set; }
    public Estate? Estate { get; set; }
    public DateOnly Data { get; set; }
    public Criterion? Criterion { get; set; }
    public int Rate { get; set; }
}

public class Agent 
{
    public long ID { get; set; }
    public string Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }
    public string Phone { get; set; }
}

public class Deal 
{
    public long ID { get; set; }
    public Estate? Estate { get; set; }
    public DateOnly Data { get; set; }
    public Agent? Agent { get; set; }
    public float Cost { get; set; }
}