using System.Net.Mime;
using DemoComp.DemoGrpcClient.Models.Greeter.SayHello;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DemoComp.DemoGrpcClient.Controllers.V1.Greeter;

/// <summary>
///     SayHello API Controller.
/// </summary>
/// <remarks>
///    version: 1.0
/// </remarks>
/// <param name="logger">
///     Logger
/// </param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SayHelloController : ControllerBase
{
    /// <summary>
    ///     Get SayHello Message API.
    /// </summary>
    /// <param name="name">
    ///     name in query parameter.
    /// </param>
    /// <returns>
    ///     SayHelloResponse.
    /// </returns>
    [HttpGet]
    public ActionResult<SayHelloResponse> Get(string name)
    {
        // Start Logging
        Log.Information("Received a request. name: {name}", name);

        // Business Logic
        var responseBody = new SayHelloResponse($"Hello, {name}!");
        var response = Ok(responseBody);

        // End Logging
        Log.Information("Create a response. Status: {response.StatusCode}, Response Body: {responseBody}",
            response.StatusCode, responseBody);

        // Response
        return response;
    }
}
