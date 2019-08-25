using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Camera player_camera;
    bool is_camera_moving = false;

    public static PlayerView instance;
    private void Awake() {
        instance = this;
    }
    public void FocusOnPlanet(Transform planet) {
        StartCoroutine(FocusPlanetRoutine(planet));
    }
    IEnumerator FocusPlanetRoutine(Transform planet) {
        
        StopCoroutine(MoveCamera(null));
        yield return MoveCamera(planet);
        player_camera.transform.parent = planet;
        yield break;
    }
    public void UnfocusPlanet() {
        StartCoroutine(UnfocusPlanetRoutine());
    }
    IEnumerator UnfocusPlanetRoutine() {
        StopCoroutine(MoveCamera(null));
        yield return MoveCamera(null);
        player_camera.transform.parent = null;
        yield break;
    }
    IEnumerator MoveCamera(Transform aligning_to) {
        print("Here");
        while(is_camera_moving == true)
                yield return null;
        is_camera_moving = true;


        Vector3 new_position = aligning_to.position;
        Quaternion new_rotation;
        if(aligning_to != null){
            new_position = new Vector3(new_position.x,new_position.y,-20);
            new_rotation = Quaternion.FromToRotation(player_camera.transform.up, aligning_to.up) * player_camera.transform.rotation;
        } else {
            new_position = new Vector3(0,0,-20);
            new_rotation = Quaternion.identity;
        }
        Transform camera_transform = player_camera.transform;
        camera_transform.parent = null;
        float progress = 0;
        while(camera_transform.position != new_position){
            if(aligning_to != null) {
                new_position =new Vector3(aligning_to.position.x,aligning_to.position.y,-20);
                new_rotation = Quaternion.FromToRotation(player_camera.transform.up, aligning_to.up) * player_camera.transform.rotation;
            }
            camera_transform.position = Vector3.Lerp(camera_transform.position,new_position,progress);
            camera_transform.rotation = Quaternion.Lerp(camera_transform.rotation,new_rotation,progress);
            progress += Time.deltaTime*1;
            yield return null;
        }
        is_camera_moving = false;
    }
}
