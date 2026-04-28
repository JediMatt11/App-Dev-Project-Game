using UnityEngine;

[CreateAssetMenu(fileName = "LevelProperties", menuName = "Scriptable Objects/LevelProperties")]
public class LevelProperties : ScriptableObject
{
    public int levelNumber;

    public int numEnemies;
    public int numTimeOfLevelSeconds;
    public int LOSDistMult;
    public float enemySpeedRun;

    public bool isNight;
}
