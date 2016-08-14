using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Booru34
{
    class PictureSaver
    {
        WebClient webClient = new WebClient();
        public void SaveToFolder(List<Search> searchItems, string folderName, int upvotes)
        {                
            foreach (var item in searchItems)
            {
                if (item.upvotes > upvotes)
                {
                    if (item.file_name != "")
                    {
                        webClient.DownloadFile("http:" + item.representations.full, BuildPath(item.file_name, folderName));

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved: " + item.file_name);                      
                    }                 
                }              
            }                     
        }

        private string BuildPath(string itemName, string folderName)
        {
            string path = @"parserTest\" + @folderName + @"\";
            DirectoryInfo di = Directory.CreateDirectory(path);
            return path + itemName;
        }
    }
}
