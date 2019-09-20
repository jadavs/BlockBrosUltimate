using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;  //Static instance of GameManager which allows it to be accessed by any other script.
    // UI references
    public GameObject dropdown1;
    public GameObject dropdown2;

    // for main menu
    //bool p1Selected = false;
    //bool p2selected = false; 
    //bool playing = false;    
    public string p1Gun = "Magnum";
    public string p2Gun = "MachineGun";

    // for the actual game scene
    //private GameObject player1, player2;
    //private PlayerHealth playerH1, playerH2;
    private bool gameover;
    public bool battleScene = false;
    Button playButton;

    void Awake() {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }  //If instance already exists and it's not this:
        else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject); 
        }       
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);        
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(setupGame);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!battleScene) {
            if (dropdown1 == null) {
                dropdown1 = GameObject.Find("P1Choices");
                dropdown2 = GameObject.Find("P2Choices");
                playButton.onClick.AddListener(setupGame);             
            }
            changeP1Gun();
            changeP2Gun();
        }
    }

    public void setupGame() {
        // load new scene with player loadouts requested
        SceneManager.LoadScene("BattleScene");
        battleScene = true;
    }

    public void changeP1Gun() {
        if (dropdown1 != null) {
            Dropdown p1Dropdown = dropdown1.GetComponent<Dropdown>();
            if (p1Dropdown != null) {
                int value = p1Dropdown.value;
                p1Gun = p1Dropdown.options[value].text; 
            }
        }                    
    }

    public void changeP2Gun() {
        if (dropdown2 != null) {
            Dropdown p2Dropdown = dropdown2.GetComponent<Dropdown>();
            if (p2Dropdown != null) {
                int value = p2Dropdown.value;
                p2Gun = p2Dropdown.options[value].text;
            }
        }               
    }   
    
    
    /*void OnTriggerEnter2D(Collider2D other) {
        // Removes one life and resets players' positions if either one dies
        if (other.gameObject.tag == "Player") {
            if (other.gameObject.name == "Player1") {
                playerH1.livesRemaining--;
                player1.transform.position = Vector3.zero;
            } else if (other.gameObject.name == "Player2") {
                playerH2.livesRemaining--;
                player2.transform.position = Vector3.zero;
            }            
        }
    }*/
}
