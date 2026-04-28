using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject waterWizard;
    public GameObject airWizard;
    public bool shouldWaterWizard = true;
    public Vector3 wizardMenuSpawnPos;
    public float wizardSpawnTimer;

    public GameObject settingsMenu;
    public GameObject levelSelectMenu;
    public GameObject mainMenu;

    public Button[] levelButtons;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];
            button.onClick.AddListener(() => LevelManager.instance.SelectLevelNumber(button));
        }

        StartCoroutine(SpawnWizards());
        for (int i = 0; i < levelSelectMenu.transform.childCount; i++)
        {
            if (i < LevelManager.lastUnlockedLevel+1)
            {
                levelSelectMenu.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
                levelSelectMenu.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                levelSelectMenu.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.gray;
                levelSelectMenu.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public IEnumerator SpawnWizards()
    {
        while (true)
        {
            yield return new WaitForSeconds(wizardSpawnTimer);
            GameObject wizardPrefab = shouldWaterWizard ? waterWizard : airWizard;
            shouldWaterWizard = !shouldWaterWizard;
            if (wizardPrefab != null)
            {
                
                GameObject spawnedWiz = Instantiate(wizardPrefab, wizardMenuSpawnPos, Quaternion.Euler(0f, 245f, 0f));
                spawnedWiz.AddComponent<MenuSpawnWizard>();
                spawnedWiz.GetComponent<MenuSpawnWizard>().Destroyable(wizardSpawnTimer);
            }
        }
    }

    public void SelectLevel()
    {
        levelSelectMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseLevelSelect()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }


}
