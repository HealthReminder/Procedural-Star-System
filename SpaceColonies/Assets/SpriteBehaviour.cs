using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBehaviour : MonoBehaviour
{
    Transform target;
    public SpriteRenderer spriteRenderer;
    private void Awake() {
        target = Camera.main.transform;
    }
    private void Start() {
        StartCoroutine(SortSpriteRoutine());
    }
    IEnumerator SortSpriteRoutine() 
    {
        float randomStart = Random.Range(0f,1f)/100;
        while(true) {
            transform.LookAt(target.position, -Vector3.up);
            spriteRenderer.sortingOrder = (int)Vector3.Distance(transform.position,target.position)*-1;
            yield return new WaitForSeconds(0.05f+randomStart);
        }
    }
}
