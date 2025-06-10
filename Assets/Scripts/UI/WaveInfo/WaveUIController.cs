using UnityEngine;
using TMPro;

public class WaveUIController : MonoBehaviour
{
    public TMP_Text waveInfoText;
    [Header("Countdown Thresholds")]
    public int firstThreshold = 30;
    public int secondThreshold = 20;
    public int finalThreshold = 10;

    private int prevSeconds = -1;

    /// <summary>
    /// Dalga metnini g�stermek veya gizlemek i�in �a�r�l�r.
    /// </summary>
    public void ShowWaveCountdown(string waveName, int secondsLeft)
    {
        if (secondsLeft == prevSeconds) return; // Ayn� saniyeyi tekrar g�stermemek i�in
        prevSeconds = secondsLeft; // Ge�erli saniyeyi g�ncelle

        if (waveInfoText == null)
            return;

        // E�ik de�erleri kontrol
        if (secondsLeft == firstThreshold || secondsLeft == secondThreshold)
        {
            waveInfoText.text = $"{waveName}, {secondsLeft} sn i�inde ba�layacak";
            waveInfoText.gameObject.SetActive(true);
        }
        else if (secondsLeft <= finalThreshold && secondsLeft > 0)
        {
            waveInfoText.text = $"{secondsLeft}";
            waveInfoText.gameObject.SetActive(true);
        }
        else // di�er durumlarda gizle
        {
            waveInfoText.gameObject.SetActive(false);
        }
    }

    public void HideWaveText()
    {
        if (waveInfoText == null)
            return;
        waveInfoText.gameObject.SetActive(false);
    }
}