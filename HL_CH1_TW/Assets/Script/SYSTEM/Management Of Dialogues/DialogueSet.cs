using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Dialogue Set", menuName = "Dialogue/Dialogue Set")]
public class DialogueSet : ScriptableObject
{
    public string setName;
    public DialogueLine[] lines;
    public string sceneName;
}