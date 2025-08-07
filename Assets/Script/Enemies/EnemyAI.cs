using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float trackingRange = 8f; // 추적 시작 범위
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking,
        Tracking
    }
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfanding enemyPathfanding;

    private void Awake()
    {
        enemyPathfanding = GetComponent<EnemyPathfanding>();
        state = State.Roaming;
    }
    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }
    private void Update()
    {
        MovementStateControl();
    }
    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Tracking:
                Tracking();
                break;
        }
    }
    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathfanding.MoveTo(roamPosition);
        // 플레이어가 추적 범위 안에 들어오면 추적 상태로 전환
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < trackingRange)
        {
            state = State.Tracking;
        }
        // 일정 시간마다 새로운 위치로 이동
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }
    private void Attacking()
    {
        // 플레이어가 공격 범위 밖으로 나가면 추적 상태로 전환
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Tracking;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfanding.StopMoving();
            }
            else
            {
                enemyPathfanding.MoveTo(roamPosition);
            }
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private void Tracking()
    {
        // 플레이어가 추적 범위 밖으로 나가면 다시 Roaming
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > trackingRange)
        {
            state = State.Roaming;
            return;
        }
        // 플레이어가 공격 범위 안에 들어오면 공격
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
            return;
        }
        // 플레이어를 따라 이동
        enemyPathfanding.MoveTo(PlayerController.Instance.transform.position);
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        // Generate a random direction and normalize it to get a unit vector
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
