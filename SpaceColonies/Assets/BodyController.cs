using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour {
    public GameObject bodyPrefab;
    public GameObject continentPrefab;
    public Gradient colors_star;
    public Gradient colors_upper;
    public Gradient colors_bottom;

    public static BodyController instance;
    private void Awake() {
        instance = this;
    }
    public void SetupBody(BodyBehaviour new_body, string parent_type) {
        string child_type;
        if(parent_type == "Root")
            child_type = "Star";
        else if(parent_type == "Star")
            child_type = "Planet";
        else if (parent_type == "Planet")
            child_type = "Moon";
        else 
            child_type = "Satellite";

        new_body.type = child_type;
        BodyData data = new_body.data;

        if(child_type == "Star") {
            float random_color = Random.Range(0f,1f);
            data.bottom_color = colors_star.Evaluate(random_color);
            data.initial_size = (random_color+1)*2;
            data.children_count = Random.Range(3,12);
            //data.children_count = 1;
        } else if(child_type == "Planet") {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/2;
            data.orbit_velocity = 0.05f;
            data.orbit_radius = 100;
            data.children_count = Random.Range(-2,3);
            //data.children_count = 1;
        } else if(child_type == "Moon") {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/4;
            data.orbit_velocity = 1;
            data.orbit_radius = 20;
            data.children_count = Random.Range(-2,3);
            //data.children_count = 0;
        } else {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/8;
            data.orbit_velocity = 2;
            data.orbit_radius = 4;
            //data.children_count = Random.Range(-2,3);
            data.children_count = 0;
        }

        print("Finhes setting up"+child_type);
    }
}
