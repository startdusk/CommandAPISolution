using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using EFCore.NamingConventions;
using CommandAPI.Models;

namespace CommandAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CommandContext>();
            // 添加认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["Authentication:Audience"],

                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                };
            });

            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];

            // 添加注入容器的数据库上下文对象
            // TODO: 2021/11/7 有BUG
            // 解决方法：https://github.com/efcore/EFCore.NamingConventions/issues/1#issuecomment-743245148
            services.AddDbContext<CommandContext>(option =>
                // 并设置snakecase格式的sql字段
                option.UseNpgsql(builder.ConnectionString)
                .UseSnakeCaseNamingConvention());

            // Registers services to enable the use of “Controllers” throughout
            // our application. As mentioned in the info box, in previous
            // versions of .NET Core Framework, you would have specified
            // services.AddMVC. Don’t worry; we cover what the Model–View–
            // Controller (MVC) pattern is below.
            services.AddControllers().AddNewtonsoftJson(setupAction =>
            {
                // 设置返回的json key格式为小写字符加下划线，需要安装Microsoft.AspNetCore.Mvc.NewtonsoftJson
                setupAction.SerializerSettings.ContractResolver =
                new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            // 添加 扫描转换Dto的映射光系(扫描项目目录中继承了AutoMapper.Profile的类)
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // 添加要注入容器的服务，这里使用Scoped，是指每来一个request就创造一个新的对象
            // services.AddScoped<ICommandAPIRepo, MockCommandAPIRepo>();
            services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 你在哪里
            app.UseRouting();
            // 你是谁
            app.UseAuthentication();
            // 你可以干啥(有啥权限)
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });

                // We “MapControllers” to our endpoints. This means we make use
                // of the Controller services (registered in the ConfigureServices
                // method) as endpoints in the Request Pipeline.
                endpoints.MapControllers();
            });
        }
    }
}
