using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetCore.Identity.Mongo;
using DataAnalysisSystem.DataEntities;
using AutoMapper;
using DataAnalysisSystem.Services;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.DataAccessLayerAbstract;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using DataAnalysisSystem.Repository.Repository;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using DataAnalysisSystem.Services.EmailProvider;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.Services.DesignPatterns.FacadeDesignPattern;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http.Features;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.DataAnalysisMethods.AdaptersInterfaces;
using DataAnalysisSystem.DataAnalysisMethods.Adapters;

namespace DataAnalysisSystem
{
    public class Startup
    {
        private string _dbConnectionString => Configuration.GetConnectionString("MongoAtlasConnection");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityMongoDbProvider<IdentityProviderUser>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = _dbConnectionString;
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(autoMapper =>
            {
                autoMapper.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Services Layer
            services.AddTransient<IEmailProvider, EmailServiceProvider>();
            services.AddSingleton<IEmailProviderConfigurationProfile>(Configuration.GetSection("EmailProviderConfiguration").Get<EmailProviderConfigurationProfile>());

            services.AddTransient<ICodeGenerator, CodeGeneratorUtilityForMongoDB>();
            services.AddTransient<ICodeQRGenerator, CodeQRGenerator>();

            services.AddTransient<IMimeTypeGuesser, MimeTypeGuesser>();
            services.AddTransient<IFileHelper, FileHelper>();

            services.AddTransient<CustomSerializer, CustomSerializer>();
            services.AddTransient<IRegexComparatorChainFacade, RegexComparatorChainFacade>();

            // Data Access Layer
            services.AddSingleton<RepositoryContext, RepositoryContext>();
            services.AddSingleton<DbContextAbstract, MongoDbContext>();
            services.AddSingleton<MongoDbContext, MongoDbContext>();

            // Data Analysis Layer
            services.AddTransient<IKMeansClusteringAdapter, KMeansClusteringAdapter>();
            services.AddTransient<IRegressionMethodAdapter, RegressionMethodAdapter>();
            services.AddTransient<IApproximationMethodAdapter, ApproximationMethodAdapter>();
            services.AddTransient<IDerivativeMethodAdapter, DerivativeMethodAdapter>();
            services.AddTransient<IBasicStatisticsMethodAdapter, BasicStatisticsMethodAdapter>();
            services.AddTransient<IHistogramMethodAdapter, HistogramMethodAdapter>();

            // Repository Layer
            services.AddTransient<IUserRepository, MongoUserRepository>();
            services.AddTransient<IDatasetRepository, MongoDatasetRepository>();
            services.AddTransient<IAnalysisRepository, MongoAnalysisRepository>();

            services.Configure<FormOptions>(x => x.ValueCountLimit = 1000000);

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.LoginPath = "/User/UserLogin";
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/UserSystemInteraction/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var supportedCultures = new[] { CultureInfo.InvariantCulture };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserSystemInteraction}/{action=MainAction}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
