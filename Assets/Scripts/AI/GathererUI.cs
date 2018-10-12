using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GathererUI : MonoBehaviour
{

    public GameObject gathererUI;
    public GameObject title;
    public GameObject resourceContent;
    public GameObject prefabResourceItem;
    private GathererAI gathererAI;
    private List<ResourceNode> resourcePickups;
    private List<StorageNode> storageDropoffs;
    private List<GameObject> resourceContentItems;
    private string gathererName;
    private UIManager ui;
    private GameManager manager;
    private bool selectingResource;
    private bool selectingStorage;

    void Start ()
    {
        resourcePickups = new List<ResourceNode>();
        storageDropoffs = new List<StorageNode>();
        resourceContentItems = new List<GameObject>();
        ui = FindObjectOfType<UIManager>();
        gathererUI = ui.gathererUI;
        title = ui.gathererTitle;
        resourceContent = ui.gathererResourceContent;
        selectingResource = false;
        gathererAI = GetComponent<GathererAI>();
        manager = FindObjectOfType<GameManager>();
	}
	
	public void UpdateInfo(List<ResourceNode> resourceNodes, List<StorageNode> storageNodes, string name)
    {
        resourcePickups = resourceNodes;
        foreach(ResourceNode node in resourcePickups)
        {
            GameObject go = (GameObject)Instantiate(prefabResourceItem, resourceContent.transform);
            go.GetComponent<Text>().text = node.name;
            resourceContentItems.Add(go);
        }
        storageDropoffs = storageNodes;
        /*foreach (StorageNode node in storageDropoffs)
        {
            GameObject go = (GameObject)Instantiate(prefabResourceItem, storageContent.transform);
            go.GetComponent<Text>().text = node.name;
        }*/
        gathererName = name;
        title.GetComponent<Text>().text = gathererName;
        ui.currentGatherer = this;
        gathererUI.SetActive(true);
    }

    public void ToggleUI(bool toggle)
    {
        gathererUI.SetActive(toggle);
        if(!toggle)
        {
            foreach(GameObject go in resourceContentItems)
            {
                Destroy(go);
                resourceContentItems.Remove(go);
            }
        }
    }

    private void Update()
    {
        if(selectingResource && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider.tag == "Resource")
                {
                    ResourceNode node = manager.FindNodeFromPosition(hitInfo.collider.gameObject.transform.position);
                    if (node != null)
                    {
                        Debug.Log("Adding resource");
                        AddResource(node);
                        selectingResource = false;
                    }
                }
                else
                {
                    Debug.Log("Disabling selection: " + hitInfo.collider.tag);
                    selectingResource = false;
                }
            }
            
        }
    }

    public void AddResource(ResourceNode node)
    {
        gathererAI.AddToResourceList(node);
    }

    public void SetSelectResource(bool toggle)
    {
        selectingResource = toggle;
    }

    public void SetSelectStorage(bool toggle)
    {
        selectingStorage = toggle;
    }
}
