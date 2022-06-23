using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microservice.Models;
using Newtonsoft.Json;

namespace Web.ControllersAPI
{
    public sealed class MedecinAPI
    {
        private static readonly HttpClient client = new();

        private MedecinAPI()
        {
            client.BaseAddress = new Uri("https://localhost:7092");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new();
        private static MedecinAPI? instance = null;

        public static MedecinAPI Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MedecinAPI();
                    }
                    return instance;
                }
            }
        }
        public static async Task<Uri?> AddMedecinAsync(Medecin medecin)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Medecins", medecin);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static async Task<Uri?> UpdateMedecinAsync(Medecin medecin)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Medecins/" + medecin.Id, medecin);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public static async Task<Uri?> DeleteMedecinAsync(long id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/Medecins/" +id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<IList<Medecin>?> GetMedecinsAsync(string searchString)
        {
            HttpResponseMessage response;
            IList<Medecin>? medecins = new List<Medecin>();
            if(searchString != null) {
                 response = client.GetAsync("api/Medecins/search/" + searchString).Result;
            }
            else
            {
                response = client.GetAsync("api/Medecins").Result;

            }
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                medecins = JsonConvert.DeserializeObject<List<Medecin>>(resp);
            }
            return medecins;
        }

        public async Task<Medecin?> GetMedecinAsync(long? id)
        {
            Medecin? medecin = null;
            HttpResponseMessage response = client.GetAsync("api/Medecins/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                medecin = JsonConvert.DeserializeObject<Medecin>(resp);
            }
            return medecin;
        }

        public async Task<Medecin?> Login(Medecin user)
        {
            Medecin? medecin = null;
            user.NomMedecin = "string";
            user.PrenomMedecin  = "string";
            user.SpecialitesId = new List<long>();
            user.SpecialitesId.Add(1);
            Debug.WriteLine("Ici");
            HttpResponseMessage response = client.PostAsJsonAsync("api/Medecins/login", user).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                medecin = JsonConvert.DeserializeObject<Medecin>(resp);
            }
            return medecin;
        }

        public async Task<IList<Specialite>?> GetSpecialitesAsync()
        {
            IList<Specialite>? specialites = new List<Specialite>();
            HttpResponseMessage response = client.GetAsync("api/Specialites").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                specialites = JsonConvert.DeserializeObject<IList<Specialite>>(resp);
            }
            return specialites;
        }

    }
}
