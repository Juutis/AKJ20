using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private List<GameMusic> gameMusics = new();
    public void PlayMusic(MusicType musicType)
    {
        Debug.Log($"play music: {musicType}");
        List<GameMusic> audios = gameMusics.Where(music => music != null && music.AudioSource != null).ToList();
        Debug.Log($"audios count: {audios.Count}");
        foreach (GameMusic audio in audios) {
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
