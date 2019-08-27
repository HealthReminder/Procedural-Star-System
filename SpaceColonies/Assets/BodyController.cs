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
            data.initial_size = Mathf.Pow(((random_color*random_color)*2)+1,2);
            data.children_count = Random.Range(3,5);
            //data.children_count = 1;
        } else if(child_type == "Planet") {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/1;
            data.orbit_velocity = 0.01f;
            data.orbit_radius = 250;
            data.trail_duration = 60;

            data.children_count = Random.Range(-0,5);
            //data.children_count = 5;
        } else if(child_type == "Moon") {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/4;
            data.orbit_velocity = 0.1f;
            data.orbit_radius = 23;
            data.trail_duration = 10;

            data.children_count = Random.Range(-1,3);
            //data.children_count = 3;
        } else {
            float random_bottom_color = Random.Range(0f,1f);
            data.bottom_color = colors_bottom.Evaluate(random_bottom_color);
            float random_upper_color = Random.Range(0f,1f);
            data.upper_color = colors_upper.Evaluate(random_upper_color);
            float random_size = Random.Range(0f,1f);
            data.initial_size = (random_size+1)/12;
            data.orbit_velocity = 0.5f;
            data.orbit_radius = 3f;
            data.trail_duration = 2;

            //data.children_count = Random.Range(-2,3);
            data.children_count = 0;
        }

        print("Finhes setting up"+child_type);
    }
}
