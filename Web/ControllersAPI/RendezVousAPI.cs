using System.Diagnostics;
using System.Net.Http.Headers;
using Microservice.Models;
using Newtonsoft.Json;

namespace Web.ControllersAPI
{
    public sealed class RendezVousAPI
    {
        private static readonly HttpClient client = new();

        private RendezVousAPI()
        {
            client.BaseAddress = new Uri("https://localhost:7092");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new();
        private static RendezVousAPI? instance;

        public static RendezVousAPI Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RendezVousAPI();
                    }
                    return instance;
                }
            }
        }

        // Gestion des appels vers l'API du CRUD des rendez vous

        public static async Task<Uri?> AddRendezVous(RendezVous rendezVous)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/RendezVous", rendezVous);
                var resp = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<Uri?> DeleteRendezVous(long? id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/RendezVous/" + id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<IList<RendezVous>?> GetRendezVousPatientAsync(long patientId)
        {
            IList<RendezVous>? rendezVous = new List<RendezVous>();
            HttpResponseMessage response = client.GetAsync("api/RendezVous/patient/" + patientId).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                rendezVous = JsonConvert.DeserializeObject<List<RendezVous>>(resp);
            }
            return rendezVous;
        }

        public async Task<PaginatedList<PlageHoraire>> Search(PlageHoraire plageHoraire, int? pageNumber)
        {
            Debug.WriteLine("api/PlageHoraires/search?page=" + pageNumber + "&spe=" + plageHoraire.spe + "&acte=" + plageHoraire.acte);
            PaginatedList<PlageHoraire>? plageHoraires = null;
            HttpResponseMessage response = client.GetAsync("api/PlageHoraires/search?page=" + pageNumber + "&spe=" + plageHoraire.spe + "&acte=" + plageHoraire.acte).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                plageHoraires = JsonConvert.DeserializeObject<PaginatedList<PlageHoraire>>(resp);
            }
            if (plageHoraires == null)
                plageHoraires = new PaginatedList<PlageHoraire>();
            return plageHoraires;
        }

        public async Task<RendezVous?> GetRendezVousAsync(long? id)
        {
            RendezVous? rendezVous = null;
            HttpResponseMessage response = client.GetAsync("api/RendezVous/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                rendezVous = JsonConvert.DeserializeObject<RendezVous>(resp);
            }
            return rendezVous;
        }
    }
}
