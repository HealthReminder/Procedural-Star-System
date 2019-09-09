using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform star_transform;
    public Transform current_focused;

    public static PlayerManager instance;
    private void Awake() {
        instance = this;        
    }
    private void Start() {
        CameraBehaviour.instance.FocusTransform(star_transform,1000);
    }
    private void Update() {
        if(Input.GetMouseButtonDown(0))
            OnLeftMouse();
        
        if(Input.GetMouseButtonDown(1))
            OnRightMouse();
    }
    public void OnRightMouse() {
        //PlayerView.instance.UnfocusPlanet();
        current_focused = null;
    }
    public void OnLeftMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position,ray.direction*100,Color.white,5);
        if (Physics.Raycast(ray, out hit, 10000)) {
            Debug.Log("Clicked on "+hit.transform.name);
            Debug.DrawRay(transform.position,ray.direction*100,Color.red,5);
            if(hit.transform.tag == "Focusable") {
                if(current_focused == hit.transform)
                    return;
                current_focused = hit.transform;
                //PlayerView.instance.FocusOnPlanet(hit.transform);
                BodyBehaviour hit_body = hit.transform.GetComponent<BodyBehaviour>();
                if(hit_body.data.type == "Star")
                    StartCoroutine(CameraBehaviour.instance.FocusTransform(current_focused,500));
                else if(hit_body.data.type == "Planet")
                    StartCoroutine(CameraBehaviour.instance.FocusTransform(current_focused,100));
                else 
                    StartCoroutine(CameraBehaviour.instance.FocusTransform(current_focused,25));
            }
        }
    }
    public void FocusStar() {
        StartCoroutine(CameraBehaviour.instance.FocusTransform(star_transform,500));
    }
    /*
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Input.mousePosition);
        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.tag == "Focusable") {
                current_focused = hit.transform;
                PlayerView.instance.FocusOnPlanet(hit.transform);
            }
        } */
}
