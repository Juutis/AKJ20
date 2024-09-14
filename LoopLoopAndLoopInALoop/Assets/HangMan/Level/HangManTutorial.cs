using UnityEngine;

public class HangManTutorial : MonoBehaviour
{
    void Awake()
    {
        HangManTutorial[] objs = FindObjectsByType<HangManTutorial>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (objs.Length > 1)
        {
            foreach (HangManTutorial obj in objs)
            {
                obj.gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
