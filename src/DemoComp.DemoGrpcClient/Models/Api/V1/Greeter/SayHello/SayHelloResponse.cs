namespace DemoComp.DemoGrpcClient.Models.Greeter.SayHello;

[ToString]
public class SayHelloResponse(string name)
{
    public string Name { get; } = name;
}
