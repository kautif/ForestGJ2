using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public GameObject[] Checkpoints;
    private GameObject checkpoint;
    [SerializeField] private int score = 0;
    [SerializeField] private int feathers = 0;
    private int maxFeathers = 5;
    private GameObject player = null;
    private int currentLevel = 0;
    private string currentCheckpoint = null;
    public GameObject levelComplete;
    


    // Start is called before the first frame update
    void Start()
    {
        //This code needs to be handed to fixed update for a level check if we want it in the overworld.
        Time.timeScale = 0;
        //Gets all checkpoints so that the current checkpoint can be found.
        Checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        
        foreach( GameObject spawn in spawns){
            spawnPoint = spawn.transform; //If you have multiple spawns this will just go to the latest one in the list.
        }

        //Checked on the start of level to set current checkpoint
        if(currentLevel == PlayerPrefs.GetInt("Highest Level")){
            currentCheckpoint = PlayerPrefs.GetString("Checkpoint");
        }

        foreach( GameObject check in Checkpoints) {
            if(check.name == currentCheckpoint){
                checkpoint = check;
            }
        }

        if(player == null){
            if(checkpoint != null) {
                player = Instantiate(playerPrefab, checkpoint.transform.position, playerPrefab.transform.rotation);
            } else {
                player = Instantiate(playerPrefab, spawnPoint);
            }
        }
        if(player != null){
            Time.timeScale = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

    }

    public void PlayerDeath() {
        
        if(feathers <= 0){
            score-=50;
            feathers = 0;
            player.transform.GetChild(1).gameObject.SetActive(false);
            player.GetComponent<CapsuleCollider>().enabled = false;
            if(checkpoint != null){
                Debug.Log("EXECUTED");
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.transform.position = checkpoint.transform.position;
                //LoadCheckpoint();
            } else if(spawnPoint != null && checkpoint == null){
                Debug.Log("Still Got Here");
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.transform.position = spawnPoint.position;
            }
            player.GetComponent<CapsuleCollider>().enabled = true;
            player.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    //Use a negative value to reduce score.
    public void AddScore(int scoreDelta) {
        score += scoreDelta;
        Debug.Log("Definitely Shouldn't");
    }

    public void AddFeathers(int featherDelta) {
        Debug.Log("Seems Like Maybe");
        if(feathers <=4){
            feathers = feathers + featherDelta;
        } else {
            score += 100;
        }
        
    }

    public void SaveState() {

        //Get scene manager so we can work with the build index
        Scene scene = SceneManager.GetActiveScene();

        //Gets the current level index and sets the level completed value to true IE 1 as we cant use booleans It would be 0 for false.
        string currentLevelIndex = "Level " + scene.buildIndex.ToString();
        PlayerPrefs.SetInt(currentLevelIndex, 1);

        //Gets highest level saved.
        int highestLevel = PlayerPrefs.GetInt("HighestLevel");
        //Checks if the highest completed level is earlier in the build index and if it is, then this becomes "HighestLevel"
        if(highestLevel < scene.buildIndex){
            PlayerPrefs.SetInt("HighestLevel",scene.buildIndex);
        }
        
        //Update Overall Score
        int totalScore = PlayerPrefs.GetInt("Score");
        totalScore += score;
        PlayerPrefs.SetInt("Score", totalScore);
        
        //Update Overall Feathers
        int totalFeathers = PlayerPrefs.GetInt("Feathers");
        totalFeathers += feathers;
        PlayerPrefs.SetInt("Score", totalScore);
        
    }

    public void LoadState() {
        
    }

    public void SetCheckpoint(string checkName) {
        foreach(GameObject check in Checkpoints){
            if(checkName == check.name){
                checkpoint = check;
            }
        }
    }
    public void LoadCheckpoint(){
        player.transform.position = checkpoint.transform.position;
    }

    public void LevelFinish(){
        Debug.Log("Level Complete!!");
        SaveState();
        if(!levelComplete.activeInHierarchy){
            levelComplete.SetActive(true);
        }
        
        //Save level completion, Level name will be the holder, then in the overworld this will be loaded on start and will show completed.
        // Then each level will be enabled based on a conditional that enables levels.
        
        //Feathers and score will be added to saved totals.
        // Level Complete!
        // Score and feathers And totals
        // Continue
        
    }
    public void LoadOverworld() {
        levelComplete.SetActive(false);
        SceneManager.LoadScene(0);
        
        
    }

    //Not using this at the moment because level order is not known.
    /*
    public void LoadNextLevel()
    {

    }
    */
}

