using UnityEngine;
using UnityEngine.Events;

public class DialogueRunner : MonoBehaviour
{
    private DialoguePanel panel;

    [SerializeField]
    private UnityEvent onDialogueCompleted;

    private DialogueLine[] dialogue;
    private int lineIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = GetComponentInChildren<DialoguePanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (panel.IsDone())
            {
                lineIndex++;
                if (lineIndex >= dialogue.Length)
                {
                    onDialogueCompleted.Invoke();
                }
                else
                {
                    panel.ShowLine(dialogue[lineIndex]);
                }
            }
            else
            {
                panel.Skip();
            }
        }
    }

    public void ShowDialogue(DialogueLine[] lines)
    {
        lineIndex = 0;
        dialogue = lines;
        panel.ShowLine(dialogue[0]);
    }
}
