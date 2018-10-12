using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public enum GAMEMODES { NONE, BUILD, DISTRIBUTION };

    public GameObject buildBar;
    public GameObject distributionBar;
    public GameObject gathererUI;
    public GameObject gathererTitle;
    public GameObject gathererResourceContent;
    public GameObject addResourceButton;
    public GameObject addStorageButton;

    private bool showBuild;
    private bool showDistribution;
    private bool buildBarEnabled;
    private bool distributionBarEnabled;
    private BuildingManager manager;
    public bool mouseOverUI;


    public GathererUI currentGatherer;

    public GAMEMODES currentMode;
	
	void Start ()
    {
        currentMode = GAMEMODES.NONE;
        buildBarEnabled = buildBar.activeSelf;
        distributionBarEnabled = distributionBar.activeSelf;
        manager = FindObjectOfType<BuildingManager>();
        mouseOverUI = false;
	}
	
	public void ActivateGathererResourceAdder()
    {
        currentGatherer.SetSelectResource(true);
        Debug.Log("Activating select resource");
    }

    public void ActivateGathererStorageAdder()
    {
        currentGatherer.SetSelectStorage(true);
    }

    void Update ()
    {
        if (currentMode == GAMEMODES.BUILD)
            UpdateBuild();
        if (currentMode == GAMEMODES.DISTRIBUTION)
            UpdateDistribution();
        if(currentMode == GAMEMODES.NONE)
        {
            buildBarEnabled = false;
            distributionBarEnabled = false;
        }
        if (buildBarEnabled)
            buildBar.SetActive(true);
        else
            buildBar.SetActive(false);
        if (distributionBarEnabled)
            distributionBar.SetActive(true);
        else
            distributionBar.SetActive(false);
	}


    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverUI = false;
    }

    void UpdateBuild()
    {
        if(showBuild && !buildBarEnabled)
        {
            distributionBarEnabled = false;
            buildBarEnabled = true;
        }
    }

    void UpdateDistribution()
    {
        if (showDistribution && !distributionBarEnabled)
        {
            buildBarEnabled = false;
            distributionBarEnabled = true;
        }
    }

    public void SetGameMode(int mode)
    {
        switch (mode)
        {
            case (0):
                currentMode = GAMEMODES.NONE;
                showDistribution = false;
                showBuild = false;
                break;
            case (1):
                if (currentMode == GAMEMODES.BUILD)
                {
                    currentMode = GAMEMODES.NONE;
                    showBuild = false;
                }
                else
                {
                    currentMode = GAMEMODES.BUILD;
                    showBuild = true;
                    showDistribution = false;
                }
                break;
            case (2):
                if (currentMode == GAMEMODES.DISTRIBUTION)
                {
                    currentMode = GAMEMODES.NONE;
                    showBuild = false;
                }
                else
                {
                    currentMode = GAMEMODES.DISTRIBUTION;
                    showDistribution = true;
                    showBuild = false;
                }
                break;
            default:
                Debug.LogError("Didn't recieve handled mode setting back to none");
                currentMode = GAMEMODES.NONE;
                break;
        }
    }

    public void SetBuildType(string name)
    {
        manager.SetCurrentBuilding(name); 
    }

   
}
