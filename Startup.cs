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
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using AuthLib;
using ShopDbLibNew;
using Microsoft.AspNetCore.HttpOverrides;





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
            services.AddTransient<UploadImageRepository>();
            services.AddTransient<TypeProductRepository>();

            services.AddControllers();

             services.AddCors(
               opshions=>opshions.AddDefaultPolicy(builder=>{
               builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
           })

           );
        
        var authConfig=Configuration.GetSection("Auth");
      var connectString=Configuration["ConnectStringLocal"];
           //  var connectString=Configuration["ConnectStringDocker"];
      
        
             services.AddDbContext<MyShopContext>(options=>options.UseMySql(connectString,mysqlOptions =>
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
            //------------20.12.20-----------
            app.UseStaticFiles();

          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
        
        });
        }

     
    }
}
