using Microsoft.EntityFrameworkCore;
using Beedle.Entities;

namespace Beedle.DbContexts{
class ItemDb : DbContext{
  public ItemDb(DbContextOptions<ItemDb> options)
    : base(options) {}
    public DbSet<Item> Items => Set<Item>();
}

class UserDb : DbContext{
  public UserDb(DbContextOptions<UserDb> options)
    : base(options) {}
    public DbSet<User> Users => Set<User>();
}
}