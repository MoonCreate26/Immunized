using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text resourceCount;
    public TMP_Text waveText;
    public Animator GameOverAnimator;
    public PanZoom panZoom;
    public GameObject pauseCanvas;
    public Slider slider;

    public int resources = 30;
    public int resourcesCapacity = 90;
    public float generationInterval = 2f;
    public float cellGenerationValue = 0.1f;
    public float minGenerationInterval = 0.5f;
    public int currentWave;
    public string adaptiveImmuneTarget;
    public bool GameOver = false;
    public bool Paused = false;

    bool pathogenEliminated = false;
    bool spawnerEmptied = false;
    int spawnerAmount;
    Spawner[] spawnerList;
    float cellAmount;
    float killedCellAmount = 1f;
    InfographicUI infographicUI;
    
    [SerializeField] float acceptableKilledCellAmount;
    [SerializeField] GameObject adaptiveCellList;
    [SerializeField] GameObject adaptiveCellButton;
    [SerializeField] SwitchTargetSprite targetDisplay;
    [SerializeField] ChangeInstruction changeInstruction;

    bool currentlyShowingAdaptive = false;

    
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

    void Update()
    {
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
