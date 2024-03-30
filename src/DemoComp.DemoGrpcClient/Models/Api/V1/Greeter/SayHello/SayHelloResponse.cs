namespace DemoComp.DemoGrpcClient.Models.Greeter.SayHello;

public class SayHelloResponse(string name)
{
    public string Name { get; } = name;
}
