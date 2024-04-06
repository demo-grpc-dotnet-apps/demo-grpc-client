using System.Collections.Specialized;
using System.Net;
using System.Net.Mime;
using System.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Serilog;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DemoComp.DemoGrpcClient.Test.IntegrationTests.V1.Greeter;

/// <summary>
///     SayHello API の Integration Test Class.
/// </summary>
public class SayHelloApiTest : IClassFixture<WebApplicationFactory<Program>>
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
    private readonly HttpClient _client;

    /// <summary>
    ///     ログを出力するためのヘルパー
    /// </summary>
    private readonly TestOutputHelper _logOutput;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="webApplicationFactory">WebApplicationFactory</param>
    /// <param name="output">Logger</param>
    public SayHelloApiTest(WebApplicationFactory<Program> webApplicationFactory, ITestOutputHelper output)
    {
        _client = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        // Logger
        _logOutput = (TestOutputHelper)output;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.TestOutput(_logOutput)
            .CreateLogger();
    }

    /// <summary>
    ///     正常系テスト: パラメータが正常な場合、200 OK が返却されることを確認する。
    /// </summary>
    /// <param name="inputName">Request.Payload.name に指定する値</param>
    /// <param name="expectedHttpStatusCode">レスポンスのステータスコードの期待値</param>
    /// <param name="expectedResponseBodyPath">レスポンスのペイロードの期待値ファイルへのパス</param>
    [Theory(Skip = "Mock 化できていない")] // FIXME: Skip を外してテストを実行する
    [InlineData("Tanaka", HttpStatusCode.OK,
        "TestData/IntegrationTests/V1/Greeter/SayHelloApiTest/Response/NormalTest.json")]
    public async Task NormalTest(string inputName, HttpStatusCode expectedHttpStatusCode,
        string expectedResponseBodyPath)
    {
        // -------------------
        // Setup
        // -------------------
        // Request
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

        // Logging
        _logOutput.Output.Should().NotBeNullOrEmpty();
        _logOutput.Output.Should().Contain("INF] Received a request. name: Tanaka");
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
