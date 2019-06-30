using UnityEngine;

public static class Mathematics
{
    private const float stdDev = 0.3989426f;

    // The normal distribution function.
    public static float GetHeightAtPointOnNormalDistribution(float _mean, float _samplePoint)
    {
        float variance = stdDev * stdDev;
        float oneOver2Pi = (float)(1.0 / (stdDev * Mathf.Sqrt(2 * Mathf.PI)));
        float heightAtTarget = oneOver2Pi * Mathf.Exp(-(_samplePoint - _mean) * (_samplePoint - _mean) / (2 * variance));
        return heightAtTarget;
    }

    // Get cauchy distributed random number
    public static float GetCauchyDistributedRandomNumber()
    {
        float random = (float)GameController.random.NextDouble();
        return ((Mathf.Clamp(Mathf.Tan(Mathf.PI * (random - 0.5f)), -30f, 30f) + 30f) / 60f) + 0.001f;
    }
}
