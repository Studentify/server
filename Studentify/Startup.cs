using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text;
using Studentify.Data;
using Microsoft.EntityFrameworkCore;
using Studentify.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Studentify.Data.Repositories;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Models;
using Studentify.Data.Repositories.ControllerRepositories.Interfaces;
using Studentify.Data.Repositories.ControllerRepositories.Implementations;

namespace Studentify
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentifyDbContext>(options => 
                options
                    // .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"), x => x.UseNetTopologySuite()));
            
            services.AddControllers();
            //services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.));

            services
                .AddScoped(typeof(ISelectRepository<>), typeof(SelectRepositoryBase<>))
                .AddScoped(typeof(IInsertRepository<>), typeof(InsertRepositoryBase<>))
                .AddScoped(typeof(IDeleteRepository<>), typeof(DeleteRepositoryBase<>))
                .AddScoped(typeof(IUpdateRepository<>), typeof(UpdateRepositoryBase<>))

                .AddScoped<IStudentifyAccountsRepository, StudentifyAccountsRepository>()

                .AddScoped<IThreadsRepository, ThreadsRepository>()
                .AddScoped<IMessagesRepository, MessagesRepository>()

                .AddScoped<IStudentifyEventsRepository, StudentifyEventsRepository>()
                .AddScoped<IInfosRepository, InfosRepository>()
                .AddScoped<ITradeOffersRepository, TradeOffersRepository>()
                .AddScoped<IMeetingsRepository, MeetingsRepository>();
                


            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); //todo make more specific
                    });
            });
            
            services.AddIdentity<StudentifyUser, IdentityRole>()
                .AddEntityFrameworkStores<StudentifyDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Studentify", Version = "v1" });
                // To Enable authorization using Swagger (JWT)    
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Studentify v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
