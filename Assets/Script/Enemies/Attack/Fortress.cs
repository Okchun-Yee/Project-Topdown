using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject fortressPeojectilePrefab;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Attack()
    {
        anim.SetTrigger(ATTACK_HASH);
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    public void SpawnProjectileAnimEvent()
    {
        Instantiate(fortressPeojectilePrefab, transform.position, Quaternion.identity);
    }
}
