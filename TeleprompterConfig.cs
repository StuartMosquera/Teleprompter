using static System.Math;

namespace Teleprompter;

internal class TeleprompterConfig
{
    public int DelayInMilliseconds { get; private set; } = 200;
    public bool Done { get; private set; }

    public void UpdateDelay(int increment) // Negative to speed up
    {
        var newDelay = Min(DelayInMilliseconds + increment, 1000);
        newDelay = Max(newDelay, 25);
        DelayInMilliseconds = newDelay;
    }

    public void SetDone()
    {
        Done = true;
    }
}
