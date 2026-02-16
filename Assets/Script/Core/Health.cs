using System;

public class Health
{
    public enum States
    {
        Reduce = -1,
        None,
        Add,
    }
    public Health(int min = 0, int max = 100)
    {
        Min = min;
        Max = max;
    }
    public readonly int Min;
    public readonly int Max;
    private float val;
    public float Value
    {
        get => val;
        set
        {
            lastValue = value;
            if (value > val)
            {

            }
            val = Math.Clamp(value, Min, Max);
        }
    }
    private float lastValue;
    public float Percentage => (float)Math.Round(val / 100f, 2);
    public void Fill() => val = Max;
    public States State
    {
        get
        {
            if (lastValue < val) return States.Add;
            if (lastValue > val) return States.Reduce;
            return States.None;
        }
    }
}