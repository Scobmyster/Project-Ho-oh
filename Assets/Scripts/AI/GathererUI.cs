using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GathererUI : MonoBehaviour
{

    public GameObject gathererUI;
    public GameObject title;
    public GameObject content;
    public GameObject prefabResourceItem;
    private GathererAI gathererAI;
    private List<Extractor> extractorList;
    private List<StorageNode> storageDropoffs;
    private List<GameObject> resourceContentItems;
    private string gathererName;
    private UIManager ui;
    private GameManager manager;
    private bool selectingExtractor;
    private bool selectingStorage;
    private bool displayResource;
    private bool displayStorage;

    void Start()
    {
        extractorList = new List<Extractor>();
        storageDropoffs = new List<StorageNode>();
        resourceContentItems = new List<GameObject>();
        ui = FindObjectOfType<UIManager>();
        gathererUI = ui.gathererUI;
        title = ui.gathererTitle;
        content = ui.gathererContent;
        selectingExtractor = false;
        displayResource = true;
        displayStorage = true;
        gathererAI = GetComponent<GathererAI>();
        manager = FindObjectOfType<GameManager>();
    }

    public void UpdateInfo(List<Extractor> extractors, List<StorageNode> storageNodes, string name)
    {
        CleanUI();
        Debug.Log("Display resource: " + displayResource);
        Debug.Log("Display storage: " + displayStorage);
        if(displayResource)
        {
            extractorList = extractors;
            foreach (Extractor extractor in extractorList)
            {
                GameObject go = (GameObject)Instantiate(prefabResourceItem, content.transform);
                go.GetComponent<Text>().text = extractor.GetExtractorName();
                resourceContentItems.Add(go);
            }
        }
        if(displayStorage)
        {
            storageDropoffs = storageNodes;
            foreach (StorageNode node in storageDropoffs)
            {
                GameObject go = (GameObject)Instantiate(prefabResourceItem, content.transform);
                go.GetComponent<Text>().text = node.name;
                resourceContentItems.Add(go);
            }
        }
        gathererName = name;
        title.GetComponent<Text>().text = gathererName;
        ui.currentGatherer = this;
        gathererUI.SetActive(true);
    }

    public void ToggleUI(bool toggle)
    {
        gathererUI.SetActive(toggle);
        if (!toggle)
        { 
            CleanUI();
        }
    }

    public void CleanUI()
    {
        for(int i = 0; i < resourceContentItems.Count; i++)
        {
            Destroy(resourceContentItems[i]);
        }
        resourceContentItems = new List<GameObject>();
    }

    private void Update()
    {
        if (selectingExtractor && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider.gameObject.GetComponentInParent<Extractor>() != null)
                {
                    Extractor node = hitInfo.collider.gameObject.GetComponentInParent<Extractor>();
                    if (node != null)
                    {
                        Debug.Log("Adding extractor");
                        AddExtractor(node);
                        selectingExtractor = false;
                    }
                }
                else
                {
                    Debug.Log("Disabling selection: " + hitInfo.collider.name);
                    selectingExtractor = false;
                }
            }

        }

        if(selectingStorage && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider.tag == "Storage")
                {
                    StorageNode node = manager.FindStorageNodeFromPosition(hitInfo.collider.gameObject.transform.position);
                    if (node != null)
                    {
                        Debug.Log("Adding storage");
                        AddStorage(node);
                        selectingStorage = false;
                    }
                }
                else
                {
                    Debug.Log("Disabling selection: " + hitInfo.collider.tag);
                    selectingStorage = false;
                }
            }
        }
    }


    public void SetResourceOnly(bool toggle)
    {
        displayResource = toggle;
        UpdateInfo(gathererAI.GetExtractorList(), gathererAI.GetStorageList(), gathererAI.name);
    }

    public void SetStorageOnly(bool toggle)
    {
        displayStorage = toggle;
        UpdateInfo(gathererAI.GetExtractorList(), gathererAI.GetStorageList(), gathererAI.name);
    }

    public void AddExtractor(Extractor node)
    {
        gathererAI.AddToExtractorList(node);
    }

    public void AddStorage(StorageNode node)
    {
        gathererAI.AddToStorageList(node);
    }

    public void SetSelectExtractor(bool toggle)
    {
        selectingExtractor = toggle;
    }

    public void SetSelectStorage(bool toggle)
    {
        selectingStorage = toggle;
    }
}
