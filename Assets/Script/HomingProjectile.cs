using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindClosestEnemy();
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.angularVelocity = 0f; // 더 이상 회전하지 않음
            return;
        }

        var dir = (target.position - transform.position).normalized;
        float rotationAmount = Vector3.Cross(dir, transform.right).z;
        rb.angularVelocity = -rotationAmount * rotationSpeed;
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDist)
            {
                closest = enemy.transform;
                minDist = dist;
            }
        }
        return closest;
    }
}
