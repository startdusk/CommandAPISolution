using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Npgsql;

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
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];
            // 添加注入容器的数据库上下文对象
            services.AddDbContext<CommandContext>(option =>
                option.UseNpgsql(builder.ConnectionString));

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

            app.UseRouting();
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
