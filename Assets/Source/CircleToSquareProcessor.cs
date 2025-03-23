//using UnityEngine;
//using UnityEngine.InputSystem;

//#if UNITY_EDITOR
//using UnityEditor;

//[InitializeOnLoad]
//#endif
//public class CircleToSquareProcessor : InputProcessor<Vector2>
//{
//#if UNITY_EDITOR
//    static CircleToSquareProcessor()
//    {
//        Initialize();
//    }
//#endif

//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    static void Initialize()
//    {
//        InputSystem.RegisterProcessor<CircleToSquareProcessor>();
//    }

//    public override Vector2 Process(Vector2 value, InputControl control)
//    {
//        if (value.x != 0f || value.y != 0f)
//        {
//            var absX = Mathf.Abs(value.x);
//            var absY = Mathf.Abs(value.y);

//            var g = Mathf.Min(absX, absY) / Mathf.Max(absX, absY);
//            value *= Mathf.Sqrt(g * g + 1f);
//        }

//        return value;
//    }
//}