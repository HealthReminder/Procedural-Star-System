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
}
public class BodyBehaviour : MonoBehaviour
{
    public string type;
    [SerializeField] public BodyData data;
    public GameObject object_appearance;
    public Transform orbiting;
    float maxY = 5;
    float maxX = 15;
    int continentQuantity = 0;
    Transform[] continents;
    float scale;
    private void Start() {
        StartCoroutine(Behaviour());
        
    }
    IEnumerator Behaviour() {
        if(type == "Star")
            BodyController.instance.SetupBody(this,"Root");
        yield return null;
        print("Starting generation for "+type);
        transform.localScale = Vector3.one*data.initial_size;
        scale = transform.lossyScale.x;
        yield return GenerateContinents();
        yield return GenerateChildren();
        Debug.Log("Finished behaviour of "+type);
        yield break;
    }
    IEnumerator GenerateChildren() {
        if(data.children_count <= 0)
            yield break;
        
        int current_orbit = 1;
        for (int i = 0; i < data.children_count; i++)
        {
            GameObject orbitator_object = new GameObject("Orbitator "+current_orbit);
            OrbitatorBehaviour orbitator = orbitator_object.AddComponent<OrbitatorBehaviour>();
            BodyBehaviour new_body = Instantiate(BodyController.instance.bodyPrefab, 
                                    transform.position,
                                    Quaternion.identity).GetComponent<BodyBehaviour>();
            BodyController.instance.SetupBody(new_body,type);
            new_body.gameObject.name = new_body.type;
            orbitator.rotation_vector = new Vector3(0,Random.Range(0.1f,1f)*new_body.data.orbit_velocity,0);
            orbitator.transform.position = transform.position;
            orbitator.following = transform;
            new_body.transform.position = transform.position;
            new_body.transform.parent = orbitator.transform;
            orbitator.transform.localRotation = Quaternion.Euler(0,Random.Range(0,360f),0);
            print("ROTATION = "+orbitator.transform.rotation.y);
            new_body.transform.position = transform.position + new Vector3(0,0,new_body.data.orbit_radius*current_orbit);


            current_orbit++;
        }
        yield return null;
        print("Generated children");
        yield break;
    }
    
    IEnumerator GenerateContinents() {
        object_appearance.GetComponent<SpriteRenderer>().color = data.bottom_color;
        if(type == "Star")
            yield break;

        continentQuantity = Random.Range(-2,15);
        if(continentQuantity <= 0)
            yield break;
            
        continents = new Transform[continentQuantity];
        GameObject continent_prefab = BodyController.instance.continentPrefab;
        for (int i = 0; i < continentQuantity; i++)
        {
            Vector3 newContinentPosition = new Vector3(0,0,0);
            newContinentPosition.x = Random.Range(-maxX,maxX)*scale;
            newContinentPosition.y = Random.Range(-maxY,maxY)*scale;
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
    private void Update() {
        FakePlanetRotation();
    }
    void FakePlanetRotation() {
        if(continents == null)
            return;
        if(continents.Length <= 0)
            return;
        //This is the movement of the continents
        for (int c = 0; c < continents.Length; c++)
        {
            Transform movingNow = continents[c];
            if(movingNow == null)
                return;
            movingNow.localPosition+= new Vector3(0.1f,0,0);
            if(movingNow.localPosition.x > maxX)
                movingNow.localPosition = new Vector3(-(maxX*scale*2),movingNow.localPosition.y,movingNow.localPosition.z);
        }
    }
}
