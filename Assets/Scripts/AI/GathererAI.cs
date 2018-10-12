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
    private List<ResourceNode> resourceNodeList;
    private List<StorageNode> storageNodeList;
    private ResourceNode resourceNode;
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
        resourceNodeList = new List<ResourceNode>();
        storageNodeList = new List<StorageNode>();
        unit = GetComponent<GathererUnit>();
        inventoryText = gameObject.GetComponentInChildren<TextMesh>();
        inventorySpace = 50;
        UpdateInventoryText();
        gathererUI = this.GetComponent<GathererUI>();
    }

    private void Update()
    {
        UpdateStateMachine();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Closing interface");
            displaying = false;
            gathererUI.ToggleUI(displaying);
        }
        if (Input.GetMouseButtonDown(0) && mouseOverMe)
        {
            Debug.Log("Displaying interface");
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
                if(resourceNode != null && resourceNode.HasResources())
                {
                    state = State.MovingToResource;
                }
                else
                {
                    int size = resourceNodeList.Count;
                    if(resourceNodeList != null && size > 0)
                        resourceNode = resourceNodeList[Random.Range(0, resourceNodeList.Count)];
                }
                break;
            case (State.MovingToResource):
                UpdateInventoryText();
                if(unit.is_Idle)
                {
                    unit.MoveTo(resourceNode.GetPosition(), () => 
                    {
                        state = State.CollectingResource;
                    });
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
                        else if(!resourceNode.HasResources())
                        {
                            state = State.Idle;
                        }
                        else
                        {
                            resourceNode.GrabResource();
                            resourceInventory++;
                            UpdateInventoryText();
                        }
                    }
                break;
            case (State.MovingToStorage):
                UpdateInventoryText();
                if(unit.is_Idle)
                {
                    if (storageNode == null)
                        storageNode = storageNodeList[0];
                    unit.MoveTo(storageNode.GetPosition(), () => 
                    {
                        state = State.DroppingInStorage;
                    });
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

    private void DisplayInterface()
    {
        gathererUI.UpdateInfo(resourceNodeList, storageNodeList, "Alpha Gatherer");
    }

    private void OnMouseOver()
    {
        mouseOverMe = true;
    }

    private void OnMouseExit()
    {
        mouseOverMe = false;
    }

    public void AddToResourceList(ResourceNode node)
    {
        resourceNodeList.Add(node);
    }

    public void AddToStorageList(StorageNode node)
    {
        storageNodeList.Add(node);
    }

    private void UpdateInventoryText()
    {
        inventoryText.text = "Status: " + state + " : " + resourceInventory;
    }
}
