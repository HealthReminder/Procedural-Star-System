using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorBehaviour : MonoBehaviour
{
    public Vector3 rotation_vector;
    private void Update() {
        transform.Rotate(rotation_vector);
    }
}
