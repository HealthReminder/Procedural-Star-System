using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitData : MonoBehaviour
{
    public Vector3 rotation_vector;
    public Transform following;
    private void Start() {
        PerformanceManager.instance.orbit_movement.Add(this);
    }
}
