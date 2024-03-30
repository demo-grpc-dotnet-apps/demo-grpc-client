using System.Net.Mime;
using DemoComp.DemoGrpcClient.Models.Greeter.SayHello;
using Microsoft.AspNetCore.Mvc;

namespace DemoComp.DemoGrpcClient.Controllers.V1.Greeter;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SayHelloController : ControllerBase
{

    [HttpGet]
    public ActionResult<SayHelloResponse> Get(string name)
    {
        return Ok(new SayHelloResponse($"Hello, {name}!"));
    }
}
