using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//----------------------------------------------


using System;
using System.Text;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using ShopDb;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;





namespace ShopAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
     

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AuthRepository>();
            services.AddTransient<KatalogRepository>();
            services.AddTransient<ProductRepository>();
            services.AddTransient<ImageRepository>();
            services.AddTransient<TypeProductRepository>();
            services.AddTransient<ProductItemRepository>();

            services.AddControllers();

            services.AddCors(option => option.AddPolicy("DefaultPolicy", builder => builder
                   .WithOrigins(Environment.GetEnvironmentVariable("FrontClient"))
                   // .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                  .AllowCredentials()
                  ));



         //   var authConfig = Configuration.GetSection("Auth");
            var connectString = Configuration["ConnectStringLocal"];
            //  var serverVersion = new MySqlServerVersion(new Version(8, 0,21));

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<MyShopContext>(
                options => options
                    .UseMySql(connectString, new MySqlServerVersion(new Version(8, 0, 11)))

            );


            //-----------------------------new db context end
          //  services.Configure<AuthLib.AuthOptions>(authConfig);

         //   var sp = services.BuildServiceProvider();

            // Resolve the services from the service provider
            // var fooService = sp.GetService<IFooService>();
        //    var options = sp.GetService<IOptions<AuthLib.AuthOptions>>();

          
            // Console.WriteLine("Aut Startyp file"+_authOptions.Value.Secret);

            services.AddAuthentication(
                opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                )
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = Environment.GetEnvironmentVariable("Issuer"),

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = Environment.GetEnvironmentVariable("Audience"),
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ClientSecrets"))),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true

                        };

                        //----------------------------------

                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();
            //-------  Cors all servers------------

            app.UseCors("DefaultPolicy");

            //15.09.21
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();
            //------------20.12.20-----------
            app.UseStaticFiles();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }


    }
}
