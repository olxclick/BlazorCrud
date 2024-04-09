using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Json;

namespace BlazorApp.Client.Services.SuperHeroService
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly HttpClient _http;

        private readonly NavigationManager _navigationManager;

        public List<SuperHero> Heroes { get; set; } = new List<SuperHero>();

        public List<Comic> Comics { get; set; } =   new List<Comic>();

        public SuperHeroService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
     
        public async Task GetComics()
        {
            var results = await _http.GetFromJsonAsync<List<Comic>>("api/superhero/comics");
            Comics = results;
        }

        public async Task<SuperHero> GetSingleHero(int id)
        {
            var results = await _http.GetFromJsonAsync<SuperHero>($"api/superhero/{id}");
            if (results == null)
                throw new Exception("Hero not found");
            return results;
        }

        public async Task GetSuperHeroes()
        {
            var results = await _http.GetFromJsonAsync<List<SuperHero>>("api/superhero");
            Heroes = results;
        }

        public async Task CreateHero(SuperHero entity)
        {
            var result = await _http.PostAsJsonAsync("api/superhero", entity);

            await SetHeroes(result);
        }

        public async Task UpdateHero(SuperHero entity)
        {
            var result = await _http.PutAsJsonAsync($"api/superhero/{entity.Id}", entity);
            await SetHeroes(result);
        }

        private async Task SetHeroes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            Heroes = response;

            _navigationManager.NavigateTo("superheroes");
        }

        public async Task DeleteHero(int id)
        {
            var result = await _http.DeleteAsync($"api/superhero/{id}");
            //var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            await SetHeroes(result);
        }
    }
}
