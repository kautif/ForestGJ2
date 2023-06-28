using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roamer : MonoBehaviour
{
    
    [SerializeField] private Vector3 roamDirection;
    private RaycastHit rayGlob;
    [SerializeField] private Vector3 faceDirection = new Vector3(1,0,0); // 0 for left 1 for right
    [SerializeField] private float directionDistance = 1.0f;
    [SerializeField] private float roamSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        //Once the NPC sees a wall with the raycheck, the NPC changes directions 180 degrees.
        ChangeDirection();
        /*
        if(SeesWall()){
            // /Vector3 oppositeDirection = this.transform.position - wallHit.position;
        }
        */
        this.GetComponent<Rigidbody>().velocity += (roamSpeed * faceDirection);
    }
    //RAY CAST IN A DIRECTION THEN IF YOU HIT SOMETHING WITHIN RANGE CHANGE DIRECITON.
    void ChangeDirection(){
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, faceDirection, out hit, this.transform.localScale.x * directionDistance, layerMask)){
            if(faceDirection.x == 1){
                faceDirection.x = -1;
            } else {
                faceDirection.x = 1;
            }
        }
        //Raycheck that outputs its raycast hit to rayGlob;
        //Then returns the result so a boolean.
        //return false; // Change this value after raycast.
    }

    void OnCollisionEnter(Collision collision){
        //Change Direction lol. Like the turtle shells in mario.

        
    }
}
