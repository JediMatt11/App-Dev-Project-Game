using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public float levelTimer;
    public float levelTime;

    public Text timerTXT;

    public List<GameObject> allWizards;

    public LevelManager levelManager;

    void Update()
    {
        if (levelManager == null)
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            levelManager.hasWon = false;
            levelManager.hasLost = false;
            return;
        }
        levelTimer -= Time.deltaTime;
        if (!levelManager.hasWon && !levelManager.hasLost)
        {
            timerTXT.text = "Avoid tags for: " + levelTimer.ToString("0");
            if (levelTimer <= 0f)
            {
                levelTimer = 0f;
                StartCoroutine(levelManager.Win());
            }
        }
    }
}
