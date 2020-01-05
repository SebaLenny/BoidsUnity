public static class ExtensionMethods
{
    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) / (toLow - fromLow) * (toHigh - fromHigh) + fromHigh;
    }
}
