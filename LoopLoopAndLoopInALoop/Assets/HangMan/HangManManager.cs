using UnityEngine;

public class HangManManager : MonoBehaviour
{
    public static HangManManager Instance;
    public bool ShowTutorial = true;

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
            Instance.ShowTutorial = false;
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (GameManager.Instance != null) {
            Difficulty = GameManager.Instance.GetDifficulty();
        }
    }

    void Update()
    {
        if (GameManager.Instance != null) {
            Difficulty = GameManager.Instance.GetDifficulty();
        }
    }
}
