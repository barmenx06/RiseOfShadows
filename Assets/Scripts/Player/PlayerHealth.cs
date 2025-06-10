using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    public GameObject healthBarRoot;
    public Image healthBarFill;
    public float barVisibleDuration = 1.2f;

    private Coroutine hideBarCoroutine;
    private Animator animator;

    private bool isDead = false; // <<< �nemli

    public GameObject deathEffectPrefab;

    public SpriteRenderer spriteRenderer; // Inspector�dan atayacaks�n!
    public GameObject damageEffectPrefab; // Inspector�dan ata!

    public GameOverController gameOverController;



    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarFill != null)
            healthBarFill.fillAmount = 1f;
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false);

        animator = GetComponent<Animator>();
    }
 

    public void TakeDamage(int damage)
    {
        // E�er oyuncu �lm��se tekrar hasar alma!
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();
        ShowHealthBar();

        // Hasar al�nca TakeHit animasyonunu tetikle
        if (animator != null)
            animator.SetTrigger("TakeHit");

        if (damageEffectPrefab != null)
        {
            GameObject effect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // Efekt 1 saniye sonra yok olsun

        }

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    void ShowHealthBar()
    {
        if (healthBarRoot == null)
            return;

        healthBarRoot.SetActive(true);

        if (hideBarCoroutine != null)
            StopCoroutine(hideBarCoroutine);
        hideBarCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }

    IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(barVisibleDuration);
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false);
    }

    void Die()
    {
        // Oyuncu �ld� olarak i�aretle
        isDead = true;

        // �l�nce Die animasyonunu tetikle
        if (animator != null)
            animator.SetTrigger("Die");

        if (gameOverController != null)
            gameOverController.ShowGameOver();


        Debug.Log("Player �ld�!");
    }

    public void Heal(int amount)
    {
        if (isDead) return; // �ld�yse can bas�lamas�n
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
        ShowHealthBar();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void OnDeathAnimationEnd()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Karakteri sahneden kald�r
    }

    IEnumerator FlashOnDamage()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = originalColor;
    }
}
