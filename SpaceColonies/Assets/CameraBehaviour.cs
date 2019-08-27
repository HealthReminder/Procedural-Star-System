using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform focusingTest;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.F))
            FocusTransform(focusingTest);
    }
    public void FocusTransform(Transform focus_on) {
        Vector3 target_vector =  transform.position - focus_on.position;
        transform.position = (target_vector.normalized*20)+ focus_on.position;
        transform.LookAt(focus_on);
        transform.parent = focus_on;
        print(target_vector);
    }
}
