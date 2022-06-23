using System.Diagnostics;
using System.Net.Http.Headers;
using Microservice.Models;
using Newtonsoft.Json;

namespace Web.ControllersAPI
{
    public sealed class AdminAPI
    {
        private static readonly HttpClient client = new ();

        private AdminAPI()
        {
            client.BaseAddress = new Uri("https://localhost:7092");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new ();
        private static AdminAPI? instance;
        
        public static AdminAPI Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AdminAPI();
                    }
                    return instance;
                }
            }
        }

        // COnnexion administrateur
        public async Task<Admin?> Login(Admin user)
        {
            Admin? admin = null;
            HttpResponseMessage response = client.PostAsJsonAsync("api/Admins/login", user).Result;

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                admin = JsonConvert.DeserializeObject<Admin?>(resp);
            }
            return admin;
        }
    }
}
