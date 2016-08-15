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
                        try
                        {
                            webClient.DownloadFile("https:" + item.representations.full,
                                BuildPath(item.file_name, folderName, item.sha512_hash));
                        }
                        catch (Exception)
                        {

                        }
                        Thread.Sleep(1000);
                        Console.ForegroundColor = ConsoleColor.Green;
                        
                        if(item.file_name != "")
                            Console.WriteLine("Saved: " + item.file_name);
                        else                       
                            Console.WriteLine("Saved with sha512 name: " + item.sha512_hash.Substring(0, 10) + ".png");
                                                                          
                }              
            }                     
        }

        private string BuildPath(string itemName, string folderName, string sha512)
        {
            string path = @"D:\parserTest\" + @folderName + @"\";
            DirectoryInfo di = Directory.CreateDirectory(path);

            if (itemName.Length > 20)
            {
                itemName = itemName.Substring(0, 20) + ".png";
            }
            
            if (itemName.Equals(""))
                itemName = sha512.Substring(0, 10) + ".png";

            if (!itemName.EndsWith(".jpg") 
                || !itemName.EndsWith(".png") 
                || !itemName.EndsWith(".jpeg") 
                || !itemName.EndsWith(".gif") 
                || !itemName.EndsWith(".bmp"))
                itemName = itemName + ".png";
            string finalPath = path + itemName;
            return finalPath;
        }
    }
}
