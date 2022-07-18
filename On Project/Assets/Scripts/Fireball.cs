using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float startSpeed;

    float currentSpeed;

    private void Start()
    {
        currentSpeed = startSpeed;
        Destroy(gameObject, 2);
    }
    private void Update()
    {
        currentSpeed += 0.02f;
        transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
    }
}
