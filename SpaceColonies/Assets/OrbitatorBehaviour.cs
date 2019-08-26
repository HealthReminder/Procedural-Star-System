using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitatorBehaviour : MonoBehaviour
{
    public Vector3 rotation_vector;
    public Transform following;
    private void Update() {
        transform.Rotate(rotation_vector);
        transform.position = following.position;
    }
}
