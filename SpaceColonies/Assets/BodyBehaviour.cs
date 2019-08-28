using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   public class BodyData {
    public float initial_size;
    public float orbit_radius;
    public float orbit_velocity;
    public Color bottom_color;
    public Color upper_color;
    public int children_count;
    public float trail_duration;
}
public class BodyBehaviour : MonoBehaviour
{
    public string type;
    [SerializeField] public BodyData data;
    public GameObject object_appearance;
    public Transform orbiting;
    public TrailRenderer trail;
    int continentQuantity = 0;
    [HideInInspector] public float maxY = 5;
    [HideInInspector] public float maxX = 15;
    [HideInInspector] public Transform[] continents;
    [HideInInspector] public float scale;
    private void Start() {
        StartCoroutine(Behaviour());
    }
    IEnumerator Behaviour() {
        if(type == "Star")
            BodyController.instance.SetupBody(this,"Root");
        yield return null;
        //print("Starting generation for "+type);
        transform.localScale = Vector3.one*data.initial_size;
        scale = transform.lossyScale.x;
        yield return GenerateContinents();
        yield return GenerateChildren();
        if(trail){
            trail.startColor = data.upper_color + new Color(0,0,0,-0.5f);
            trail.endColor = data.bottom_color+ new Color(0,0,0,-0.5f);
            trail.time = data.trail_duration;
            trail.startWidth = data.initial_size*10;
        }
        PerformanceManager.instance.land_movement.Add(this);
        //Debug.Log("Finished behaviour of "+type);
        yield break;
    }
    IEnumerator GenerateChildren() {
        if(data.children_count <= 0)
            yield break;
        
        int current_orbit = 1;
        for (int i = 0; i < data.children_count; i++)
        {
            GameObject orbitator_object = new GameObject("Orbitator "+current_orbit);
            OrbitData orbitator = orbitator_object.AddComponent<OrbitData>();
            BodyBehaviour new_body = Instantiate(BodyController.instance.bodyPrefab, 
                                    transform.position,
                                    Quaternion.identity).GetComponent<BodyBehaviour>();
            BodyController.instance.SetupBody(new_body,type);
            new_body.gameObject.name = new_body.type;
            Vector3 orbit_rotation_vector = new Vector3(Random.Range(-0.3f,0.3f)*new_body.data.orbit_velocity,
            Random.Range(-1f,1f)*new_body.data.orbit_velocity,
            0);
            orbitator.rotation_vector = orbit_rotation_vector;
            orbitator.transform.position = transform.position;
            orbitator.following = transform;
            new_body.transform.position = transform.position;
            new_body.transform.parent = orbitator.transform;
            new_body.transform.position = transform.position + new Vector3(0,0,(new_body.data.orbit_radius*current_orbit)+2);
            print("Old pos "+orbitator.transform.localRotation);
            float rand1 = Random.Range(-360f,360f);
            float rand2 = Random.Range(-360f,360f);
            orbitator.transform.localRotation = Quaternion.Euler(orbit_rotation_vector.x*rand1*100,
            orbit_rotation_vector.y*rand2*100,
            0);
            print("New pos "+orbitator.transform.localRotation);
            current_orbit++;
        }
        yield return null;
        //print("Generated children");
        yield break;
    }
    
    IEnumerator GenerateContinents() {
        object_appearance.GetComponent<SpriteRenderer>().color = data.bottom_color;
        if(type == "Star")
            yield break;

        continentQuantity = Random.Range(-2-(int)data.initial_size,9+(int)data.initial_size);
        if(continentQuantity <= 0)
            yield break;
            
        continents = new Transform[continentQuantity];
        GameObject continent_prefab = BodyController.instance.continentPrefab;
        for (int i = 0; i < continentQuantity; i++)
        {
            Vector3 newContinentPosition = new Vector3(0,0,0);
            newContinentPosition.x = Random.Range(-maxX-2,maxX+2)*scale;
            newContinentPosition.y = Random.Range(-maxY-2,maxY+2)*scale;
            Transform newContinent = Instantiate(continent_prefab,
            transform.position, object_appearance.transform.rotation).transform;
            newContinent.parent = object_appearance.transform;
            newContinent.localPosition = newContinentPosition;            
            newContinent.GetComponent<SpriteRenderer>().color = data.upper_color;
            continents[i] = newContinent;
        }
        print("Generated continents");
        yield break;
    }
}
