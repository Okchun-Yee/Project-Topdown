using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockback { get; private set; }
    [SerializeField] private float knockBackTime = .2f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        GettingKnockback = true;
        Vector2 difference =
        (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        GettingKnockback = false;
    }
}
