using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tek bir dalgaya (wave) ait d��man tipleri listesini ve dalga ad�n� tutar.
/// </summary>
[System.Serializable]
public class EnemyWave
{
    [Tooltip("Dalga ad�, UI'da g�sterilir")]
    public string waveName;

    [Tooltip("Bu dalgadaki t�m d��man tipi bilgileri")]
    public List<EnemySpawnInfo> enemiesInWave;
}
