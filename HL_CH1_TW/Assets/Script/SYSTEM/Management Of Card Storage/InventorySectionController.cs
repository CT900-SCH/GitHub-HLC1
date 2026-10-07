using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySectionPage
{
    public string pageName;
    public Button button;
    public GameObject section;
    public GameObject highlight;
}

public class InventorySectionController : MonoBehaviour
{
    [SerializeField] private List<InventorySectionPage> pages = new();

    [SerializeField, Min(0)]
    private int startingPageIndex = 0;

    private void Awake()
    {
        // Connect every button to its matching section.
        for (int i = 0; i < pages.Count; i++)
        {
            int pageIndex = i;

            if (pages[i].button != null)
            {
                pages[i].button.onClick.AddListener(
                    () => ShowPage(pageIndex)
                );
            }
        }

        if (pages.Count > 0)
        {
            startingPageIndex =
                Mathf.Clamp(startingPageIndex, 0, pages.Count - 1);

            ShowPage(startingPageIndex);
        }
    }

    public void ShowPage(int selectedPageIndex)
    {
        if (selectedPageIndex < 0 ||
            selectedPageIndex >= pages.Count)
        {
            return;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i].section != null)
            {
                pages[i].section.SetActive(i == selectedPageIndex);
            }

            if (pages[i].highlight != null)
            {
                pages[i].highlight.SetActive(i == selectedPageIndex);
            }
        }
    }
}
