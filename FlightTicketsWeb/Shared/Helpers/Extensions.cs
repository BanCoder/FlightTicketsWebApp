using FlightTicketsWeb.Web.ViewModels.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Shared.Helpers
{
	public static class Extensions
	{
		public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
		{
			var connectionString = configuration["Project:ConnectionSettings:sqlConnection"];
			serviceCollection.AddDbContext<FlightTicketsContext>(x =>
			{
				x.UseSqlServer(connectionString);
			});
			return serviceCollection;
		}
	}
}
