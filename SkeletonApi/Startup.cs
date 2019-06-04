using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SkeletonApi.Data;
using SkeletonApi.Service;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SkeletonApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var s = ComputeSha256Hash();

            Environment.SetEnvironmentVariable("Secret", s);

            services.AddMvc();
            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("DataUserMemory"));
            services.AddAutoMapper(typeof(Startup));

            
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        }

        private string ComputeSha256Hash()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 128; i++)
            {
                sb.Append("a");
            }
            return sb.ToString();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
