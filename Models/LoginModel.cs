using System.ComponentModel.DataAnnotations;

namespace Short.er.Models {
  public class LoginModel {
	public string ReturnUrl { get; set; }
	[Required]
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[Display(Name = "Remember me?")]
	public bool RememberMe { get; set; }
  }
}
