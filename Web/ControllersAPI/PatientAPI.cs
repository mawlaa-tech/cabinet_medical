using System.Diagnostics;
using System.Net.Http.Headers;
using Microservice.Models;
using Newtonsoft.Json;

namespace Web.ControllersAPI
{
    public sealed class PatientAPI
    {
        private static readonly HttpClient client = new ();

        private PatientAPI()
        {
            client.BaseAddress = new Uri("https://localhost:7092");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new ();
        private static PatientAPI? instance;
        
        public static PatientAPI Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PatientAPI();
                    }
                    return instance;
                }
            }
        }

        // Gestion des appels vers l'API du CRUD du patient

        public static async Task<Uri?> AddPatientAsync(Patient patient)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Patients", patient);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<Uri?> UpdatePatientAsync(Patient patient)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Patients/" + patient.Id, patient);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<Uri?> DeletePatientAsync(long? id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/Patients/" + id);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<IList<Patient>?> GetPatientsAsync()
        {
            IList<Patient>? patients = new List<Patient>();
            HttpResponseMessage response = client.GetAsync("api/Patients").Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                patients = JsonConvert.DeserializeObject<List<Patient>>(resp);
            }
            return patients;
        }

        public async Task<Patient?> GetPatientAsync(long? id)
        {
            Patient? patient = null;
            HttpResponseMessage response = client.GetAsync("api/Patients/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                patient = JsonConvert.DeserializeObject<Patient>(resp);
            }
            return patient;
        }

        public async Task<Patient?> Login(Patient user)
        {
            Patient? patient = null;
            user.NomPat = "string";
            user.PrenPat = "string";
            user.DatNaisPat = DateTime.Now;
            user.SexPat = "homme";
            user.PhonePat = "5";
            HttpResponseMessage response = client.PostAsJsonAsync("api/Patients/login", user).Result;

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                patient = JsonConvert.DeserializeObject<Patient>(resp);
            }
            return patient;
        }
    }
}
