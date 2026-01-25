using System.ComponentModel.DataAnnotations;

namespace FlightTicketsWeb.Web.ViewModels
{
	public class BookingModel
	{
		[RegularExpression(@"^[\d]{10}$", ErrorMessage = "Номер паспорта должен содержать только цифры (10 символов)")]
		public string PassportNum { get; set; }
		[RegularExpression(@"^[А-Яа-яЁёA-Za-z\s\-]+$", ErrorMessage ="Фамилия должна содержать только буквы (русские и английские)")]
		[StringLength(30, ErrorMessage = "Максимум 30 символов")]
		public string Surname { get; set; }
		[RegularExpression(@"^[А-Яа-яЁёA-Za-z\s\-]+$", ErrorMessage = "Имя должно содержать только буквы (русские и английские)")]
		[StringLength(30, ErrorMessage = "Максимум 30 символов")]
		public string FirstName { get; set; }
		[RegularExpression(@"^[А-Яа-яЁёA-Za-z\s\-]+$", ErrorMessage = "Отчество должно содержать только буквы (русские и английские)")]
		[StringLength(30, ErrorMessage = "Максимум 30 символов")]
		public string LastName { get; set; }
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }
		public string Sex { get; set; }
		[RegularExpression(@"^(\+7|7|8)?[\s\-]?\(?[0-9]{3}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$", ErrorMessage ="Введите корректный номер телефона (+7 (123) 456-78-90))")]
		public string? Phone { get; set; }
		[EmailAddress(ErrorMessage ="Введите корректный email адрес")]
		public string? Email { get; set; }
		public int FlightId { get; set; }
		public int? HotelId { get; set; }
	}
}
