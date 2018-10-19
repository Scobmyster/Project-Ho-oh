using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public enum GAMEMODES { NONE, BUILD, DISTRIBUTION, DEMOLISH };

    public GameObject buildBar;
    public GameObject distributionBar;
    public GameObject resourceBar;
    public GameObject extractorBar;
    public GameObject gathererUI;
    public GameObject gathererTitle;
    public GameObject gathererContent;
    public GameObject addExtractorButton;
    public GameObject addStorageButton;
    public GameObject toggleExtractor;
    public GameObject toggleStorage;

    private bool showBuild;
    private bool showDistribution;
    private bool buildBarEnabled;
    private bool distributionBarEnabled;
    public bool mouseOverUI;

    private BuildingManager builder;
    private GameManager game;
    public GathererUI currentGatherer;
    private BuildNode lastNodeOver;

    private Dictionary<string, GameObject> displayBarMap;

    public GAMEMODES currentMode;

    private Vector3 lastMousePosition;

	
	void Start ()
    {
        currentMode = GAMEMODES.NONE;
        buildBarEnabled = buildBar.activeSelf;
        distributionBarEnabled = distributionBar.activeSelf;
        builder = FindObjectOfType<BuildingManager>();
        game = FindObjectOfType<GameManager>();
        mouseOverUI = false;
        displayBarMap = new Dictionary<string, GameObject>();
        SetupDisplayBarMap();
        lastMousePosition = new Vector3();
	}

    private void SetupDisplayBarMap()
    {
        displayBarMap.Add("build", buildBar);
        displayBarMap.Add("distribution", distributionBar);
        displayBarMap.Add("resource", resourceBar);
        displayBarMap.Add("extractor", extractorBar);
    }
	
	public void ActivateGathererExtractorAdder()
    {
        currentGatherer.SetSelectExtractor(true);
    }

    public void ActivateGathererStorageAdder()
    {
        currentGatherer.SetSelectStorage(true);
    }

    void Update ()
    {
        if(currentMode == GAMEMODES.DEMOLISH && Input.GetMouseButtonDown(0))
        {
            BuildNode node = RayCastUtilities.RayCastComponentOfBuildNodeInParent();
            node.Demolish();
        }
        if(!mouseOverUI)
        {
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(lastNodeOver != null)
                lastNodeOver.SetNodeSelected(false);
            BuildNode node = RayCastUtilities.RayCastComponentOfBuildNodeInParent();
            if(node != null)
            {
                GameObject go = node.gameObject;
                builder.SetBuildPreview(go.transform.position);
                node.SetNodeSelected(true);
                lastNodeOver = node;
            }
        }
	}


    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverUI = false;
    }

    
    public void SetGameMode(int mode)
    {
        switch (mode)
        {
            case (0):
                currentMode = GAMEMODES.NONE;
                SwitchOffDisplayBars();
                break;
            case (1):
                if (currentMode == GAMEMODES.BUILD)
                {
                    currentMode = GAMEMODES.NONE;
                    SwitchOffDisplayBars();
                }
                else
                {
                    currentMode = GAMEMODES.BUILD;
                    SwitchDisplayedBar("build");
                }
                break;
            case (2):
                if (currentMode == GAMEMODES.DISTRIBUTION)
                {
                    currentMode = GAMEMODES.NONE;
                    SwitchOffDisplayBars();
                }
                else
                {
                    currentMode = GAMEMODES.DISTRIBUTION;
                    SwitchDisplayedBar("distribution");
                }
                break;
            case (3):
                {
                    if (currentMode == GAMEMODES.DEMOLISH)
                    {
                        currentMode = GAMEMODES.NONE;
                    }
                    else
                    {
                        currentMode = GAMEMODES.DEMOLISH;
                        SwitchOffDisplayBars();
                    }
                }
                break;
            default:
                Debug.LogError("Didn't recieve handled mode setting back to none");
                currentMode = GAMEMODES.NONE;
                break;
        }
    }

    public void ToggleResources()
    {
        currentGatherer.SetResourceOnly(toggleExtractor.GetComponent<Toggle>().isOn);
    }

    public void ToggleStorage()
    {
        currentGatherer.SetStorageOnly(toggleStorage.GetComponent<Toggle>().isOn);
    }

    public void SetBuildType(string name)
    {
        builder.SetCurrentBuilding(name); 
    }

    public void SwitchDisplayedBar(string barName)
    {
        if (!displayBarMap.ContainsKey(barName))
            Debug.LogError("Have been given a bar name that does not exist in the map: " + barName);
        else
        {
            foreach(string name in displayBarMap.Keys)
            {
                if (name == barName)
                    displayBarMap[name].SetActive(true);
                else
                    displayBarMap[name].SetActive(false);
            }
        }
    }

    public void SwitchOffDisplayBars()
    {
        foreach (string name in displayBarMap.Keys)
        {
            displayBarMap[name].SetActive(false);
        }
    }


}
