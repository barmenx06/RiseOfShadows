using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GameObject settingsRoot;

    public void showSettings()
    {
        Time.timeScale = 0;
        if (settingsRoot != null )
            settingsRoot.SetActive(true);
    }
}
