using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AngularBlog.Model;

namespace AngularBlog.Handlers
{
    public class PostHandler
    {
        public static void SaveFilesToDisk(Post post, string folder)
        {
            foreach (Match match in Regex.Matches(post.Content, "(src|href)=\"(data:([^\"]+))\"(>.*?</a>)?"))
            {
                string extension = string.Empty;
                string filename = string.Empty;

                // Image
                if (match.Groups[1].Value == "src")
                {
                    extension = Regex.Match(match.Value, "data:([^/]+)/([a-z]+);base64").Groups[2].Value;
                }
                // Other file type
                else
                {
                    // Entire filename
                    extension = Regex.Match(match.Value, "data:([^/]+)/([a-z0-9+-.]+);base64.*\">(.*)</a>").Groups[3].Value;
                }

                byte[] bytes = ConvertToBytes(match.Groups[2].Value);
                string path = SaveFileToDisk(bytes, extension, folder);

                string value = string.Format("src=\"{0}\" alt=\"\" ", path);

                if (match.Groups[1].Value == "href")
                    value = string.Format("href=\"{0}\"", path);

                Match m = Regex.Match(match.Value, "(src|href)=\"(data:([^\"]+))\"");
                post.Content = post.Content.Replace(m.Value, value);
            }
        }

        private static byte[] ConvertToBytes(string base64)
        {
            int index = base64.IndexOf("base64,", StringComparison.Ordinal) + 7;
            return Convert.FromBase64String(base64.Substring(index));
        }

        private static string SaveFileToDisk(byte[] bytes, string extension, string folder)
        {
            folder = Path.Combine(folder, "Files");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string fileName = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(extension))
                extension = ".bin";
            else
                extension = "." + extension.Trim('.');

            fileName += extension;
            string filePath = folder + "/" + fileName;

            File.WriteAllBytes(filePath, bytes);
            
            return "/Data/Posts/Files/"+ fileName;
        }
    }
}
