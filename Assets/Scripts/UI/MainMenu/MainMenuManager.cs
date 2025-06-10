using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public MapProfile[] availableMaps;

    public void OnPlayButtonClicked()
    {
        int index = Random.Range(0, availableMaps.Length);
        MapManager.Instance.currentMap = availableMaps[index];

        // Oyun sahnesinin ad�n� yaz: �rn. "GameScene"
        SceneManager.LoadScene("MainScene");
    }
}
