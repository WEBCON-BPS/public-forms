using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WEBCON.FormsGenerator.API;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html;
using WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField;
using WEBCON.FormsGenerator.BusinessLogic.Application.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Application.Service;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Service;
using WEBCON.FormsGenerator.Data;
using WEBCON.FormsGenerator.Presentation.Configuration;
using WEBCON.FormsGenerator.Presentation.Configuration.Model;
using static WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField.HtmlFormFieldBuilder;

namespace WEBCON.FormsGenerator.Presentation
{
    [ExcludeFromCodeCoverage]
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
            services.AddControllersWithViews();
            services.AddTransient<IDataEncoding, DataEncodingService>(serviceProvider =>
            {
                var conf = Configuration.GetSection("APISettings")?.Get<ApiSettings>();
                return new DataEncodingService()
                {
                    AppKey = conf?.AppKey
                };
            });
            services.AddScoped<IFormUnitOfWork, FormGeneratorUnitOfWork>();
            services.AddScoped<IFormCommandService, FormCommandService>();
            services.AddScoped<IFormQueryService, FormQueryService>();
            services.AddTransient<IFormBuilder, HtmlFormBuilder>();
            AddBpsClients(services);
            services.AddTransient<IFormContentService, FormContentService>();
            services.AddScoped<IBpsQueryService, BpsQueryService>();
            services.AddScoped<IBpsStartElementService, BpsStartElementService>();
            services.AddTransient<IBpsClientCheckConnection, BpsApiClientCheckConnection>();
            services.AddTransient<IBpsCheckConnectionService, BpsCheckConnectionService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IFormContentRefreshService, FormContentRefreshService>();
            services.AddSingleton<IFormFieldFactory, FormFieldFactory>(serviceLocator =>
            {
                return new FormFieldFactory(new BpsFormFieldBuilder(), new HtmlFormValueBuilder());
            });
            services.ConfigureWritable<ApiSettings>(Configuration.GetSection("APISettings"));
            services.ConfigureWritable<CaptchaSettings>(Configuration.GetSection("CaptchaSettings"));
            services.ConfigureWritable<Logging>(Configuration.GetSection("Logging"));
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login/";
                options.LogoutPath = $"/Account/Logout/";
                options.AccessDeniedPath = $"/account/login/accessDenied";
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                                    new CultureInfo("en"),
                                    new CultureInfo("pl") };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

        }

        private void AddBpsClients(IServiceCollection services)
        {
            var conf = Configuration.GetSection("APISettings")?.Get<Configuration.Model.ApiSettings>();
            var credentials = new BusinessLogic.Application.DTO.Credentials(conf?.ClientId, conf?.ClientSecret);

            services.AddScoped<IBpsClientQueryService, BpsApiQueryClient>(serviceProvider =>
                new BpsApiQueryClient(conf?.Url, conf?.DatabaseId ?? 1, new FormFieldFactory(new HtmlFormFieldBuilder(), new BpsFormFieldValueBuilder()), credentials)
            );
            services.AddScoped<IBpsClientCommandService, BpsApiCommandClient>(serviceProvider =>
                new BpsApiCommandClient(conf?.Url, conf?.DatabaseId ?? 1, credentials)
            );
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
                app.UseStatusCodePagesWithReExecute("/Form/Error", "?statusCode={0}");
                app.UseExceptionHandler("/Form/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseHttpsRedirection();
            app.UseStaticFiles();         
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Forms}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
