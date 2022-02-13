using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BookStore.Authorization.Roles;
using BookStore.Authorization.Users;
using BookStore.MultiTenancy;

namespace BookStore.EntityFrameworkCore
{
    public class BookStoreDbContext : AbpZeroDbContext<Tenant, Role, User, BookStoreDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }
    }
}
