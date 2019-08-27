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
