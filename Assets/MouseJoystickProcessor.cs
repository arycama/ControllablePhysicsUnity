//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//#if UNITY_EDITOR
//using UnityEditor;

//[InitializeOnLoad]
//#endif
//public class MouseJoystickProcessor : InputProcessor<Vector2>
//{
//#if UNITY_EDITOR
//    static MouseJoystickProcessor()
//    {
//        Initialize();
//    }
//#endif

//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    static void Initialize()
//    {
//        InputSystem.RegisterProcessor<MouseJoystickProcessor>();
//    }

//    public override Vector2 Process(Vector2 value, InputControl control)
//    {
//        return new Vector2(value.x / Screen.width * 2f - 1f, value.y / Screen.height * 2f - 1f);
//    }
//}
