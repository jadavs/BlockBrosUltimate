using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    float health = 100f;
    float maxHealth = 100f;
    public GameObject deathParticles;
    public int livesRemaining = 3;
    public Slider healthBar;
    public GameObject heartText;
    public GameObject panel1, panel2;
    bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = 3;
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (livesRemaining <= 0) {
            gameover = true;
            if (this.gameObject.name == "Player1") {
                panel2.SetActive(true);
                Debug.Log("Player 2 WON THE GAME");                
            } else {
                panel1.SetActive(true);
                Debug.Log("Player 1 WON THE GAME");
            }
            Debug.Log("GAME OVER");
        }

        if (gameover) {
            float input = Input.GetAxis("Restart");
            if (input > 0f) {
                SceneManager.LoadScene("MainScreen");
                GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                gameManager.battleScene = false;
            }            
        }
    }

    public void takeDamage(float dmg) {
        health -= dmg;
        Instantiate(deathParticles, transform.position, transform.rotation);
        UpdateHealthBar();
        // play death animation
        if (health <= 0f) {
            transform.position = Vector3.zero;
            health = maxHealth;
            UpdateHealthBar();
            if (livesRemaining > 0) {
                livesRemaining--;
                heartText.GetComponent<TMP_Text>().text = livesRemaining + "";
            }            
            Die();
        }
    }

    void Die() {
        // do the death effect        
        Debug.Log("Dead bruh");
    }

    public void UpdateHealthBar() {
        healthBar.value = CalculateHealth();
    }

    float CalculateHealth() {
        return health / maxHealth;
    }
}
