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
using DataAnalysisSystem.DataAnalysisCommands;
using DataAnalysisSystem.AkkaNet;
using Akka.Actor;
using Akka.Configuration;

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
            services.AddSingleton<IEmailProviderConfigurationProfile>
                (Configuration.GetSection("EmailProviderConfiguration").Get<EmailProviderConfigurationProfile>());

            services.AddTransient<ICodeGenerator, CodeGeneratorUtilityForMongoDB>();

            services.AddTransient<IMimeTypeGuesser, MimeTypeGuesser>();
            services.AddTransient<IFileHelper, FileHelper>();

            services.AddTransient<CustomSerializer, CustomSerializer>();
            services.AddTransient<IRegexComparatorChainFacade, RegexComparatorChainFacade>();

            // Data Access Layer
            services.AddSingleton<RepositoryContext, RepositoryContext>();
            services.AddSingleton<DbContextAbstract, MongoDbContext>();
            services.AddSingleton<MongoDbContext, MongoDbContext>();

            // Repository Layer
            services.AddTransient<IUserRepository, MongoUserRepository>();
            services.AddTransient<IDatasetRepository, MongoDatasetRepository>();
            services.AddTransient<IAnalysisRepository, MongoAnalysisRepository>();

            // Command and Akka.Net Layer
            services.AddTransient<IDataAnalysisHub, DataAnalysisHub>();
            services.AddTransient<IDataAnalysisService, DataAnalysisService>();
            services.AddTransient<IActorModelHub, ActorModelHub>();

            // Actor-Model environment
            var config = ConfigurationFactory.ParseString(@"
                             akka.remote.dot-netty.tcp {
                             transport-class = ""Akka.Remote.Transport.DotNetty.DotNettyTransport, Akka.Remote""
                             transport-protocol = tcp
                             port = 8091
                             hostname = ""127.0.0.1""
                         }");

            services.AddSingleton(_ => ActorSystem.Create("local-akka-server", config));
            
            // Max form size
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
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

            lifetime.ApplicationStarted.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>();
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait();
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
