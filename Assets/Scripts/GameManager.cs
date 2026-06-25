using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour
{
    // UI
    public TMP_Text resourceCount;
    public TMP_Text timerText;
    public TMP_Text timerTextFinal;
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

    // Pathogen & Spawn Mechanic
    [SerializeField] int spawnCapacity = 10;
    int baseCapacity;
    public int spawnCount = 0; 
    public bool spawnEnabled = true;
    public int maxUnlockedIndex = 0;
    public float difficultyMultiplier = 1;
    float elapsedTime = 0f;
    [HideInInspector] public int[] pathogenCount;

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

        pathogenCount = new int[pathogenDictionary.GetLength()];
        baseCapacity = spawnCapacity;

        StartCoroutine(resourceGenerator());
    }

    // Track time elapsed & Game State
    // Check resource capacity
    void Update()
    {
        // Update time
        elapsedTime += Time.deltaTime;
        difficultyMultiplier = 1 + elapsedTime * 0.005f;

        // Update timer
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = time.ToString(@"mm\:ss\.ff");

        // Update resources
        resourceCount.text = resources.ToString();

        if(resources > resourcesCapacity)
        {
            resources = resourcesCapacity;
        }

        // Check if GameOver condition has been met
        if(killedCellAmount > acceptableKilledCellAmount)
        {
            GameOverAnimator.gameObject.SetActive(true);
            GameOver = true;
            panZoom.enabled = false;
            timerTextFinal.text = timerText.text;
        }

        // Update spawn check
        spawnCapacity = (int)(baseCapacity * Math.Pow(difficultyMultiplier, 2));
        spawnEnabled = spawnCount < spawnCapacity;
    }

    public int CheckPathogenUnlocked()
    {
        if(elapsedTime > unlockTime[maxUnlockedIndex + 1])
        {
            maxUnlockedIndex++;
            setNewInstruction("More powerful pathogens will invade as time passes...");
            Debug.Log("Index " + maxUnlockedIndex + " unlocked at " + elapsedTime + "!");
            return CheckPathogenUnlocked();
        }

        return maxUnlockedIndex;
    }

    // Generate resource
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
