using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;
    public Sound[] sounds;
    void Awake()
    {
        if(am == null){
            am = this;
        }else{
            if(am != this){
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        SceneManager.activeSceneChanged += PlayTheme;
    }
    private void PlayTheme(Scene current, Scene next){
        int id = next.buildIndex;
        if(id == 0){
            Play("Menu");
            Pause("World");
            Pause("Battle");
        }else if(id == 1){
            Pause("Menu");
            Play("World");
            Pause("Battle");
        }else if(id == 2){
            Pause("Menu");
            Pause("World");
            Play("Battle");
        }else{
            Pause("Menu");
            Pause("World");
            Pause("Battle");
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void Pause(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
    public bool IsPlaying(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }
    public void SetPitch(string name, float ptichLevel){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = ptichLevel;
    }
}