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
    [SerializeField] private int levelNum;

    public Image levelStartImg;

    void Start()
    {
        
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
