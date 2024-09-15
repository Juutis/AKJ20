using System.Collections.Generic;
using System.Data;
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
    private MiniGame[] minigames;

    private int sceneIndex = 0;
    
    private int totalLoops = 2;
    private int currentLoop = 0;

    [SerializeField]
    private GameObject winGameScreen;

    [SerializeField]
    private GameObject lassoScreen;

    [SerializeField]
    private GameObject gunScreen;

    [SerializeField]
    private GameObject boozeScreen;

    [SerializeField]
    private GameObject loseScreen;

    private bool winScreenActive;

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
        if (winScreenActive)
        {
            if (Input.anyKeyDown)
            {
                GoToNextLevel();
            }
        }
    }

    public void LoadNextLevel()
    {
        FadeOut();
        Invoke("DisplayWinScreen", fadeDuration);
    }

    public void Lose()
    {
        FadeOut();
        Invoke("DisplayLoseScreen", fadeDuration);
    }

    public void FadeOut()
    {
        fadeOutTimer = Time.time + fadeDuration;
    }

    public void FadeIn()
    {
        fadeInTimer = Time.time + fadeDuration;
    }

    void DisplayWinScreen()
    {
        if (sceneIndex == minigames.Count() - 1 && currentLoop == totalLoops - 1) {
            winGameScreen.SetActive(true);
            return;
        }
        DisplayWinScreenForMiniGame(minigames[sceneIndex]);
        winScreenActive = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void HideWinScreen()
    {
        foreach (var minigame in minigames)
        {
            var winScreen = getWinScreen(minigame.WinScreen);
            winScreen.SetActive(false);
        }
        winScreenActive = false;
    }

    void DisplayLoseScreen()
    {
        loseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void RestartDay() 
    {
        loseScreen.SetActive(false);
        sceneIndex = 0;
        FadeIn();
        SceneManager.LoadScene(minigames[sceneIndex].SceneName);
    }

    void GoToNextLevel() {
        HideWinScreen();
        sceneIndex++;
        if (sceneIndex >= minigames.Count()) {
            sceneIndex = 0;
            currentLoop++;
        }
        FadeIn();
        var difficulty = (float)currentLoop / (totalLoops - 1); // 0.0 = min difficulty, 1.0 = max difficulty
        HangManManager.Instance.Difficulty = difficulty;
        SceneManager.LoadScene(minigames[sceneIndex].SceneName);
    }

    private void DisplayWinScreenForMiniGame(MiniGame miniGame)
    {
        var winScreen = getWinScreen(miniGame.WinScreen);
        winScreen.SetActive(true);
    }

    private GameObject getWinScreen(WinScreen winScreen)
    {
        switch (winScreen)
        {
            case WinScreen.LASSOMAN:
                return lassoScreen;
            case WinScreen.GUNMAN:
                return gunScreen;
            case WinScreen.BOOZEMAN:
                return boozeScreen;
            default:
                return null;
        }
    }
}

[System.Serializable]
struct MiniGame
{
    public string SceneName;
    public WinScreen WinScreen;
}

enum WinScreen
{
    GUNMAN,
    BOOZEMAN,
    LASSOMAN
}
