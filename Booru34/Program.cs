﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Booru34
{
    public class Representations
    {
        public string thumb_tiny { get; set; }
        public string thumb_small { get; set; }
        public string thumb { get; set; }
        public string small { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string tall { get; set; }
        public string full { get; set; }
    }

    public class Search
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<object> duplicate_reports { get; set; }
        public string first_seen_at { get; set; }
        public string uploader_id { get; set; }
        public string file_name { get; set; }
        public string description { get; set; }
        public string uploader { get; set; }
        public string image { get; set; }
        public int score { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }
        public int faves { get; set; }
        public int comment_count { get; set; }
        public string tags { get; set; }
        public List<string> tag_ids { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public double aspect_ratio { get; set; }
        public string original_format { get; set; }
        public string mime_type { get; set; }
        public string sha512_hash { get; set; }
        public string orig_sha512_hash { get; set; }
        public string source_url { get; set; }
        public Representations representations { get; set; }
        public bool is_rendered { get; set; }
        public bool is_optimized { get; set; }
    }

    public class RootObject
    {
        public List<Search> search { get; set; }
        public int total { get; set; }
        public List<object> interactions { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PictureSaver pictureSaver = new PictureSaver();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Write("Enter tags (example: fluttershy,rainbow+dash): ");
            string tags = Console.ReadLine();

            Console.Write("Enter starting page: ");
            int pageNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter ending page: ");
            int totalPages = int.Parse(Console.ReadLine());

            Console.Write(@"Enter folders name (example mainsix\fluttershy\ or fluttershy): ");
            string folderName = Console.ReadLine();

            Console.Write("Enter rating from (example 400): ");
            int upvotes = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Filter explicit images? (true/false): ");
            bool explicitFilter = Boolean.Parse(Console.ReadLine());
            string filter = "&filter_id=123676";
            if (explicitFilter)
                filter = "";

            //

            while (pageNumber <= totalPages)
            {
                WebRequest request =
                        WebRequest.Create("https://derpibooru.org/search.json?q=" + tags + "&page=" + pageNumber + 
                        filter + "&min_score=" + upvotes + "&key=&perpage=50");

                Console.WriteLine("Connecting...");
                
                try
                {
                    using (Stream stream = request.GetResponse().GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                Console.WriteLine("Parsing items from the page...");
                                var root = (RootObject)JObject.Parse(line).ToObject(typeof(RootObject));

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Items parsed! Current page is: " + pageNumber);

                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("Saving ponies...");

                                pictureSaver.SaveToFolder(root.search, folderName);
                  
                                pageNumber++;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error occuried while connecting to derpiboo.ru");
                    Console.WriteLine("Try again later...");                    
                }
                Thread.Sleep(300);
            }     
            Console.WriteLine("All pictures are saved");                  
            Console.Read();
        }       
    }


}
