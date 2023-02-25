using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOverTime : MonoBehaviour
{
    [SerializeField]
    private float CountDown = 1.05f;
    private float cCountDown;
    private void Start() {
        cCountDown = CountDown;
    }
    private void Update() {
        if(cCountDown > 0){
            cCountDown -= Time.unscaledDeltaTime;
        }else{
            gameObject.SetActive(false);
            cCountDown = CountDown;
        }    
    }
}
