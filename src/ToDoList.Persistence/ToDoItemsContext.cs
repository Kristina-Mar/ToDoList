﻿namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    private readonly string connectionString;
    public ToDoItemsContext()
    {
        connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? "Data Source=../../data/localdb.db";
        this.Database.Migrate();
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}
