using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    public Transform player_transform;
    [Range(0.001f,100)]
    public float simulation_speed = 1;
    [Range(0.001f,1)]
    public float performance_percentage = 1;
    public List<OrbitData> orbit_movement;
    public List<BodyBehaviour> land_movement;
    public List<SpriteRenderer> renderer_order;
    public List<Transform> look_at;
    public static PerformanceManager instance;
    private void Awake() {
        instance = this;
        Clear();
        StartCoroutine(BulkLoop());
        StartCoroutine(PreWarmRoutine());
    }
    IEnumerator PreWarmRoutine() {
        float set_sim = simulation_speed;
        simulation_speed = 100;
        yield return new WaitForSeconds(1);
        simulation_speed = set_sim;
    }

    IEnumerator BulkLoop() {
        while(true) {
            int i;
            OrbitData o;
            for (i = 0; i < orbit_movement.Count; i++){
                o = orbit_movement[i];
                Transform this_transform = o.transform;
                this_transform.Rotate(o.rotation_vector*simulation_speed);
                this_transform.position = o.following.position;
            }
            SpriteRenderer c;
            for (i = 0; i < renderer_order.Count; i++){
                c = renderer_order[i];
                c.transform.LookAt(player_transform.position, -Vector3.up);
                c.sortingOrder = (int)Vector3.Distance(transform.position,player_transform.position)*-1;                
            }
            BodyBehaviour b;
            for (i = land_movement.Count-1; i >= 0; i--){
                b = land_movement[i]; 
                if(b.continents == null)
                    land_movement.RemoveAt(i);
                else if(b.continents.Length <= 0)
                    land_movement.RemoveAt(i);
                else {
                    Transform[] con = b.continents;
                    for (int x = 0; x < con.Length; x++) {
                        Transform movingNow = con[x];
                        if(movingNow != null){
                            movingNow.localPosition+= new Vector3(0.1f*simulation_speed,0,0);
                            if(movingNow.localPosition.x > (b.maxX*b.scale)+(2/b.scale))
                                movingNow.localPosition = new Vector3(-(b.maxX*b.scale)-(2/b.scale),movingNow.localPosition.y,movingNow.localPosition.z);
                        }
                    }   
                }
            }
            //yield return new WaitForSeconds(1/performance_percentage*Time.deltaTime);
            yield return null;
        }
    }
    private void Clear() {
        orbit_movement = new List<OrbitData>();
        land_movement = new List<BodyBehaviour>();
        renderer_order = new List<SpriteRenderer>();
        look_at = new List<Transform>();
    }
}
