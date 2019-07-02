using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedSoft.Test.Api.Database;
using RedSoft.Test.Api.Implementations;
using RedSoft.Test.Api.Interfaces;
using RedSoft.Test.Api.Middleware;
using RedSoft.Test.Api.Models;
using RedSoft.Test.Api.ViewModel;

namespace RedSoft.Test.Api
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
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				// Configuring ModelValidation Exception Handling
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = context =>
					{
						var modelErrors = context.ModelState.Values
							.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
							.ToArray();

						return new BadRequestObjectResult(
							new ErrorResponse(String.Join("; ", modelErrors))
						);
					};
				});

			services.AddDbContext<SqlContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));

			services.AddScoped<IAccountService, AccountService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Ensuring database created
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<SqlContext>();
				context.Database.EnsureCreated();
			}

			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
