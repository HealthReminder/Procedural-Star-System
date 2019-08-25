using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform current_focused;
    private void Update() {
        if(Input.GetMouseButtonDown(0))
            OnLeftMouse();
        
        if(Input.GetMouseButtonDown(1))
            OnRightMouse();
    }
    public void OnRightMouse() {
        PlayerView.instance.UnfocusPlanet();
        current_focused = null;
    }
    public void OnLeftMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position,ray.direction*100,Color.white,5);
        if (Physics.Raycast(ray, out hit, 100)) {
            Debug.Log(hit.transform.name);
            Debug.DrawRay(transform.position,ray.direction*100,Color.red,5);
            if(hit.transform.tag == "Focusable") {
                if(current_focused == hit.transform)
                    return;
                current_focused = hit.transform;
                PlayerView.instance.FocusOnPlanet(hit.transform);
            }
        }
    }

    public static PlayerManager instance;
    private void Awake() {
        instance = this;
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
