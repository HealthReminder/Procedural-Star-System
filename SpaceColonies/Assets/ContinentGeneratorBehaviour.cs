using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentGeneratorBehaviour : MonoBehaviour
{
    float coast_roughness = 0.2f;
    float initial_size = 0.2f;
    private void Awake() {
        StartCoroutine(GenerateContinent());
    }
    IEnumerator GenerateContinent() {
        while(transform.parent == null)
            yield return null;

        float parent_scale = transform.parent.localScale.x;

        coast_roughness = Random.Range(0.05f,0.3f);
        initial_size = Random.Range(0.1f*parent_scale,0.4f*parent_scale)*parent_scale;
        

        Transform root = transform.GetChild(0);
        int children_count = root.childCount;

        //Set initial size
        transform.localScale = new Vector3(initial_size,initial_size,initial_size);
        //Get bones
        Transform[] children = new Transform[children_count];
        for (int i = 0; i < children_count; i++)
            children[i] = root.GetChild(i);
        
        //Set perlin noise values
        float[] perlinValues = new float[children.Length];
        float incrementor = Random.Range(0f,1f);
        for (int o = 0; o < children_count; o++)
        {
            perlinValues[o] = Mathf.PerlinNoise(incrementor,0);
            incrementor+= coast_roughness+0.05f;
            //print(incrementor);
        }

        //Move bones
        for (int u = 0; u < children.Length; u++)
        {
            Transform this_child = children[u].transform;
            this_child.position +=  -this_child.right + this_child.right*perlinValues[u]*2*parent_scale;
        }

        //Set collider
        PolygonCollider2D coll = transform.GetComponent<PolygonCollider2D>();
        Vector2[] new_points = new Vector2[children_count];
        int offset = 0;
        int index = offset;
        for (int w = 0; w < children_count; w++){
            index += 1;
            if(index >= children.Length)
                index = 0;
            new_points[index] = children[index].localPosition;
        }
        coll.points = new_points;

        yield break;
    }
}
