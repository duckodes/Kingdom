using System;
using System.Threading;
using System.Threading.Tasks;

public static class Wait
{
    public static Task Milliseconds(int ms, out Action cancel)
    {
        var cts = new CancellationTokenSource();
        cancel = () => cts.Cancel();

        return Task.Delay(ms, cts.Token);
    }
}