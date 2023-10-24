using System;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace API.Analyzer
{
    public class Module
    {
        static async Task NewMain(string[] args)
        {
            HttpClient http = new HttpClient();                                                
            http.BaseAddress = new Uri("https://reqres.in/api/users?delay=3");              

            try
            {
                HttpResponseMessage response = await http.GetAsync(http.BaseAddress);      

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserResponse>(jsonContent);


                    if (result != null && result.Data != null)
                    {
                        foreach (var user in result.Data)
                        {
                            Console.WriteLine($"User ID: {user.Id}");
                            Console.WriteLine($"First Name: {user.First_name}");
                            Console.WriteLine($"Last Name: {user.Last_name}");
                            Console.WriteLine($"Email: {user.Email}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No user data found.");
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class UserResponse
        {
            public List<User> Data { get; set; }
        }


        public class User
        {
            public int Id { get; set; }
            public string First_name { get; set; }
            public string Last_name { get; set; }
            public string Email { get; set; }
            // public decimal amount { get; set; }

        }
        static bool ProfitablenessAccountCheck(int Id, decimal amount)
        {
            return true;
        }


    }

    //Calculation formulas
    //EPS = (Net Income) / (Number of Common Shares Outstanding) // MFS // EPS = (20.08 billion) / (7.43 billion) EPS = 2.7021
    //Нетни доходи//         //Акции в обръщение//
}

