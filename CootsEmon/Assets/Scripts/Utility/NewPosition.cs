using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPosition : MonoBehaviour
{
    private Vector3 rightPos = new Vector3(155,0,0);
    private Vector3 leftPos = new Vector3(-616,0,0);
    private void Awake() {
        FindNewPosition();
    }
    public void FindNewPosition(){
        transform.localPosition = Vector3.Lerp(rightPos, leftPos, Random.Range(0f,1f));
    }
}
