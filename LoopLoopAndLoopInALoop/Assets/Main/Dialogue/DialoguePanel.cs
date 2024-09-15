using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private GameObject gunmanFace;

    [SerializeField]
    private GameObject sheriffFace;

    [SerializeField]
    private GameObject boozemanFace;

    private DialogueLine curDialogLine;
    private float lineTriggered;
    private float charactersPerSecond = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curDialogLine.Text == null)
        {
            textField.SetText("");
            return;
        }
        int showChars = (int)((Time.time - lineTriggered) * charactersPerSecond);
        textField.SetText(curDialogLine.Text.Substring(0, Mathf.Min(showChars, curDialogLine.Text.Length)));
    }

    public bool IsDone()
    {
        if (curDialogLine.Text == null)
        {
            return true;
        }
        return textField.text.Length == curDialogLine.Text.Length;
    }

    public void Skip()
    {
        lineTriggered = -1000;
    }

    public void ShowLine(DialogueLine line)
    {
        curDialogLine = line;
        textField.SetText("");
        gunmanFace.SetActive(false);
        sheriffFace.SetActive(false);
        boozemanFace.SetActive(false);
        switch(line.Character)
        {
            case Character.GUNMAN:
                gunmanFace.SetActive(true);
                break;
            case Character.SHERIFF:
                sheriffFace.SetActive(true);
                break;
            case Character.DRUNKARD:
                boozemanFace.SetActive(true);
                break;
        }
        lineTriggered = Time.time;
    }
}

[System.Serializable]
public struct DialogueLine
{
    public Character Character;
    public string Text;
}

public enum Character
{
    GUNMAN,
    SHERIFF,
    DRUNKARD
}