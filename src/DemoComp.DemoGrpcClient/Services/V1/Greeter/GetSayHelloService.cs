using DemoComp.DemoGrpcClient.Models.Api.V1.Greeter.SayHello;
using DemoComp.DemoGrpcClient.Proto;
using DemoComp.DemoGrpcClient.Repositories;

namespace DemoComp.DemoGrpcClient.Services.V1.Greeter;

public class GetSayHelloService(GreetRepository greetRepository)
{
    public SayHelloResponse GetSayHello(string name)
    {
        var request = new HelloRequest
        {
            Name = name
        };
        var reply = greetRepository.GetSayHello(request);

        return new SayHelloResponse(reply.Message);
    }
}
