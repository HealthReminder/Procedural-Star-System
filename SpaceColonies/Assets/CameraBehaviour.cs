using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    bool is_focusing = false;
    bool can_zoom = true;
    public Transform focusing_transform;
    public AnimationCurve smooth_curve;

    float zoom_prog = 0.1f;
    bool is_positive = true;
    public void Zoom(float rate) {
        if(!can_zoom || focusing_transform == null)
            return;

        float max_distance = 250;
        float min_distance = 10;
        float movement_velocity = 20;
        float current_distance = Vector3.Distance(transform.position, focusing_transform.position);
        if(rate > 0) {
            if(!is_positive) {
                is_positive = true;
                zoom_prog = 0.1f;
            }
            if(current_distance > min_distance) {
                transform.position = zoom_prog*transform.forward*movement_velocity + transform.position;
                if(zoom_prog < 1)
                    zoom_prog += Time.deltaTime;
            }
        } else {
            if(is_positive) {
                is_positive = false;
                zoom_prog = 0.1f;
            }
            if(current_distance < max_distance) {
                transform.position = zoom_prog*transform.forward*-movement_velocity + transform.position;
                if(zoom_prog < 1)
                    zoom_prog += Time.deltaTime;
            }
        }
    }
    public IEnumerator FocusTransform(Transform focus_on) {
        is_focusing = false;
        yield return null;
        is_focusing = true;

        can_zoom = false;
        focusing_transform = focus_on;

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
        can_zoom = true;
        yield break;
    }

    public static CameraBehaviour instance;
    private void Awake() {
        instance = this;
    }
}
