using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    private void Awake() {
        SceneManager.activeSceneChanged += SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, Scene arg1)
    {
        PauseGame(false);
    }

    public IEnumerator LoadNewScene(int id, string transitionEffect = "transitionStart"){
        PauseGame(true);
        transition.Play(transitionEffect);
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(id);
    }

    public void PauseGame(bool shouldPause){
        if(shouldPause){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
    }
}
