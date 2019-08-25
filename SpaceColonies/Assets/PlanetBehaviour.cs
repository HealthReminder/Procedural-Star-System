using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    public float maxY = 5;
    public float maxX = 15;
    public int continentQuantity = 0;
    public GameObject continentPrefab;
    Transform[] continents;
    public Color bottom_color;
    public Color upper_color;
    float scale;
    private void Awake() {
        scale = transform.lossyScale.x;
        GenerateContinents();
    }
    private void Update() {
        FakePlanetRotation();
    }
    private void GenerateContinents() {
        //maxY = Random.Range(1,5);
        //maxX = Random.Range(1,15);
        //Do everything proportional to this objects scale
        continentQuantity = Random.Range(-2,15);
        if(continentQuantity <= 0)
            return;
        continents = new Transform[continentQuantity];
        for (int i = 0; i < continentQuantity; i++)
        {
            Vector3 newContinentPosition = new Vector3(0,0,0);
            newContinentPosition.x = Random.Range(-maxX,maxX)*scale;
            newContinentPosition.y = Random.Range(-maxY,maxY)*scale;
            Transform newContinent = Instantiate(continentPrefab, newContinentPosition+transform.position, Quaternion.identity).transform;
            newContinent.parent = transform;
            newContinent.GetComponent<SpriteRenderer>().color = upper_color;
            continents[i] = newContinent;
        }
        
        //Change colour
        GetComponent<SpriteRenderer>().color = bottom_color;
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
            movingNow.localPosition+= new Vector3(0.1f,0,0);
            if(movingNow.localPosition.x > maxX)
                movingNow.localPosition = new Vector3(-(maxX*scale*2),movingNow.localPosition.y,movingNow.localPosition.z);
        }
    }
}
