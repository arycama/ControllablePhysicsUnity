using System;
using UnityEngine;

[Serializable]
public class FloatPid
{
    [SerializeField]
    private float proportionalGain;

    [SerializeField]
    private float integralGain;

    [SerializeField]
    private float derivativeGain;

    [SerializeField]
    private float maxIntegral;

    private float lastError, integral;

    public float Update(float error, float timeFrame)
    {
        integral = Mathf.Clamp(integral + error * timeFrame, -maxIntegral, maxIntegral);

        var deriv = (error - lastError) / timeFrame;
        lastError = error;

        return error * proportionalGain + integral * integralGain + deriv * derivativeGain;
    }
}