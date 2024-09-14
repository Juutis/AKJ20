using UnityEngine;

public class HangManManager : MonoBehaviour
{
    public static HangManManager Instance;

    public float Difficulty {
        get {
            return difficulty;
        }
        set {
            value = Mathf.Clamp01(value);
            difficulty = value;
        }
    }
    private float difficulty = 0;

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
