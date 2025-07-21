using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Sprite fullHealthIcon, emptyHealthIcon;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private Transform healthIcon;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string HEALTH_ICON_TEXT = "Heart Container";
    const string TOWN_TEXT = "Scene_01";
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    protected override void Awake()
    {
        base.Awake();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }
    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider(); UpdateHealthIcon();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthSlider(); UpdateHealthIcon();
        }
    }
    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        currentHealth -= damageAmount;
        DamageRecoveryTime();

        UpdateHealthSlider(); UpdateHealthIcon();
        CheckIfPlayerDeath();
    }
    public void DamageRecoveryTime()
    {
        canTakeDamage = false;
        StartCoroutine(DamageRecoveryRoutine());
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);

            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }
    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("[PlayerHealth] Player has died. Loading town scene.");
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void UpdateHealthIcon()
    {
        for(int i = 0; i < maxHealth; i++)
        {
            healthIcon = GameObject.Find(HEALTH_ICON_TEXT).transform.GetChild(i);
            if (i < currentHealth)
            {
                healthIcon.GetComponent<Image>().sprite = fullHealthIcon;
            }
            else
            {
                healthIcon.GetComponent<Image>().sprite = emptyHealthIcon;
            }
        }
    }
}
