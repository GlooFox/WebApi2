using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi2.Dao;
using WebApi2.Middleware;
using WebApi2.Models;

namespace WebApi2
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
			services.AddMvc();

			services.AddTransient<IStorage>(provider => Storage.Instance);
			services.AddTransient<IDao<Post>, PostsDao>();
			services.AddTransient<IDao<Comment>, CommentsDao>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
			app.UseMvc();
		}
	}
}
