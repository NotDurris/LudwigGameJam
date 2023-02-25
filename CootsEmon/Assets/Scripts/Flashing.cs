using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    [SerializeField]
    private GameObject flasher;
    [SerializeField]
    private float flashCooldown;
    private float cFlashCooldown;
    // Update is called once per frame
    private void Awake() {
        cFlashCooldown = flashCooldown;
    }
    void Update()
    {
        if(cFlashCooldown > 0){
            cFlashCooldown -= Time.deltaTime;
        }else{
            flasher.SetActive(!flasher.activeSelf);
            cFlashCooldown = flashCooldown;
        }
    }
}
