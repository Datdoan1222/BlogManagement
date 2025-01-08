using BlogManagement.Contracts;
using BlogManagement.Data;
using BlogManagement.Mappings;
using BlogManagement.Repository;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddControllersWithViews();
            services.AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                    s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            services.AddDbContext<BlogManagementDBContext>
                (opts => opts.UseSqlServer(Configuration.GetConnectionString("BlogManageDB")));

            //register lifetime scoped 
            services.AddScoped<IUploadImagePostCreateEditPost, UploadImage>();
            services.AddScoped<IImageUploadUser, ImageUploadUsers>();
            services.AddScoped<IUserRepository, UserRepository>();

            //register lifetime Trainsient 
            services.AddTransient<ITokenService, TokenService>();

            //unitofwork
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            //register automapper
            services.AddAutoMapper(typeof(Maps));

            //configuration services to add two JwtBearerDefaults.AuthenticationScheme
            //and CookieAuthenticationDefaults.AuthenticationScheme
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "MultiScheme";
            })
            .AddPolicyScheme("MultiScheme", "JWT or Cookie", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    var bearerAuth = context.Request.Headers["Authorization"].FirstOrDefault()?.StartsWith("Bearer ") ?? false;

                    // You could also check for the actual path here if that's your requirement:
                    if (bearerAuth)
                        return JwtBearerDefaults.AuthenticationScheme;
                    else
                        return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.LoginPath = Configuration["Google:LoginPath"];
                        options.LoginPath = Configuration["Facebook:LoginPath"]; // Must be lowercase
                    })
                    .AddGoogle(options =>
                    {
                        options.ClientId = Configuration["Google:ClientId"];
                        options.ClientSecret = Configuration["Google:ClientSecret"];
                        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    })
                    .AddFacebook(options =>
                    {
                        options.AppId = Configuration["Facebook:AppId"];
                        options.AppSecret = Configuration["Facebook:AppSecret"];
                        options.Events.OnCreatingTicket = (context) =>
                        {
                            var picture = $"https://graph.facebook.com/{context.Principal.FindFirstValue(ClaimTypes.NameIdentifier)}/picture?type=large";
                            context.Identity.AddClaim(new Claim("Picture", picture));
                            return Task.CompletedTask;
                        };
                    })

            .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new
                                    SymmetricSecurityKey
                                    (Encoding.UTF8.GetBytes
                                    (Configuration["Jwt:Key"]))
                        };
                    });

            services.AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
                               .RequireAuthenticatedUser()
                               .Build();
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //specify that session state, authentication, and routing that will be used.
            app.UseSession();

            //config Jwt middleware
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
