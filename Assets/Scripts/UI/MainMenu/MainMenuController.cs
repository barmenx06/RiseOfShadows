using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Play tu�una atand���nda sahneyi y�kleyen fonksiyon
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }
}
