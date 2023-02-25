using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    public float Speed;
    [SerializeField] private Vector3 startPos;

    private Vector3 rightPos = new Vector3(8.54f,-3.53f,90f);
    private Vector3 leftPos = new Vector3(-8.54f,-3.53f,90f);
    private bool GoingLeft;
    // Start is called before the first frame update
    private void Awake() {
        startPos = (transform as RectTransform).position;
        GoingLeft = Random.Range(0,1) == 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 cPosition = (transform as RectTransform).position;
        if(GoingLeft){
            if(Vector3.Distance(cPosition, leftPos) > 0.01f){
                (transform as RectTransform).position = Vector3.MoveTowards(cPosition, leftPos, Speed * Time.deltaTime);
            }else{
                GoingLeft = !GoingLeft;
            }
            
        }else{
            if(Vector3.Distance(cPosition, rightPos) > 0.01f){
                (transform as RectTransform).position = Vector3.MoveTowards(cPosition, rightPos, Speed * Time.deltaTime);
            }else{
                GoingLeft = !GoingLeft;
            }
            
        }
    }
}
