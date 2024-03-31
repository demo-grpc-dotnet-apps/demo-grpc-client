using System.Collections.Specialized;
using System.Net;
using System.Net.Mime;
using System.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace DemoComp.DemoGrpcClient.Test.IntegrationTests.V1.Greeter;

/// <summary>
///     SayHello API の Integration Test Class.
/// </summary>
/// <param name="webApplicationFactory">
///     テスト対象のWebアプリケーションのファクトリ
/// </param>
public class SayHelloApiTest(WebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    ///     テスト対象のAPIエンドポイントのURIを構築するためのビルダー
    /// </summary>
    private readonly UriBuilder _targetApiEndPointBuilder = new("https", "localhost", 7191, "api/v1.0/SayHello");

    /// <summary>
    ///     クエリパラメータのビルダー
    /// </summary>
    private readonly NameValueCollection _queryParameters = HttpUtility.ParseQueryString(string.Empty);

    /// <summary>
    ///     テスト用のHTTPクライアント
    /// </summary>
    private readonly HttpClient _client = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        { AllowAutoRedirect = false });

    [Theory]
    [InlineData("Tanaka", HttpStatusCode.OK,
        "TestData/IntegrationTests/V1/Greeter/SayHelloApiTest/Response/NormalTest.json")]
    public async Task NormalTest(string inputName, HttpStatusCode expectedHttpStatusCode,
        string expectedResponseBodyPath)
    {
        // -------------------
        // Setup
        // -------------------
        _queryParameters.Add("name", inputName);
        _targetApiEndPointBuilder.Query = _queryParameters.ToString();
        var expectedResponseBody = RemoveNewLine(await File.ReadAllTextAsync(expectedResponseBodyPath));

        // -------------------
        // Exercise
        // -------------------
        var response = await _client.GetAsync(_targetApiEndPointBuilder.Uri);

        // -------------------
        // Verify
        // -------------------
        // Status Code
        response.StatusCode.Should().Be(expectedHttpStatusCode);

        // HTTP Headers
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        // Response Body
        var responseBody = JToken.Parse(await response.Content.ReadAsStringAsync()).ToString();
        RemoveNewLine(responseBody).Should().Be(expectedResponseBody);
    }

    /// <summary>
    ///     改行コードを削除する
    /// </summary>
    /// <param name="value">
    ///     対象の文字列
    /// </param>
    /// <returns>
    ///     改行コードを削除した文字列
    /// </returns>
    private static string RemoveNewLine(string value)
    {
        return value
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);
    }
}
