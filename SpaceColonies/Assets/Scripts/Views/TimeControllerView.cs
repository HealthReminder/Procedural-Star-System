using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControllerView : MonoBehaviour
{
    public Text current_time_speed;
    public Slider time_speed_slider;
    public static TimeControllerView instance;
    private void Awake() {
        instance = this;
    }
    public void SetTimeSpeed(float new_speed){
        PerformanceManager.instance.simulation_speed = new_speed;
        time_speed_slider.value = new_speed; 
        current_time_speed.text = new_speed.ToString()+"x";
    }

}
