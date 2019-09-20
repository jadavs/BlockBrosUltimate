using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    GameObject gun;
    public GameObject[] guns;

    // player joystick controls
    public string moveLR = "HorizontalP1";    
    public string jumpAxis = "JumpP1";
    public string blinkAxis = "BlinkP1";
    // AxisDowns
    private bool jumpAxisInUse = false;
    //private bool blinkAxisInUse = false;

    // moving controls
    public float speed = 10f;
    private Vector2 velocity;
    private int move = 0;
    public float maxSpeed = 14;
    public float changeBoost  = 4f;

    // jump controls
    public float jumpVelocity = 5f;    
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 1f;
    bool jumpRequest = false;
    bool onGround = true;
    public float jumpCount = 0, maxJumps = 3;

    // recoil controls
    public bool addRecoil = false; 
    public float recoilF = 5f;

    // blink controls
    public bool blink = false;
    bool allowBlink = true;
    public float blinkUnits = 5f;
    public float yLimit = 5f;
    public float blinkRate = 1f;

    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        //gun = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        
        // first get game manager and find out which gun you want
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string gunToAdd = "Magnum";
        if (jumpAxis == "JumpP1") {
            gunToAdd = gameManager.p1Gun;    
        } else if (jumpAxis == "JumpP2") {
            gunToAdd = gameManager.p2Gun;
        }
        int gunIndex = findGunNumber(gunToAdd);
        // now instantiate that gun's prefab object onto the sphere
        if (gunIndex == -1) {
            Debug.Log("Gun Not FOUND");
        } else {
            Instantiate(guns[gunIndex], this.gameObject.transform.GetChild(0));
            gun = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        }
    }

    int findGunNumber(string gunName) {
        Debug.Log(gunName);
        for (int i = 0; i < guns.Length; i++) {
            if (guns[i].name == gunName) {
                return i;
            }
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Input for player movement and jumps
        float moveInput = Input.GetAxis(moveLR);
        if (moveInput > 0f) {
            move = 1;
        }
        if (moveInput < 0f) {
            move = -1;
        }
        if (moveInput == 0) {
            move = 0;
        }
        
        float jumpInput = Input.GetAxis(jumpAxis);
        if (jumpInput > 0f && jumpCount < maxJumps) {
            if (!jumpAxisInUse) {
                jumpRequest = true;
                jumpCount++;
                onGround = false;
                jumpAxisInUse = true;
            }            
        }
        if (onGround) {
            jumpCount = 0;
        }
        if (jumpInput == 0f) {
            jumpAxisInUse = false;
        }
        
        float blinkInput = Input.GetAxis(blinkAxis);
        if (blinkInput > 0f && !onGround && allowBlink) {
            blink = true;
        }
    }

    public void recoil(float recoilForce) {
        addRecoil = true;
        recoilF = recoilForce; 
    }

    void OnCollisionEnter2D(Collision2D col) {        
        if (col.gameObject.tag == "ground") {
            onGround = true;
        }
    }

    void FixedUpdate() {
        movePlayer();
        if (jumpRequest) {
            jumpPlayer();
        }
        if (addRecoil) {
            recoil();            
        }
        if (!onGround && blink && allowBlink) {
            StartCoroutine("blinkPlayer");
        }
        handleGravity();        
    }

    void handleGravity() {
        if (rb.velocity.y < 0) {
            rb.gravityScale = fallMultiplier;
        } else if (rb.velocity.y > 0) {
            rb.gravityScale = jumpMultiplier;
        } else {
            rb.gravityScale = 1f;
        }
    }

    IEnumerator blinkPlayer() {
        blink = false;
        allowBlink = false;
        Vector2 currPosition = new Vector2(gun.transform.position.x, gun.transform.position.y);
        Vector2 newPosition = currPosition;            
        if (move == 1) {
            newPosition = new Vector2(currPosition.x + blinkUnits, currPosition.y);
        } else if (move == -1) {
            newPosition = new Vector2(currPosition.x - blinkUnits, currPosition.y);
        }
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);        
        yield return new WaitForSeconds(blinkRate);
        allowBlink = true;
    }

    void jumpPlayer() {
        rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        jumpRequest = false;
    }

    void recoil() {
        Vector2 forceDirection = new Vector2(gun.transform.right.x, gun.transform.right.y);
        rb.AddForce(-forceDirection * recoilF);
        addRecoil = false;
    }

    void movePlayer() {
        float currSpeed = Mathf.Abs(rb.velocity.x);
        float dirSpeed = rb.velocity.x;
        if (move != 0 && currSpeed < maxSpeed) {
            rb.AddForce(speed * Vector2.right * move);
            move = 0;
        }

        if (dirSpeed > 0.2f && move < 0) {
            rb.AddForce(speed * changeBoost * -Vector2.right);
        }
        if (dirSpeed < 0.2f && move > 0) {
            rb.AddForce(speed * changeBoost * Vector2.right);
        }
    }
}
