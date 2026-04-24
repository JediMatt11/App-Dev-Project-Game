using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int lastUnlockedLevel;

    public static LevelManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 0);
    }

    public void WonCurrentLevel(LevelProperties lp)
    {
        if (lastUnlockedLevel < lp.levelNumber)
        {
            lastUnlockedLevel = lp.levelNumber;
            PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);
            PlayerPrefs.Save();
        }
    }
}
