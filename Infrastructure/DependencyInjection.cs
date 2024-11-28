using Application.Interfaces.RepositoryInterfaces;
using Domain;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<RealDatabase>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IRepository<Author>, GenericRepository<Author>>();
            services.AddScoped<IRepository<Book>, GenericRepository<Book>>();
            services.AddScoped<IRepository<User>, GenericRepository<User>>();

            return services;
        }
    }
}
