using UnityEngine;

public class HangManTutorial : MonoBehaviour
{
    void Start()
    {
        if (!HangManManager.Instance.ShowTutorial)
        {
            Destroy(gameObject);
        }
    }
}
