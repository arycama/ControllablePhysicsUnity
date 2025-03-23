//using Cinemachine;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class HovershipCamera : MonoBehaviour
//{
//    [SerializeField]
//    private float lookSpeed = 2f;

//    [SerializeField]
//    private CinemachineFreeLook freeLook;

//    [SerializeField]
//    private InputActionReference lookAction;

//    private Vector2 look;

//    private void OnEnable()
//    {
//        lookAction.action.Enable();
//    }

//    private void Update()
//    {
//        var lookInput = lookAction.action.ReadValue<Vector2>();
//        look = Vector2.LerpUnclamped(look, lookInput, lookSpeed * Time.deltaTime);

//        freeLook.m_XAxis.Value = Mathf.Lerp(freeLook.m_XAxis.m_MinValue, freeLook.m_XAxis.m_MaxValue, 0.5f * look.x + 0.5f);
//        freeLook.m_YAxis.Value = Mathf.Lerp(freeLook.m_YAxis.m_MinValue, freeLook.m_YAxis.m_MaxValue, 0.5f * look.y + 0.5f);
//    }
//}