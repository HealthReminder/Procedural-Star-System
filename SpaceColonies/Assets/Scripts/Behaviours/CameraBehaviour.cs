using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    bool is_focusing = false;
    bool can_zoom = true;
    bool can_orbit = true;
    public Transform focusing_transform;
    public AnimationCurve smooth_curve;

    public static CameraBehaviour instance;
    private void Awake() {
        instance = this;
    }
    
    float zoom_prog = 0.1f;
    bool is_positive = true;
    public void Zoom(float rate) {
        if(!can_zoom || focusing_transform == null)
            return;

        float max_distance = 2000;
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
    public IEnumerator FocusTransform(Transform focus_on,float ideal_distance) {
        is_focusing = false;
        yield return null;
        is_focusing = true;

        TimeControllerView.instance.SetTimeSpeed(1);

        can_zoom = false;
        can_orbit = false;
        focusing_transform = focus_on;

        //Free camera
        transform.parent = null;
        
        
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
        can_orbit = true;
        yield break;
    }
    
    public void Orbit() {
        //if(!can_orbit || focusing_transform == null)
        //    return;
        //Vector3 mouse_pos = Input.mousePosition;
        //Vector3 rotation_vector = new Vector3(mouse_pos.y-Screen.height/2,mouse_pos.x-Screen.width/2,0).normalized;
        //print(rotation_vector);
        //transform.RotateAround(focusing_transform.position,mouse_pos,2f);
        //if(rotation_vector.x < 0)
        //    transform.RotateAround (focusing_transform.position, focusing_transform.right, 100 * Time.deltaTime);
        //else if(rotation_vector.x > 0)
        //    transform.RotateAround (focusing_transform.position, -focusing_transform.right, 100 * Time.deltaTime);
        //if(rotation_vector.y < 0)
        //    transform.RotateAround (focusing_transform.position, focusing_transform.up, 100 * Time.deltaTime);
        //else if(rotation_vector.y > 0)
        //    transform.RotateAround (focusing_transform.position, -focusing_transform.up, 100 * Time.deltaTime);
    }


}
