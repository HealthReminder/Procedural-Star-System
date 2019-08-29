using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControllerView : MonoBehaviour
{
    public Text current_time_speed;
    public Slider time_speed_slider;
    public void SetTimeSpeed(float new_speed){
        PerformanceManager.instance.simulation_speed = new_speed;
        time_speed_slider.value = new_speed; 
        current_time_speed.text = new_speed.ToString()+"x";
    }
    public void UpdateTimeSpeed() {
        PerformanceManager.instance.simulation_speed = time_speed_slider.value;
        current_time_speed.text = time_speed_slider.value.ToString()+"x";
    }

}
