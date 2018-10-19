using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererAI : MonoBehaviour
{

    private enum State
    {
        Idle,
        MovingToResource,
        CollectingResource,
        MovingToStorage,
        DroppingInStorage
    };

    private State state;
    private List<Extractor> extractorList;
    private List<StorageNode> storageNodeList;
    private Extractor currentExtractor;
    private StorageNode storageNode;
    private GathererUnit unit;
    private GameManager manager;
    private int resourceInventory;
    private int inventorySpace;
    private TextMesh inventoryText;
    private bool mouseOverMe;
    private bool displaying;
    public GathererUI gathererUI;

    private void Awake()
    {
        state = State.Idle;
        manager = FindObjectOfType<GameManager>();
        extractorList = new List<Extractor>();
        storageNodeList = new List<StorageNode>();
        unit = GetComponent<GathererUnit>();
        inventoryText = gameObject.GetComponentInChildren<TextMesh>();
        inventorySpace = 50;
        UpdateInventoryText();
        gathererUI = this.GetComponent<GathererUI>();
        GameManager.OnExtractorDemolishListeners += RemoveExtractorFromPlan;
        GameManager.OnStorageDemolishListeners += RemoveStorageFromPlan;
    }

    private void Update()
    {
        UpdateStateMachine();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            displaying = false;
            gathererUI.ToggleUI(displaying);
        }
        if (Input.GetMouseButtonDown(0) && mouseOverMe)
        {
            DisplayInterface();
            displaying = true;
        }
    }

    private void UpdateStateMachine()
    {
        switch(state)
        {
            case (State.Idle):
                UpdateInventoryText();
                if(currentExtractor != null && currentExtractor.HasResources())
                {
                    state = State.MovingToResource;
                }
                else
                {
                    int size = extractorList.Count;
                    if(extractorList != null && size > 0)
                        currentExtractor = extractorList[Random.Range(0, extractorList.Count)];
                }
                break;
            case (State.MovingToResource):
                UpdateInventoryText();
                if(unit.is_Idle && currentExtractor != null)
                {
                    unit.MoveTo(currentExtractor.GetPosition(), () =>
                    {
                        state = State.CollectingResource;
                    });
                }
                if(currentExtractor == null)
                {
                    state = State.Idle;
                }
                break;
            case (State.CollectingResource):
                    UpdateInventoryText();
                    if(unit.is_Idle)
                    {
                        if(resourceInventory >= inventorySpace)
                        {
                            state = State.MovingToStorage;
                        }
                        else if(!currentExtractor.HasResources())
                        {
                            state = State.Idle;
                        }
                        else
                        {
                            currentExtractor.GrabResource();
                            resourceInventory++;
                            UpdateInventoryText();
                        }
                    }
                break;
            case (State.MovingToStorage):
                UpdateInventoryText();
                if(unit.is_Idle)
                {

                    if (storageNodeList.Count > 0)
                    {
                        if (storageNode != null)
                        {
                            unit.MoveTo(storageNode.GetPosition(), () =>
                            {
                                state = State.DroppingInStorage;
                            });
                        }
                        else
                        {
                            storageNode = storageNodeList[0];
                        }
                    }
                }
                break;
            case (State.DroppingInStorage):
                UpdateInventoryText();
                if(unit.is_Idle)
                {
                    storageNode.DropResource(resourceInventory);
                    resourceInventory = 0;
                    UpdateInventoryText();
                    state = State.Idle;
                }
                break;
        }
    }

    public void RemoveExtractorFromPlan(Extractor node)
    {
        extractorList.Remove(node);
        currentExtractor = null;
    }

    public void RemoveStorageFromPlan(StorageNode node)
    {
        storageNodeList.Remove(node);
        storageNode = null;
    }

    private void DisplayInterface()
    {
        gathererUI.UpdateInfo(extractorList, storageNodeList, "Alpha Gatherer");
    }

    private void OnMouseOver()
    {
        mouseOverMe = true;
    }

    private void OnMouseExit()
    {
        mouseOverMe = false;
    }

    public void AddToExtractorList(Extractor node)
    {
        extractorList.Add(node);
    }

    public List<Extractor> GetExtractorList()
    {
        return extractorList;
    }

    public void AddToStorageList(StorageNode node)
    {
        storageNodeList.Add(node);
    }

    public List<StorageNode> GetStorageList()
    {
        return storageNodeList;
    }

    private void UpdateInventoryText()
    {
        inventoryText.text = "Status: " + state + " : " + resourceInventory;
    }
}
