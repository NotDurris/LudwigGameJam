using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static Vector2 Position;
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float rotationMagnitude;
    private Vector2 moveVector;
    private Rigidbody2D rb;
    private Animator am;

    private float timeKeeping = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Position;
        foreach(TrainerEncounter te in FindObjectsOfType<TrainerEncounter>()){
            te.LostBattleDialogue();
        }
        rb = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position;
        moveVector = Vector2.zero;

        if(Input.GetKey(KeyCode.DownArrow)){
            moveVector.y = -1;
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            moveVector.x = -1;
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            moveVector.y = 1;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            moveVector.x = 1;
        }

        if(isMoving()){
            if(timeKeeping < Mathf.PI){
                timeKeeping += Time.deltaTime;
            }else{
                timeKeeping = 0;
            }
                
            transform.rotation =  Quaternion.Euler(Vector3.forward * Mathf.Sin(timeKeeping * rotationSpeed) * rotationMagnitude);
        }else{
            transform.rotation =  Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.z, 0, 0.2f));
        }
    }

    public bool isMoving(){
        if(moveVector != Vector2.zero){
            am.SetBool("IsMoving", true);
            return true;
        } else{
            am.SetBool("IsMoving", false);
            return false;
        }
    }

    private void FixedUpdate() {
        rb.velocity = moveVector.normalized * Speed;
    }
}
