using DemoComp.DemoGrpcClient.Proto;
using Serilog;

namespace DemoComp.DemoGrpcClient.Repositories;

public class GreetRepository(Greeter.GreeterClient client)
{
    public HelloReply GetSayHello(HelloRequest request)
    {
        Log.Information("Call SayHello. Request: {@request}", request);

        try
        {
            var reply = client.SayHello(request);
            reply.Message = "Hello, " + request.Name + "!";
            Log.Information("Reply from SayHello: {@reply}", reply);
            return reply;
        }
        catch (Exception e)
        {
            Log.Warning(e, "Exception occurred while calling SayHello.");
            throw;
        }
    }
}
