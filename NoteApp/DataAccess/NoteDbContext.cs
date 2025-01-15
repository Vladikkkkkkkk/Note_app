using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.DataAccess;

public class NoteDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public NoteDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Note> Notes => Set<Note>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database")); 
    }
}