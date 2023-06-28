using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformerController : MonoBehaviour
{
    private Rigidbody rigidPlayer;
    private CapsuleCollider playerCollider;
    private Transform spawnPoint = null;
    private GameObject[] spawns = null;
    public GameObject gm = null;
    [SerializeField] private float gravityRate = -9.81f;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float JumpForce = 450.0f;
    [SerializeField] private float FlyForce = 350.0f;
    [SerializeField] private float bumpForce = 40.0f;
    private int turnDirection = 0;
    [SerializeField] public int featherCount = 0;
    private int maxFeathers = 5;
    [SerializeField] private int score = 0;

    private Vector3 rayHitPoint = Vector3.zero;

    //We every time this player controller is created it should have an instance of game manager set ( Unless you can call to a static instance?)

    [SerializeField] bool grounded = false;
    // Start is called before the first frame update
    Dictionary<string, int> layers = new Dictionary<string, int>();

    void Start()
    {
        layers.Add("Terrain", 6);
        Physics.gravity = new Vector3(0,gravityRate,0);
        rigidPlayer = this.GetComponent<Rigidbody>();
        playerCollider = this.GetComponent<CapsuleCollider>();
        //Always check last spawn point so that we can add checkpoints.
        if(spawnPoint == null && spawns == null){
            spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            foreach (GameObject spawn in spawns)
            {
                
                spawnPoint = spawn.transform;
                
            }
            
        }
        if(gm == null){
            gm = GameObject.FindGameObjectsWithTag("GM")[0];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        
        Physics.gravity = new Vector3(0,gravityRate,0);
        //Move
        this.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed,0,0));
        Turn();

        //Jump and glide
        if(IsGroundedRay()){
            if(Input.GetAxis("Vertical") > 0.00f){
                rigidPlayer.AddRelativeForce(Vector3.up * Time.deltaTime * JumpForce, ForceMode.Impulse);
            }
        } else {
            if(Input.GetAxis("Vertical") > 0.00f){
                rigidPlayer.AddRelativeForce(Vector3.up * Time.deltaTime * FlyForce, ForceMode.Acceleration);
            }
        }
        
        

    }

    void Turn() {
        if(Input.GetAxis("Horizontal") > 0){
            if(turnDirection == 0){
                this.transform.GetChild(1).Rotate(0,90,0);
            } else if (turnDirection == -1){
                this.transform.GetChild(1).Rotate(0,180,0);
            }
            turnDirection = 1; // right
        } else if (Input.GetAxis("Horizontal") < 0){
            if(turnDirection == 0){
                this.transform.GetChild(1).Rotate(0,-90,0);
            }
            else if(turnDirection == 1) {
                this.transform.GetChild(1).Rotate(0,-180,0);
            }
            turnDirection = -1; // left
        }
    }

    private bool IsGroundedRay() {
        float extraHeightTest = 0.01f;
        RaycastHit rayHit;
        Color rayColor = Color.white;

        if(Physics.Raycast(playerCollider.bounds.center, Vector3.down, out rayHit,playerCollider.bounds.extents.y + extraHeightTest)){
            //Debug.Log("HIT" + rayHit.collider.transform.position);
        }
        if(rayHit.collider != null){
            rayColor = Color.green;
            grounded = true;
        } else {
            rayColor = Color.red;
            grounded = false;
        }

        Debug.DrawRay(playerCollider.bounds.center, Vector3.down * (playerCollider.bounds.extents.y + extraHeightTest), rayColor);
        
        return rayHit.collider != null;
        
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.gameObject.tag == "Hazard"){
            //Needs a hit timeout so that the player cant collide more than once every 250-500 ms. This way we can just throw a fraction of the feathers
            // out and they can fall to the ground.

            gm.GetComponent<GameManager>().AddScore(-10);
            gm.GetComponent<GameManager>().AddFeathers(-1);
            
            //Cause feathers or feather to drop.
            //Screen animation for hit? Blood particle animation?
            Vector3 bumpDistance = Vector3.zero;
            
            //The opposite of this is a cool effect that pulls the player in.
            bumpDistance.x = this.transform.position.x - collision.collider.transform.position.x;
            bumpDistance.y = (this.transform.position.y - collision.collider.transform.position.y) * 0.5f;
            
            rigidPlayer.AddForce(bumpDistance * Time.deltaTime * bumpForce, ForceMode.Impulse);
            
            gm.GetComponent<GameManager>().PlayerDeath();
            
        }

        if(collision.collider.gameObject.tag == "Feather"){
            Destroy(collision.collider.gameObject);
            
            //Add to feathers logic.
            if(featherCount >= maxFeathers){
                gm.GetComponent<GameManager>().AddScore(100);
            } else {
                gm.GetComponent<GameManager>().AddScore(100);
                gm.GetComponent<GameManager>().AddFeathers(1);
                
            }
            
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "CheckPoint"){
            gm.GetComponent<GameManager>().SetCheckpoint(other.gameObject.name);
        }
        if(other.gameObject.tag == "EndPoint"){
            gm.GetComponent<GameManager>().LevelFinish();
        }
    }
    

}
