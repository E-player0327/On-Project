using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public float disableTime;
    float temp;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        temp = disableTime;
    }
    // Update is called once per frame
    void Update()
    {
        disableTime -= Time.deltaTime;
        spriteRenderer.color = new Color(1, 1, 1, disableTime / temp);
        if (disableTime < 0)
            Destroy(gameObject);
    }
}
