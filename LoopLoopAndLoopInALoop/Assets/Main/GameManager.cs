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
        FadeIn();
        sceneIndex++;
        if (sceneIndex >= scenes.Count()) {
            sceneIndex = 0;
        }
        SceneManager.LoadScene(scenes[sceneIndex]);
    }
}
