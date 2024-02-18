using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using welcomeApp.Models;

namespace welcomeApp.Services
{
    public class OfficeLocationService
    {
        private readonly HttpClient _httpClient;

        public OfficeLocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCityNameByRegionAsync(int regionCode)
        {
            var citiesResponse = await _httpClient.GetAsync("https://europlan.ru/api/office/cities");
            citiesResponse.EnsureSuccessStatusCode();

            var citiesContent = await citiesResponse.Content.ReadAsStringAsync();
            var cities = JsonSerializer.Deserialize<List<City>>(citiesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var city = cities?.FirstOrDefault(c => c.RegionCode == regionCode.ToString());
            return city?.Name ?? string.Empty;
        }

        public async Task<string> GetOfficeAddressByCityNameAsync(string cityName)
        {
            var officesResponse = await _httpClient.GetAsync("https://europlan.ru/api/office/offices");
            officesResponse.EnsureSuccessStatusCode();

            var officesContent = await officesResponse.Content.ReadAsStringAsync();
            var offices = JsonSerializer.Deserialize<List<OfficeInfo>>(officesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var office = offices?.FirstOrDefault(o => string.Equals(o.CityName, cityName, StringComparison.OrdinalIgnoreCase));

            return office?.Address ?? "Адрес не найден";
        }

        private class City
        {
            public string? Name { get; set; }
            public string? RegionCode { get; set; }
        }

        private class OfficeInfo
        {
            public string? Address { get; set; }
            public string? CityName { get; set; }
        }
    }
}
