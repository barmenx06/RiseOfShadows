using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public delegate void EnemyDieHandler();
    public event EnemyDieHandler OnEnemyDie;

    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarRoot; // Can bar�n� kapsayan UI objesi
    public Image healthBarFill;

    private bool isDead = false;
    private Coroutine hideCoroutine;
    public float barShowDuration = 1.5f;

    [Header("EXP Drop Ayarlar�")]
    public GameObject expDropPrefab;      // Inspector�dan ata (mor y�ld�z prefab��)
    public int expValue = 10;             // Verece�i exp miktar�

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false); // Ba�lang��ta gizli
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(true);
            if (hideCoroutine != null)
                StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(HideBarAfterDelay());
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    IEnumerator HideBarAfterDelay()
    {
        yield return new WaitForSeconds(barShowDuration);
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // EXP Drop Spawnla
        if (expDropPrefab != null)
        {
            GameObject expObj = Instantiate(expDropPrefab, transform.position, Quaternion.identity);

            // E�er expAmount ExpDrop scriptinde varsa, burada de�eri g�ncelle:
            ExpDrop expDrop = expObj.GetComponent<ExpDrop>();
            if (expDrop != null)
                expDrop.expAmount = expValue;
        }

        // �l�m i�lemleri
        if (OnEnemyDie != null)
            OnEnemyDie.Invoke();

        Destroy(gameObject, 0.5f);
    }
}
