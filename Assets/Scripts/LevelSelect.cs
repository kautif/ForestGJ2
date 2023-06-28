using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelConfirm;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI levelText;
    public int levelNum;
    public Material levelComplete;

    public Image levelStartImg;

    void Start()
    {
        //---LEVEL PROGRESSION CLEARING CODE!---
        //PlayerPrefs.SetInt("Level 0", 0); // Overworld
        //PlayerPrefs.SetInt("Level 1", 0); // Level 1
        //PlayerPrefs.SetInt("Level 2", 0); // Level 2
        //PlayerPrefs.SetInt("Level 3", 0); // Level 3
        //PlayerPrefs.SetInt("Level 4", 0); // Level 4
        //PlayerPrefs.SetInt("Level 5", 0); // Level 5
        //--------------------------------------
        
        //Sets level completetion material to level node when the level is complete. //This is using the build index.
        string currentLevelIndex = "Level " + this.gameObject.GetComponent<LevelSelect>().levelNum;
        if(PlayerPrefs.GetInt(currentLevelIndex) > 0){
            //The player has beat this level
            this.gameObject.GetComponent<MeshRenderer>().material = levelComplete;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                if (hit.collider.CompareTag("Map Node"))
                {
                    
                    switch (hit.collider.gameObject.name) {
                        case "1":
                            levelNum = 1;
                            break;
                        case "2":
                            levelNum = 2;
                            break;
                        case "3":
                            levelNum = 3;
                            break;
                        case "4":
                            levelNum = 4;
                            break;
                        case "5":
                            levelNum = 5;
                            break;
                        default: 
                            levelNum = 0; 
                            break;

                    }
                    levelText.text = $"Level {levelNum}";
                    levelConfirm.SetActive(true);
                }
            }
        }
    }
    public void TransitionToLevel() { 
        levelStartImg.gameObject.SetActive(true);
        StartCoroutine(LoadLevel());
    }

    public IEnumerator LoadLevel() 
    {
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(levelNum);
    }

    public void OnCancelClick() {
        levelConfirm.SetActive(false);
    }
}
