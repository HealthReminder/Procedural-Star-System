using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShowoff : MonoBehaviour
{
    public Vector3 vector;
    private void Update() {
        transform.Rotate(vector);
    }
}
