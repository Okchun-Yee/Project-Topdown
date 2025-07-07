using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickups : MonoBehaviour
{
    private enum PickupType
    {
        GoldCoin,
        HealthGlobe,
        StaminaGlobe,
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        // 초기 위치에서 위로 튕겨오르는 애니메이션
        StartCoroutine(AnimCurveSpawnRoutine());
    }
    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }
    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPos = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);
        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linerT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linerT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPos, endPoint, linerT) + new Vector2(0f, height);
            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickupType)
        {
            case PickupType.GoldCoin:
                Debug.Log("coin picked up");
                break;
            case PickupType.HealthGlobe:
            PlayerHealth.Instance.HealPlayer();
                Debug.Log("health picked up");
                break;
            case PickupType.StaminaGlobe:
                Debug.Log("stamina picked up");
                break;
            default:
                Debug.LogWarning("Unknown pickup type");
                break;
        }
    }
}
