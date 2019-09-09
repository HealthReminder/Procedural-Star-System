using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBehaviour : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private void Start() {
        PerformanceManager.instance.renderer_order.Add(spriteRenderer);
        Destroy(this);
    }
}
