using System.ComponentModel.DataAnnotations;

namespace Short.er.Models {
  public class ConfirmAccountModel {
	
  [Required]
	[Display(Name = "Code")]
	public string Code { get; set; }
  }
}
