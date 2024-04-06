namespace DemoComp.DemoGrpcClient.Models.Api.V1.Greeter.SayHello;

[ToString]
public class SayHelloResponse(string message)
{
    public string Message { get; } = message;
}
