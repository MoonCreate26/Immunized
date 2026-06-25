using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI
    public TMP_Text resourceCount;
    public TMP_Text waveText;
    public Animator GameOverAnimator;
    public PanZoom panZoom;
    public GameObject pauseCanvas;
    public Slider slider;
    InfographicUI infographicUI;
    [SerializeField] GameObject adaptiveCellButton;
    [SerializeField] SwitchTargetSprite targetDisplay;
    [SerializeField] ChangeInstruction changeInstruction;
    
    // Resource Generation
    public int resources = 30;
    public int resourcesCapacity = 90;
    public float generationInterval = 2f;
    public float cellGenerationValue = 0.1f;
    public float minGenerationInterval = 0.5f;
    public int currentWave;

    // Pathogen & Spawn Mechanic
    Spawner[] spawnerList;
    public int maxUnlockedIndex = 0;
    public float difficultyMultiplier = 1;
    float elapsedTime = 0f;
    bool pathogenEliminated = false;
    bool spawnerEmptied = false;
    int spawnerAmount;

    [SerializeField] public UniversalPathogenDictionary pathogenDictionary;
    [SerializeField] float[] unlockTime;
    [SerializeField] public GameObject[] pathogenPrefabs;

    // Adaptive Immune System
    public int adaptiveImmuneTarget = -1;
    [SerializeField] GameObject adaptiveCellList;
    bool currentlyShowingAdaptive = false;

    // Game State
    public bool GameOver = false;
    public bool Paused = false;

    // Cells
    float cellAmount;
    float killedCellAmount = 1f;
    [SerializeField] float acceptableKilledCellAmount;
    
    // Initialize UI and Spawners
    // Handle cell count
    void Start()
    {
        slider.maxValue = acceptableKilledCellAmount;

        spawnerList = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
        spawnerAmount = spawnerList.Length;

        infographicUI = pauseCanvas.GetComponent<InfographicUI>();

        GameOver = false;

        setNewInstruction("Eliminate All Pathogens by Placing Immune Cells");

        foreach(GameObject gameObject in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if(LayerMask.LayerToName(gameObject.layer) == "Cells")
            {
                cellAmount++;
            }
        }

        StartCoroutine(resourceGenerator());
    }

    // Track time elapsed & Game State
    // Check resource capacity
    void Update()
    {
        elapsedTime += Time.deltaTime;

        resourceCount.text = resources.ToString();
        waveText.text = "Wave " + (currentWave + 1).ToString();

        if(pathogenEliminated && spawnerEmptied)
        {
            newWave();

            setNewInstruction("Wave Eliminated!");
        }

        if(resources > resourcesCapacity)
        {
            resources = resourcesCapacity;
        }

        if(killedCellAmount > acceptableKilledCellAmount)
        {
            GameOverAnimator.gameObject.SetActive(true);
            GameOver = true;
            panZoom.enabled = false;
        }
    }

    void newWave()
    {
            currentWave++;
            pathogenEliminated = false;
            spawnerEmptied = false;

            spawnerAmount = spawnerList.Length;

            foreach(Spawner spawner in spawnerList)
            {
                spawner.refreshSpawner();
            }
    }

    public void CheckPathogenEliminated()
    {
        if(FindObjectsByType<PathogenicBehavior>(FindObjectsSortMode.None).Length <= 0)
        {
            pathogenEliminated = true;
        }

        else
        {
            pathogenEliminated = false;
        }
    }

    public int CheckPathogenUnlocked()
    {
        if(elapsedTime > unlockTime[maxUnlockedIndex + 1])
        {
            maxUnlockedIndex++;
            return CheckPathogenUnlocked();
        }

        return maxUnlockedIndex;
    }

    public void CheckSpawnerEmptied()
    {
        spawnerAmount--;

        if(spawnerAmount <= 0)
        {
            spawnerEmptied = true;
        }
    }

    IEnumerator resourceGenerator()
    {
        yield return new WaitForSeconds(generationInterval);

        changeResource(1);

        StartCoroutine(resourceGenerator());
    }

    public void changeResource(int change)
    {
        if(resources >= resourcesCapacity && change > 0)
        {
            return;
        }

        resources += change;
    }

    public void cellKilled()
    {
        slider.value --;

        killedCellAmount++;
        generationInterval += cellGenerationValue;

        Debug.Log("Cells Alive: " + (100 - (killedCellAmount / cellAmount) * 100) + "%");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseCanvas.SetActive(true);

        Paused = true;
    }

    public void Unpause()
    {
        if(infographicUI.infoCurrentlyShown)
        {
            infographicUI.StartCoroutine("StartCountDown");
            return;
        }

        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);

        infographicUI.disableAllInfographic();

        Paused = false;
    }

    public void SwitchCellList()
    {
        currentlyShowingAdaptive = !currentlyShowingAdaptive;

        adaptiveCellList.SetActive(currentlyShowingAdaptive);
    }

    public void showAdaptiveCellButton()
    {
        adaptiveCellButton.SetActive(true);

        StartCoroutine(updateTargetSprite());
    }

    IEnumerator updateTargetSprite()
    {
        yield return new WaitUntil(() => currentlyShowingAdaptive);

        targetDisplay.UpdateSprite(adaptiveImmuneTarget);
    }

    public void setNewInstruction(string Message)
    {
        changeInstruction.change(Message);
    }
}
