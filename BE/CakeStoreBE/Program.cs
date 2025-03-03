
using CakeStoreBE.Application.Services;
using CakeStoreBE.Infrastructure.Database;
using CakeStoreBE.Utils.JWTProcess;
using CakeStoreBE.Utils.JWTProcess.TokenGenerators;
using Microsoft.EntityFrameworkCore;

namespace CakeStoreBE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BakeStoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //AddScope cho Services
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
            builder.Services.AddScoped<ICakeProductServices, CakeProductServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();


            //Token
            builder.Services.AddScoped<TokenGenerator>();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<JwtServices>(builder.Configuration.GetSection("JwtServices"));
            


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CakeStore API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
