using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SimpleSingleton<UIManager>
{
    public GameObject CanvasPrefab;

    public string UITemplatePath = "UI Prefab";

    Dictionary<GameUIID, GameObject> _UITemplates = new Dictionary<GameUIID, GameObject>();
    RectTransform _mainCanvas = null;

    public List<GameObject> _openedUI = new List<GameObject>();

    public bool HasOpened => _openedUI.Count > 0;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }


    // Initialize
    public void Initialize()
    {
        _mainCanvas = (RectTransform)Instantiate(CanvasPrefab).transform;

        GameObject[] templates = Resources.LoadAll<GameObject>(UITemplatePath);

        foreach (GameObject template in templates)
        {
            if (template.TryGetComponent(out GameUI uiTemplate))
            {
                _UITemplates[uiTemplate.ID] = template;
            }
        }
    }

    //Open
    public GameObject Open(GameUIID id)
    {
        GameObject newUIObj = null;

        if (_UITemplates.TryGetValue(id, out var template))
        {
            newUIObj = Instantiate(template, _mainCanvas);
            _openedUI.Add(newUIObj);
        }
        return newUIObj;
    }

    public GameObject OpenReplace(GameUIID id)
    {
        CloseAll();
        return Open(id);
    }

    //Close
    public void Close(GameObject obj)
    {
        _openedUI.Remove(obj);
        Destroy(obj); // TODO do a transition
    }

    //CloseAll
    public void CloseAll()
    {
        GameObject[] copies = _openedUI.ToArray(); // TODO David promised to explain why this is needed
        foreach (GameObject obj in copies)
        {
            Close(obj);
        }
        _openedUI.Clear();
    }

    //That UI is still there
    public bool IsUIOpen(GameUIID id)
    {
        return _openedUI.Exists(go => go.GetComponent<GameUI>()?.ID == id);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum GameUIID
{
    MainMenu,
    Settings,
    Pause,
    Gate,
    HUD,
    Logo,
    Title,
    HomeMenu,
    BlueScreen,
}

public enum LevelUIID
{
    NoLevels,
    Level1_Info,
    Victory1,
    Defeat1,
}
