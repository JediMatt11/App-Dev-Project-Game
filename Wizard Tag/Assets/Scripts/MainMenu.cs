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

    private void Start()
    {
        StartCoroutine(SpawnWizards());
        Debug.Log(levelSelectMenu.transform.childCount);
        for (int i = 0; i < levelSelectMenu.transform.childCount; i++)
        {
            if (i < LevelManager.lastUnlockedLevel + 1)
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
