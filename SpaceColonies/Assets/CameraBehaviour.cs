using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    bool is_focusing = false;
    public Transform focusingTest;
    public AnimationCurve smooth_curve;
    public IEnumerator FocusTransform(Transform focus_on) {
        is_focusing = false;
        yield return null;
        is_focusing = true;

        //Free camera
        transform.parent = null;
        float ideal_distance = 100;

        //Create references
        Vector3 target_vector = Vector3.zero;
        float percentage_rotation = 0;   
        float curve_progress = 0;
        Quaternion toRotation = Quaternion.identity;
        while(is_focusing) {
            Vector3 my_pos = transform.position;
            Vector3 focus_pos = focus_on.position;
            float current_distance = Vector3.Distance(my_pos,focus_pos);
            target_vector =  (focus_pos - my_pos).normalized;

            if(current_distance > ideal_distance)
                transform.position = (target_vector*5*smooth_curve.Evaluate(curve_progress))+ my_pos;
            else if(current_distance <= ideal_distance - 7)
                transform.position = (target_vector*-5*smooth_curve.Evaluate(curve_progress))+ my_pos;

            if(percentage_rotation <= 1 && target_vector.magnitude != 0){
                toRotation = Quaternion.LookRotation(target_vector,transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, percentage_rotation*smooth_curve.Evaluate(curve_progress));
                percentage_rotation+=Time.deltaTime;
            }

            if(Mathf.Abs(ideal_distance - current_distance) <= 8 && percentage_rotation >= 1) {
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1);
                transform.parent = focus_on;
                is_focusing = false;
            }

            if(curve_progress <= 1)
                curve_progress+=0.01f;
            yield return null;
        }
        yield break;
    }

    public static CameraBehaviour instance;
    private void Awake() {
        instance = this;
    }
}
