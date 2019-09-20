using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Rigidbody2D rb;
    public float knifeSpeed = 20f;
    // need bottom two only when we hit another player
    PlayerMovement playerScript;
    GameObject player;
    // for range purposes
    Vector3 startingPos;
    public float range = 50f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * knifeSpeed;
        //playerScript = player.GetComponent<PlayerMovement>();   
        startingPos = transform.position;
    }

    void Update() {
        if (Vector2.Distance(startingPos, transform.position) > range) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // when hitting a player or ground
        if (other.gameObject.tag == "ground") {
            rb.velocity = Vector2.zero;
            Invoke("DestroyKnife", 1);
        }   
        /*if (other.gameObject.tag == "Player") {
            playerScript.livesRemaining--;
        }*/
    }

    void DestroyKnife() {
        Destroy(gameObject);
    }
}
