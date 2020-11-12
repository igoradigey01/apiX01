using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//----------------------------------------------
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Text;
using AuthApi.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using AuthLib;
using Microsoft.AspNetCore.HttpOverrides;




namespace AuthApi
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
            services.AddTransient<AuthApi.Model.KatalogRepository>();
            services.AddControllers();
           services.AddCors(
               opshions=>opshions.AddDefaultPolicy(builder=>{
               builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
           })

           );
        
        var authConfig=Configuration.GetSection("Auth");
        var connectString=Configuration["ConnectStringLocal"];

       // Console.WriteLine("startup file --connect string"+connectString);
        //Console.WriteLine("connectString -val--"+connectString);
        
           services.AddDbContext<AppDbContext>(options=>options.UseMySql(connectString,mysqlOptions =>
        {
            mysqlOptions
                .ServerVersion(new Version(8, 0, 0), ServerType.MySql);
        }));
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

          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
        
        });
        }

        public static string GetConnetionStringDB(){

          // задаетсля в двух местах - по другому не работает
          // created one 07.11.2020
       throw new Exception("NOt Implimetn Exception"); 
           // return 
            //  "server=localhost;port=3306;UserId=root;Password=;database=AuthUserDB;"; // -LOCAL HOST CONNECT
           //  "server=db;port=3306;UserId=root;Password=;database=AuthUserDB;"; // DOCKER-COMPOSE CONNECT
          //  Configuration.GetConnectionString("BloggingDatabase");
              /*

             string server=configuration["ConnectionString:server"];
                string userId=configuration["ConnectionString:UserId"];
                string password=configuration["ConnectionString:Password"];
                  string database=configuration["ConnectionString:database"];
                  //------------------------------------------------
            StringBuilder connectionString = new StringBuilder("server=");
                       
            connectionString.Append(server);
            connectionString.Append(";UserId=");
            connectionString.Append(userId);
            connectionString.Append(";Password=");
            connectionString.Append(password);
            connectionString.Append(";database=");
            connectionString.Append(database);
            Console.WriteLine("connectionString:"+connectionString.ToString());
            
                //optionsBuilder.UseMySql();
              //optionsBuilder.UseMySql(connectionString.ToString());
              */
        }
    }
}
