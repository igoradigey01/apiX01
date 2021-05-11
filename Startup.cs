using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//----------------------------------------------


using System;
using System.Text;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using AuthLib;
using ShopDb;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;





namespace WebShopAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        IOptions<AuthLib.AuthOptions> _authOptions;
       
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

             services.AddCors(
               opshions=>opshions.AddDefaultPolicy(builder=>{
             builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            // builder.WithOrigins("http://localhost:4200"); //09.05.21
           })

           );
        
     
           //  var connectString=Configuration["ConnectStringDocker"];
      
        //-----------------------------new db context begin
        /*
             services.AddDbContext<MyShopContext>(options=>options.UseMySql(connectString
             ,mysqlOptions =>
        {
            mysqlOptions
                .ServerVersion(new Version(8, 0, 0), ServerType.MySql);
        }
          
        ));
           */
        //---------
        // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
               var authConfig=Configuration.GetSection("Auth");
             var connectString=Configuration["ConnectStringLocal"];
            var serverVersion = new MySqlServerVersion(new Version(8, 0,21));

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<MyShopContext>(
                options => options
                    .UseMySql(connectString,  new MySqlServerVersion(new Version(8, 0, 11)))
                //   .EnableSensitiveDataLogging() // These two calls are optional but help
                 //   .EnableDetailedErrors() 
                         // with debugging (remove for production).
            );
           
           
    //-----------------------------new db context end
            services.Configure<AuthLib.AuthOptions>(authConfig);

               var sp = services.BuildServiceProvider();

    // Resolve the services from the service provider
   // var fooService = sp.GetService<IFooService>();
    var options = sp.GetService<IOptions<AuthLib.AuthOptions>>();

    _authOptions=options;
   // Console.WriteLine("Aut Startyp file"+_authOptions.Value.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer =_authOptions.Value.Issuer,
 
                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = _authOptions.Value.Audience,
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,
 
                            // установка ключа безопасности
                            IssuerSigningKey = _authOptions.Value.GetSymmerySecuritiKey(),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true
                            
                        };

                        //----------------------------------
                       
                    });
                    //----------------0--12.08.20---------------
                    
                   


                  

                    
           

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
            app.UseCors();
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
