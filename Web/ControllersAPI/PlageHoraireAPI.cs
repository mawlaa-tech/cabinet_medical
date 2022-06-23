using System.Diagnostics;
using System.Net.Http.Headers;
using Microservice.Models;
using Newtonsoft.Json;

namespace Web.ControllersAPI
{
    public sealed class PlageHoraireAPI
    {
        private static readonly HttpClient client = new();

        private PlageHoraireAPI()
        {
            client.BaseAddress = new Uri("https://localhost:7092");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new();
        private static PlageHoraireAPI? instance;

        public static PlageHoraireAPI Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PlageHoraireAPI();
                    }
                    return instance;
                }
            }
        }

        // Gestion de la connexion du medecin

        public async Task<Medecin?> Login(Medecin? medecin)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Medecins", medecin).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                medecin = JsonConvert.DeserializeObject<Medecin>(resp);
            }
            return medecin;
        }

        // Gestion des appels vers l'API du CRUD de la plage horaire

        public static async Task<Uri?> AddPlageHoraire(PlageHoraire plageHoraire)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/PlageHoraires", plageHoraire);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<PlageHoraire?> CheckDateAsync(PlageHoraire plageHoraire)
        {
            PlageHoraire? PlageHoraire = null;
            HttpResponseMessage response = await client.PostAsJsonAsync("api/PlageHoraires/check/date", plageHoraire);
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                PlageHoraire = JsonConvert.DeserializeObject<PlageHoraire>(resp);
            }
            return PlageHoraire;
        }

        public static async Task<PlageHoraire?> CheckTimeAsync(PlageHoraire plageHoraire)
        {
            PlageHoraire? PlageHoraire = null;
            HttpResponseMessage response = await client.PostAsJsonAsync("api/PlageHoraires/check/time", plageHoraire);
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                PlageHoraire = JsonConvert.DeserializeObject<PlageHoraire>(resp);
            }
            return PlageHoraire;
        }

        public static async Task<Uri?> UpdatePlageHoraire(PlageHoraire plageHoraire)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/PlageHoraires/" + plageHoraire.Id, plageHoraire);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<Uri?> DeletePlageHoraire(long? id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/PlageHoraires/" + id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<IList<PlageHoraire>?> GetPlageHorairesAsync(long medecinId)
        {
            IList<PlageHoraire>? plageHoraires = new List<PlageHoraire>();
            HttpResponseMessage response = client.GetAsync("api/PlageHoraires/medecin/" + medecinId).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                plageHoraires = JsonConvert.DeserializeObject<List<PlageHoraire>>(resp);
            }
            return plageHoraires;
        }

        public async Task<PlageHoraire?> GetPlageHoraireAsync(long? id)
        {
            PlageHoraire? plageHoraire = null;
            HttpResponseMessage response = client.GetAsync("api/PlageHoraires/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                plageHoraire = JsonConvert.DeserializeObject<PlageHoraire>(resp);
            }
            return plageHoraire;
        }

        // Gestion de  la recuperation de la liste des specialites et actes medicaux

        public async Task<string?> GetActeMedicauxAsync(long specialiteId)
        {
            var acteMedicaux = "";
            HttpResponseMessage response = client.GetAsync("api/ActeMedicaux/specialite/" + specialiteId).Result;
            if (response.IsSuccessStatusCode)
            {
                acteMedicaux = await response.Content.ReadAsStringAsync();
            }
            return acteMedicaux;
        }

        public async Task<IList<ActeMedical>?> GetActeMedicauxAsyncNoJson(long specialiteId)
        {
            IList<ActeMedical>? acteMedicaux = new List<ActeMedical>();
            HttpResponseMessage response = client.GetAsync("api/ActeMedicaux/specialite/" + specialiteId).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                acteMedicaux = JsonConvert.DeserializeObject<List<ActeMedical>>(resp);
            }
            return acteMedicaux;
        }

        public async Task<IList<Specialite>?> GetSpecialitesAsync(long medecinId)
        {
            Medecin? medecin = null;
            HttpResponseMessage response = client.GetAsync("api/Medecins/" + medecinId + "/specialites").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                medecin = JsonConvert.DeserializeObject<Medecin>(resp);
            }
            return medecin!.Specialites;
        }
    }
}
