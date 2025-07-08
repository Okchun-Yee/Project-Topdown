using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject EliteEnemyPeojectilePrefab;

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
        Instantiate(EliteEnemyPeojectilePrefab, transform.position, Quaternion.identity);
    }
}
