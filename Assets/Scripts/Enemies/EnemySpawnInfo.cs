using UnityEngine;

/// <summary>
/// Bir dalgadaki tek bir d��man tipi i�in spawn ayarlar�n� tutar.
/// </summary>
[System.Serializable]
public class EnemySpawnInfo
{
    [Tooltip("Spawn edilecek d��man prefab'�")]
    public GameObject enemyPrefab;

    [Tooltip("Bu prefab'tan ka� tane spawn edilecek")]
    public int amount;
}
