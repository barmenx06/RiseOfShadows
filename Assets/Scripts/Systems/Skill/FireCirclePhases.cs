using UnityEngine;

public class FireCirclePhases : MonoBehaviour
{
    public GameObject fireballVFX; // içteki sprite objesi
    public float interval = 5f;     // 5 saniyede bir kademe
    public int maxPhase = 5;

    private int currentPhase = 0;

    void Start()
    {
        fireballVFX.SetActive(false);
        InvokeRepeating(nameof(NextPhase), interval, interval);
    }

    void NextPhase()
    {
        currentPhase++;

        if (currentPhase < maxPhase)
        {
            // geçici olarak aç, scale büyüt
            fireballVFX.SetActive(true);
            float scale = 1f + currentPhase * 0.3f;
            transform.localScale = new Vector3(scale, scale, 1);
            Invoke(nameof(Hide), 1f); // 1 saniye sonra kapat
        }
        else if (currentPhase == maxPhase)
        {
            // 5. kademe: sonsuza kadar açýk
            float scale = 1f + currentPhase * 0.3f;
            transform.localScale = new Vector3(scale, scale, 1);
            fireballVFX.SetActive(true);
            CancelInvoke(); // artýk döngü durur
        }
    }

    void Hide()
    {
        if (currentPhase < maxPhase)
        {
            fireballVFX.SetActive(false);
        }
    }
}
