using UnityEngine;

public class SpriteBehaviour : MonoBehaviour
{
    Transform target;
    public SpriteRenderer spriteRenderer;
    private void Awake() {
        target = Camera.main.transform;
    }
    void Update() 
    {
        transform.LookAt(target.position, -Vector3.up);
        spriteRenderer.sortingOrder = (int)Vector3.Distance(transform.position,target.position)*-1;
    }
}
