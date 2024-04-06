using System.Net.Mime;
using DemoComp.DemoGrpcClient.Models.Api.V1.Greeter.SayHello;
using DemoComp.DemoGrpcClient.Services.V1.Greeter;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DemoComp.DemoGrpcClient.Controllers.V1.Greeter;

/// <summary>
///     SayHello API Controller.
/// </summary>
/// <remarks>
///    version: 1.0
/// </remarks>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SayHelloController(GetSayHelloService getSayHelloService) : ControllerBase
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
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    public ActionResult<SayHelloResponse> Get(string name)
    {
        // Start
        Log.Information("Received a request. name: {name}", name);

        try
        {
            // Business Logic
            var responseBody = getSayHelloService.GetSayHello(name);

            // End
            Log.Information("Create a response. Response Body: {responseBody}", responseBody);

            // Response
            return Ok(responseBody);
        }
        catch (Exception e)
        {
            // TODO: Global Error Handler

            Log.Error(e, "An error occurred.");
            throw;
        }
    }
}
