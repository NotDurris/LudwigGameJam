using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private LevelManager lm;
    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            FindObjectOfType<AudioManager>().Play("Interact");
            StartCoroutine(lm.LoadNewScene(1));
        }
    }
}
