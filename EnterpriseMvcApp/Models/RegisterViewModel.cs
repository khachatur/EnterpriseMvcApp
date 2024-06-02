using System.ComponentModel.DataAnnotations;

namespace EnterpriseMvcApp.Models
{
	public class RegisterViewModel
	{
		[Required]
		[StringLength(50)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
