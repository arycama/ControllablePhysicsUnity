//using System;
//using UnityEngine;

//[Serializable]
//public class Vector3Pid
//{
//    [SerializeField]
//    private float proportionalGain;

//    [SerializeField]
//    private float integralGain;

//    [SerializeField]
//    private float derivativeGain;

//    [SerializeField]
//    private float maxIntegral;

//    private Vector3 lastError, integral;

//    public Vector3 Update(Vector3 error, float timeFrame)
//    {
//        integral = (integral + error * timeFrame).Clamp(-maxIntegral, maxIntegral);

//        var deriv = (error - lastError) / timeFrame;
//        lastError = error;

//        return error * proportionalGain + integral * integralGain + deriv * derivativeGain;
//    }
//}