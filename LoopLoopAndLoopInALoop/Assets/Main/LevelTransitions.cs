using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LevelTransitions", menuName = "Scriptable Objects/LevelTransitions")]
public class LevelTransitions : ScriptableObject
{
    public DialogueLine[] InitialDialogue;
    public Day[] Days;
}

[System.Serializable]
public struct Day
{
    public LevelTransition BoozeToLasso;
    public LevelTransition LassoToHanging;
    public LevelTransition HangingToBooze;
}

[System.Serializable]
public struct LevelTransition
{
    public DialogueLine[] Dialog;
}