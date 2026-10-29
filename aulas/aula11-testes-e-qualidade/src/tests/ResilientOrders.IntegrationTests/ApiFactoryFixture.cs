using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ResilientOrders.IntegrationTests;

// Sobe a API em memória (TestServer) para os testes de endpoint.
// O BaseUrl exposto é usado pelo RestSharp.
public class ApiFactoryFixture : IDisposable
{
    public WebApplicationFactory<Program> Factory { get; }
    public string BaseUrl { get; }
    public HttpClient HttpClient { get; }

    public ApiFactoryFixture()
    {
        Factory = new WebApplicationFactory<Program>();
        HttpClient = Factory.CreateClient();
        BaseUrl = HttpClient.BaseAddress!.ToString().TrimEnd('/');
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        Factory.Dispose();
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("api")]
public class ApiCollection : ICollectionFixture<ApiFactoryFixture> { }
