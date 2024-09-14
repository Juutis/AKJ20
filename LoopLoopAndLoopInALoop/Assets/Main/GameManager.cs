using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private SpriteRenderer curtains;

    private float fadeDuration = 1.0f;
    private float fadeInTimer = 0;
    private float fadeOutTimer = 0;

    [SerializeField]
    private string[] scenes;

    private int sceneIndex = 0;
    
    private int totalLoops = 5;
    private int currentLoop = 0;

    [SerializeField]
    private GameObject winGameScreen;

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FadeIn();
    }

    void Update()
    {
        if (fadeInTimer > Time.time)
        {
            var t = (fadeInTimer - Time.time) / fadeDuration;
            t = Mathf.Clamp01(t);
            curtains.color = Color.Lerp(Color.clear, Color.black, t);
        }
        if (fadeOutTimer > Time.time)
        {
            var t = (fadeOutTimer - Time.time) / fadeDuration;
            t = Mathf.Clamp01(t);
            curtains.color = Color.Lerp(Color.black, Color.clear, t);
        }
    }

    public void LoadNextLevel()
    {
        FadeOut();
        Invoke("GoToNextLevel", fadeDuration);
    }

    public void FadeOut()
    {
        fadeOutTimer = Time.time + fadeDuration;
    }

    public void FadeIn()
    {
        fadeInTimer = Time.time + fadeDuration;
    }

    void GoToNextLevel() {
        sceneIndex++;
        if (sceneIndex >= scenes.Count()) {
            sceneIndex = 0;
            currentLoop++;
            if (currentLoop >= totalLoops)
            {
                Debug.Log("ROLL CREDITS");
                winGameScreen.SetActive(true);
                return;
            }
        }
        FadeIn();
        var difficulty = (float)currentLoop / (totalLoops - 1); // 0.0 = min difficulty, 1.0 = max difficulty
        HangManManager.Instance.Difficulty = difficulty;
        SceneManager.LoadScene(scenes[sceneIndex]);
    }
}
