using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public GameObject bullet, bulletSpawn;
    GameObject bullets, sphere, player;
    public GameObject knife;
    private PlayerMovement playerScript;
    public float recoilForce = 10f;
    private Knife knifeScript;
    bool allowFire = true;
    public bool knifeThrown = false;
    public float fireRate = 1f;

    // Joystick Controls
    public string gunRight = "MouseXP1";
    public string gunDown = "MouseYP1";
    public string fireAxis = "Fire1P1";
    public string knifeAxis = "Fire2P1";
    // May or may not need gun to control which side gets shot


    void Start() {        
        knifeScript = knife.GetComponent<Knife>(); // not sure what to use this for
        bullets = GameObject.FindWithTag("bulletHolder");
        player = transform.parent.parent.gameObject;
        sphere = transform.parent.gameObject;
        playerScript = player.GetComponent<PlayerMovement>();

        if (player.name == "Player2") {
            gunRight = "MouseXP2";
            gunDown = "MouseYP2";
            fireAxis = "Fire1P2";
            knifeAxis = "Fire2P2";
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // Fire bullet according to fire rate only
        float shootingAxis = Input.GetAxis(fireAxis);
        if (shootingAxis > 0 && allowFire) {
            StartCoroutine("FireBullet");            
        }

        // Throw knife(can only be done once per round)
        float knifeThrow = Input.GetAxis(knifeAxis);
        if (knifeThrow > 0 && !knifeThrown) {            
            Instantiate(knife, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bullets.transform);
            knifeThrown = true;
        }

        // figures out rotation of gun based on mouse position input
        /*Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        sphere.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        float vectorRight = Input.GetAxis(gunRight);
        float vectorDown = -Input.GetAxis(gunDown);

        Vector2 dirToFace = new Vector2(vectorRight, vectorDown);
        dirToFace = dirToFace.normalized;
        float angle = Mathf.Atan2(dirToFace.y, dirToFace.x) * Mathf.Rad2Deg;
        sphere.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Fires a bullet as per the fire rate
    IEnumerator FireBullet() {
        allowFire = false;
        Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bullets.transform);
        playerScript.recoil(recoilForce);
        yield return new WaitForSeconds(fireRate);
        allowFire = true;
    }
}
