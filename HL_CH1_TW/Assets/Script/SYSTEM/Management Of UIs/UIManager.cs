using System.Collections.Generic;
using UnityEngine;

public class UIManager : SimpleSingleton<UIManager>
{
    public GameObject CanvasPrefab;
    public string UITemplatePath = "UI Prefab";

    // Separate template lists for separate UI categories.
    private Dictionary<UIPanelID, GameObject> panelTemplates =
        new Dictionary<UIPanelID, GameObject>();

    private Dictionary<UIMainID, GameObject> mainUITemplates =
        new Dictionary<UIMainID, GameObject>();

    private RectTransform mainCanvas;

    public List<GameObject> openedUI = new List<GameObject>();

    public bool HasOpened => openedUI.Count > 0;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        mainCanvas = (RectTransform)Instantiate(CanvasPrefab).transform;

        GameObject[] templates =
            Resources.LoadAll<GameObject>(UITemplatePath);

        foreach (GameObject template in templates)
        {
            // Register PanelUI prefabs.
            if (template.TryGetComponent(out PanelUI panelUI))
            {
                panelTemplates[panelUI.ID] = template;
            }

            // Register MainUI prefabs.
            if (template.TryGetComponent(out MainUI mainUI))
            {
                mainUITemplates[mainUI.ID] = template;
            }
        }
    }

    // Open a PanelUI.
    public GameObject Open(UIPanelID id)
    {
        if (panelTemplates.TryGetValue(id, out GameObject template))
        {
            return CreateUI(template);
        }

        Debug.LogWarning("Cannot find PanelUI: " + id);
        return null;
    }

    // Open a MainUI.
    public GameObject Open(UIMainID id)
    {
        if (mainUITemplates.TryGetValue(id, out GameObject template))
        {
            return CreateUI(template);
        }

        Debug.LogWarning("Cannot find MainUI: " + id);
        return null;
    }

    // Replace everything with a PanelUI.
    public GameObject OpenReplace(UIPanelID id)
    {
        CloseAll();
        return Open(id);
    }

    // Replace everything with a MainUI.
    public GameObject OpenReplace(UIMainID id)
    {
        CloseAll();
        return Open(id);
    }

    private GameObject CreateUI(GameObject template)
    {
        GameObject newUI = Instantiate(template, mainCanvas);
        openedUI.Add(newUI);

        return newUI;
    }

    public bool IsUIOpen(UIPanelID id)
    {
        return openedUI.Exists(ui =>
            ui != null &&
            ui.GetComponent<PanelUI>()?.ID == id);
    }

    public bool IsUIOpen(UIMainID id)
    {
        return openedUI.Exists(ui =>
            ui != null &&
            ui.GetComponent<MainUI>()?.ID == id);
    }

    public void Close(GameObject ui)
    {
        if (ui == null)
            return;

        openedUI.Remove(ui);
        Destroy(ui);
    }

    public void CloseAll()
    {
        GameObject[] copies = openedUI.ToArray();

        foreach (GameObject ui in copies)
        {
            Close(ui);
        }

        openedUI.Clear();
    }
}

public enum GameSceneID
{
    Cutscene
}

public enum UIPanelID
{
    CharacterSelect,
    HubWorld,
}

public enum UIMainID
{
    Start,
    Options,
    Guide,
    Credits,
    Exit,

    // In New Safe
    CharacterSelect,
    // In Old Safe
    HubWorld,
}

public enum UIPauseID
{
    Inventory,
    DeckBuilder,
    CraftAltar,
    News
}