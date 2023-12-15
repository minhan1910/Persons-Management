using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {        
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country>? Countrties { get; set; }
        public DbSet<Person>? Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed to Countries
            string countriesJson = File.ReadAllText("countries.json");
            var countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);
            countries?.ForEach(country => modelBuilder.Entity<Country>().HasData(country));

            // Seed to Persons
            string personsJson = File.ReadAllText("persons.json");
            var persons = JsonSerializer.Deserialize<List<Person>>(personsJson);
            persons?.ForEach(person => modelBuilder.Entity<Person>().HasData(person));
        }
    }
}
