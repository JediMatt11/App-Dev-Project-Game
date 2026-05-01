using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int lastUnlockedLevel;
    public int curLevelNum;

    public static LevelManager instance;
    public LevelProperties[] levelProperties;
    public Level curLevel;

    public Material daySkybox;
    public Material nightSkybox;

    public GameObject enemyPrefab;
    public GameObject settingsMenu;
    public Terrain terrain;

    public bool hasWon;
    public bool hasLost;

    public MainMenu mainMenu;

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
        lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel");
        mainMenu = GameObject.Find("MGMT").GetComponent<MainMenu>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        //lastUnlockedLevel = 0;
        //PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);
        //PlayerPrefs.Save();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        mainMenu = GameObject.Find("MGMT").GetComponent<MainMenu>();
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

    public void SelectLevelNumber(Button b)
    {
        if (mainMenu != null)
        {
            mainMenu.mainMenuSFX.PlayOneShot(mainMenu.levelClick);
        }
        int num = int.Parse(b.transform.GetChild(0).gameObject.GetComponent<Text>().text);
        StartCoroutine(LoadSampleSceneAsync("SampleScene", num));
    }
    private IEnumerator LoadSampleSceneAsync(string sceneName, int num)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        ApplyProperties(num);
    }

    public void ApplyProperties(int num)
    {
        curLevelNum = num;
        curLevel = GameObject.Find("Level").GetComponent<Level>();
        SetTimeOfDay(levelProperties[num - 1].isNight);
        //SetEnemyStats(levelProperties[num - 1].enemySpeedRun);
        SetGeneralLevel(levelProperties[num - 1].numEnemies, levelProperties[num - 1].numTimeOfLevelSeconds);
    }

    public void SetTimeOfDay(bool isNight)
    {
        RenderSettings.skybox = isNight ? nightSkybox : daySkybox;
        curLevel.directionalLight.intensity = isNight ? 0.35f : 3f;
    }

    public void SetGeneralLevel(int numEnemies, int seconds)
    {
        SpawnEnemies(numEnemies);
        curLevel.levelTime = seconds;
        curLevel.levelTimer = seconds;
        curLevel.timerTXT.text = "Avoid tags for: " + curLevel.levelTimer.ToString();
    }

    public void SpawnEnemies(int num)
    {
        curLevel.allWizards = new List<GameObject>();
        terrain = GameObject.Find("Ground").GetComponent<Terrain>();
        for (int i = 0; i < num; i++)
        {
            float randomX = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
            float randomZ = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);

            float terrainY = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ)) + terrain.transform.position.y;

            Vector3 spawnPosition = new Vector3(randomX, terrainY, randomZ);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 359f), 0f));
            enemy.GetComponent<NavMeshAgent>().speed = levelProperties[curLevelNum - 1].enemySpeedRun;
            curLevel.allWizards.Add(enemy);
        }
    }

    public IEnumerator Win()
    {
        hasWon = true;
        WonCurrentLevel(levelProperties[curLevelNum - 1]);
        curLevel.allWizards.ForEach(wiz => Destroy(wiz));
        curLevel.timerTXT.text = "Level Complete!";
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Lose()
    {
        StartCoroutine(LoseCO());
    }

    public IEnumerator LoseCO()
    {
        hasLost = true;
        curLevel.timerTXT.text = "You Lose!";
        curLevel.allWizards.ForEach(wiz => Destroy(wiz));
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

}
