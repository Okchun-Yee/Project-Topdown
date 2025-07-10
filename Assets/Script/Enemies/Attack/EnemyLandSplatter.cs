using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLandSplatter : MonoBehaviour
{
    private SpriteFade spriteFader;
    private void Awake()
    {
        spriteFader = GetComponent<SpriteFade>();
    }
    private void Start()
    {
        StartCoroutine(spriteFader.SlowFadeRoutine());
        Invoke("DisableCollider", 0.2f);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }
    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
