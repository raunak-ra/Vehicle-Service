using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Common.Model;
using System.Linq;

namespace WebApp.Service
{
    public class VehicleService
    {
        public async Task<List<Vehicle>> GetVehiclesAsync(int startYear, int endYear)
        {
            var tasks = new List<Task>();
            using (var client = new HttpClient())
            {
                for (int i = startYear; i <= endYear; i++)
                {
                    string uri = $"https://vpic.nhtsa.dot.gov/api//vehicles/getmodelsformakeyear/make/honda/modelyear/{i}?format=json";
                    tasks.Add(client.GetAsync(uri));
                }

                await Task.WhenAll(tasks);
            }

            var vehicleData = GetVehicleData(tasks);
            var filteredVehicle = GetFilteredVehicle(startYear, endYear, vehicleData);
            return filteredVehicle;
        }

        private static Dictionary<int, List<Vehicle>> GetVehicleData(List<Task> tasks)
        {
            var vpicResponses = new List<VpicResponse>();
            tasks.ForEach(async task =>
            {
                var httpResponse = ((Task<HttpResponseMessage>)task).Result;
                var response = await httpResponse.Content.ReadAsStringAsync();
                vpicResponses.Add(JsonConvert.DeserializeObject<VpicResponse>(response));
            });

            var vehicleData = new Dictionary<int, List<Vehicle>>();

            foreach (var response in vpicResponses)
            {
                var citeria = response.SearchCriteria;
                var year = Convert.ToInt32(citeria.Substring(citeria.Count() - 4));
                vehicleData[year] = response.Results;
            }

            return vehicleData;
        }

        private static List<Vehicle> GetFilteredVehicle(int startYear, int endYear, Dictionary<int, List<Vehicle>> vehicleData)
        {
            var vehiclesNeedToExclude = vehicleData[endYear - 1];
            vehiclesNeedToExclude.AddRange(vehicleData[endYear]);

            var filteredVehicle = new List<Vehicle>();

            for (int i = startYear; i <= endYear - 2; i++)
            {
                foreach (var vehicle in vehicleData[i])
                {
                    if (vehiclesNeedToExclude.Exists(bike => bike.Model_ID == vehicle.Model_ID) == false)
                        filteredVehicle.Add(vehicle);
                }
            }

            return filteredVehicle;
        }
    }
}
