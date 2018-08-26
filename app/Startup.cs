using Autofac;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidnightLizard.Schemes.Querier.Auth;
using MidnightLizard.Schemes.Querier.Configuration;
using MidnightLizard.Schemes.Querier.Container;
using MidnightLizard.Schemes.Querier.Schema;
using System;

namespace MidnightLizard.Schemes.Querier
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ElasticSearchConfig>(x =>
            {
                var esConfig = new ElasticSearchConfig();
                this.Configuration.Bind(esConfig);
                return esConfig;
            });
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            })
            .AddUserContextBuilder(httpContext =>
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    return new GraphQLUserContext { User = httpContext.User };
                }
                if (string.IsNullOrEmpty(this.Configuration.GetValue<string>(nameof(AuthConfig.NoErrors))))
                {
                    throw new UnauthorizedAccessException("Failed to authenticate the client");
                }
                return null;
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;

                    // base-address of your identityserver
                    options.Authority = this.Configuration
                        .GetValue<string>("IDENTITY_URL");

                    // name of the API resource
                    options.ApiName = "schemes-querier";
                    options.ApiSecret = this.Configuration.GetValue<string>("IDENTITY_SCHEMES_QUERIER_API_SECRET");

                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(4); // default = 10
                    options.JwtValidationClockSkew = TimeSpan.FromMinutes(1);
                });
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<GraphQLSchemaModule>();
            builder.RegisterModule<ModelDeserializationModule>();
            builder.RegisterModule<DataAccessModule>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
                //app.UseHttpsRedirection();
            }

            var corsConfig = new CorsConfig();
            this.Configuration.Bind(corsConfig);
            app.UseCors(builder => builder
                .WithOrigins(corsConfig.ALLOWED_ORIGINS.Split(new[] { "~", "," }, StringSplitOptions.RemoveEmptyEntries))
                .AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();

            app.UseMvc();

            app.UseGraphQL<ISchema>(Routes.Query);

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = Routes.Playground,
                GraphQLEndPoint = Routes.Query
            });
        }
    }
}
