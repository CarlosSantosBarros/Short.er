using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Short.er.Models {
  public class ShortenedUrl {

	public int Id { get; set; }
	public string User { get; set; }
	public string? Hash { get; set; }
	public DateTime CreatedDateTime { get; set; } = DateTime.Now;
	public int NumberOfRquests { get; set; } = 0;
	[Required]
	[BindProperty]
	public string? Url { get; set; }
  }
}
