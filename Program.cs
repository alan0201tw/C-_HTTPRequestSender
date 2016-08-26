using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;

namespace SendingRequest
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static HttpRequestMessage req = new HttpRequestMessage();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // Thank you , Stack Overflow

        static HttpRequestMessage Clone_req()
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            clone.Content = req.Content;
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        static void init_message()
        {
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
            Console.WriteLine("/// This is a program that can send http request to specific url. ///");
            Console.WriteLine("/// Codes are written in C# , Please first input target url and   ///");
            Console.WriteLine("/// the program will show further usage guide.                    ///");
            Console.WriteLine("///                                              Made by Fatshih  ///");
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
        }

        static void show_user_guide()
        {
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
            Console.WriteLine("/// Input a string to mark your next command , each command       ///");
            Console.WriteLine("/// correspond to a specific string. Table as below :             ///");
            Console.WriteLine("/// 'URL'                 - Change target URL                     ///");   
            Console.WriteLine("/// 'guide'               - Show user guide                       ///");
            Console.WriteLine("/// 'change method'       - Change Method                         ///");
            Console.WriteLine("/// 'add header'          - Add Header                            ///");
            Console.WriteLine("/// 'remove header'       - Remove Header with key                ///");
            Console.WriteLine("/// 'info'                - Show current request information      ///");
            Console.WriteLine("/// 'send request'        - Send Request                          ///");
            Console.WriteLine("/// 'cls' - Clear Console    'ctrl + c' or 'exit' - exit program  ///");
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
        }

        static void show_request_info()
        {
            Console.WriteLine(req);
            //Console.WriteLine(req.Properties);
        }

        static void change_target_url()
        {
            try
            {
                Console.WriteLine("\nInput URL : ");
                string url = Console.ReadLine();
                req.RequestUri = new Uri(url);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        static void add_header()
        {
            try
            {
                Console.WriteLine("\nAdding header");
                Console.WriteLine("Input header - key");
                string key = Console.ReadLine();
                Console.WriteLine("\nInput value correspond to {0} ", key);
                string value = Console.ReadLine();

                req.Headers.Add(key, value);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void remove_header()
        {
            try
            {
                Console.WriteLine("\nRemoving header");
                Console.WriteLine("Input header - key");
                string key = Console.ReadLine();

                req.Headers.Remove(key);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void change_method()
        {
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
            Console.WriteLine("/// Input the method you want to change to , available method are ///");
            Console.WriteLine("/// GET , POST , PUT , DELETE                                     ///");
            Console.WriteLine("/// Valid Inputs are : get , post , put ,delete                   ///");
            Console.WriteLine("/////////////////////////////////////////////////////////////////////");
            Console.WriteLine("Input the method you want to change to :");

            string method = Console.ReadLine();

            switch(method)
            {
                case "get":
                    req.Method = HttpMethod.Get;
                    break;
                case "post":
                    req.Method = HttpMethod.Post;
                    break;
                case "put":
                    req.Method = HttpMethod.Put;
                    break;
                case "delete":
                    req.Method = HttpMethod.Delete;
                    break;
                default:
                    Console.WriteLine("In-Valid Input , please try again.");
                    break;
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        static void Main(string[] args)
        {
            init_message();
            change_target_url();
            show_user_guide();

            while (true)
            {
                Console.WriteLine("Input command : ");
                string command = Console.ReadLine();

                switch(command)
                {
                    case "URL":
                        change_target_url();
                        break;
                    case "guide":
                        show_user_guide();
                        break;
                    case "change method":
                        change_method();
                        break;
                    case "add header":
                        add_header();
                        break;
                    case "remove header":
                        remove_header();
                        break;
                    case "info":
                        show_request_info();
                        break;
                    case "send request":
                        send_request();
                        break;
                    case "cls":
                        Console.Clear();
                        show_user_guide();
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("No correspond command found , please try again.");
                        break;
                }

                Console.Write('\n');
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        static async void send_request()
        {
            try
            {
                var tmp_req = Clone_req();
                //Change content (body) , wont fit in Get
                //req.Content = new StringContent("{\"lalala\":\"aa\"}", Encoding.UTF8);
                //use async method
                //var result = await client.SendAsync(req);
                //use sync method
                var result = client.SendAsync(tmp_req).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(result);
                //Console.WriteLine("-------------------------");
                Console.WriteLine(resultContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when sending {0} request to {1},\n\terror message : {2}",req.Method , req.RequestUri , ex.Message);
            }
            finally
            {
                Console.WriteLine("-------------------------");
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
