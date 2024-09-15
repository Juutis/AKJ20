using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer main;
    void Awake()
    {
        main = this;
    }
    [SerializeField]
    private List<GameMusic> gameMusics = new();
    public void PlayMusic(MusicType musicType)
    {
        var audios = gameMusics.Where(music => music != null && music.AudioSource != null);
        foreach (var audio in audios) {
            if (audio.AudioSource != null) {
                audio.AudioSource.Pause();
            }
        }
        GameMusic target = gameMusics.Find(music => music.Type == musicType);
        if (target != null && target.AudioSource != null)
        {
            target.AudioSource.Play();
        }
    }
}
[System.Serializable]
public class GameMusic
{
    public MusicType Type;
    public AudioSource AudioSource;
}

public enum MusicType
{
    MainMenu,
    Saloon,
    Chase,
    Hangman
}
