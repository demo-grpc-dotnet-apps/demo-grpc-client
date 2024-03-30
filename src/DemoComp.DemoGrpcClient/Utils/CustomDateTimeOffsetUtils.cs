using DemoComp.DemoGrpcClient.Utils.Interfaces;

namespace DemoComp.DemoGrpcClient.Utils;

public class CustomDateTimeOffsetUtils : IDateTimeOffset
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
