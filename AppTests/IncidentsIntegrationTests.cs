using IncidentApiRimel.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace AppTests
{
    public class IncidentsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public IncidentsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetIncidents_ReturnsOk()
        {
            var response = await _client.GetAsync("api/IncidentsDb/get-all");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task PostIncident_CreatesIncident()
        {
            var incident = new
            {
                Title = "Test Incident",
                Description = "Test Description",
                Severity = "HIGH"
            };
            var response = await _client.PostAsJsonAsync("api/IncidentsDb/create-incident", incident, cancellationToken: TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var createdIncident = await response.Content.ReadFromJsonAsync<Incident>(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(createdIncident);
            Assert.Equal("Test Incident", createdIncident.Title);
            Assert.Equal("HIGH", createdIncident.Severity);
        }
        [Fact]
        public async Task PostThenGet_ReturnsInsertedIncident()
        {
            var incident = new
            {
                Title = "Integration Test",
                Description = "Test Description",
                Severity = "MEDIUM"
            };
            await _client.PostAsJsonAsync("api/IncidentsDb/create-incident", incident);
            var response = await _client.GetAsync("api/IncidentsDb/get-all");
            var data = await response.Content.ReadFromJsonAsync<List<Incident>>();
            Assert.Contains(data, i => i.Title == "Integration Test");
        }
    }
}
