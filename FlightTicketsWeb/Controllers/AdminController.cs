using FlightTicketsWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicketsWeb.Controllers
{
	[Authorize(Policy = "AdminOnly")]
	public class AdminController : Controller
	{
		private readonly FlightTicketsContext _context;

		public AdminController(FlightTicketsContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return RedirectToAction("SqlQuery");
		}
		public IActionResult SqlQuery()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ExecuteSql(string sql)
		{
			if (string.IsNullOrWhiteSpace(sql))
			{
				ViewBag.Error = "Введите SQL запрос";
				return View("SqlQuery");
			}

			try
			{
				var upperSql = sql.Trim().ToUpper();
				ViewBag.Query = sql;

				if (upperSql.StartsWith("SELECT"))
				{
					using var command = _context.Database.GetDbConnection().CreateCommand();
					command.CommandText = sql;
					await _context.Database.OpenConnectionAsync();
					using var reader = await command.ExecuteReaderAsync();

					var results = new List<Dictionary<string, object>>();
					var columns = new List<string>();
					for (int i = 0; i < reader.FieldCount; i++)
					{
						columns.Add(reader.GetName(i));
					}

					while (await reader.ReadAsync())
					{
						var row = new Dictionary<string, object>();
						foreach (var column in columns)
						{
							row[column] = reader[column] is DBNull ? null : reader[column];
						}
						results.Add(row);
					}

					ViewBag.Results = results;
					ViewBag.Columns = columns;

					if (results.Count == 0)
					{
						ViewBag.Info = "Запрос выполнен успешно, но данных не найдено.";
					}
				}
				else
				{
					var result = await _context.Database.ExecuteSqlRawAsync(sql);
					ViewBag.Success = $"Успешно! Затронуто строк: {result}";
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Ошибка: {ex.Message}";
			}
			return View("SqlQuery");
		}
	}
}
