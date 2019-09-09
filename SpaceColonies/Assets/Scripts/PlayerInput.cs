using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]   public class MyFloatEvent : UnityEvent<float>
{
}
public class PlayerInput : MonoBehaviour
{
    public MyFloatEvent on_mouse_wheel_up;
    public MyFloatEvent on_mouse_wheel_down;
    public UnityEvent on_right_button_down;

    float f;
    private void Update() {
        f = Input.GetAxis("Mouse ScrollWheel");
        if (f > 0f )
            on_mouse_wheel_up.Invoke(f);
        if (f < 0f )
            on_mouse_wheel_down.Invoke(f);
        
        if(Input.GetMouseButton(1))
            on_right_button_down.Invoke();
    }
}
