using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    [SerializeField]
    private List<GameSound> sounds;
    public void PlaySound(GameSoundType soundType)
    {
        if (soundType == GameSoundType.None)
        {
            return;
        }
        GameSound gameSound = sounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                audio.pitch = 1f;
                audio.loop = false;
                if (gameSound.Type == GameSoundType.Bottle)
                {
                    audio.pitch = Random.Range(0f, 5f);
                }
                audio.Play();
            }
        }
    }
    public void PlayLoop(GameSoundType soundType)
    {
        if (soundType == GameSoundType.None)
        {
            return;
        }
        GameSound gameSound = sounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                if (!audio.isPlaying)
                {
                    audio.pitch = 1f;
                    audio.loop = true;
                    audio.Play();
                }
            }
        }
    }
    public void StopLoop(GameSoundType soundType)
    {
        if (soundType == GameSoundType.None)
        {
            return;
        }
        GameSound gameSound = sounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                if (audio.isPlaying)
                {
                    audio.pitch = 1f;
                    audio.loop = true;
                    audio.Stop();
                }
            }
        }
    }
}


public enum GameSoundType
{
    None,
    Bottle,
    FallingBottle,
    Horse,
    Shoot,
    Whoop,
    Throw
}


[System.Serializable]
public class GameSound
{
    [field: SerializeField]
    public GameSoundType Type { get; private set; }

    [field: SerializeField]
    private List<AudioSource> sounds;

    public AudioSource Get()
    {
        if (sounds == null || sounds.Count == 0)
        {
            return null;
        }
        return sounds[Random.Range(0, sounds.Count)];
    }
}