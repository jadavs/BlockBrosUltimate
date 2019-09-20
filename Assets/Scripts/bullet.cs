using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float range;
    //public GameObject spawnPoint;
    public bool shotgun = false;
    Vector3 startingPos;
    public float damage;
    public GameObject bulletCollisionVFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        startingPos = transform.position;
    }

    void Update() {
        if (Vector2.Distance(startingPos, transform.position) > range) {
            if (shotgun) {
                Destroy(transform.parent.gameObject);
            } else {
                Destroy(gameObject);
            }                                 
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        // destroy when hits ground
        if (col.gameObject.tag == "ground") {
            Destroy(gameObject);
        }
        // hit another bullet - destroy both
        if (col.gameObject.tag == "bullet") { 
            Destroy(col.gameObject);
            Destroy(gameObject);
            Instantiate(bulletCollisionVFX, transform.position, transform.rotation);
        }
        // hits player and does damage
        if (col.gameObject.tag == "Player") {
            PlayerHealth playa  = col.GetComponent<PlayerHealth>();
            if (playa != null) {
                playa.takeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
