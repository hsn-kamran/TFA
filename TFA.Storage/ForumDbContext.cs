﻿using Microsoft.EntityFrameworkCore;

namespace TFA.Storage;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        
    }

    public DbSet<Forum> Forums { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Topic> Topics { get; set; }
}
