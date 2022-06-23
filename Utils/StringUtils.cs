using System.Text.RegularExpressions;

namespace Short.er.Utils {
  public class StringUtils {
	public static string RemoveBadChars(string word) {
	  Regex reg = new Regex("[^a-zA-Z0-9]");
	  return reg.Replace(word, string.Empty);
	}
  }
}
