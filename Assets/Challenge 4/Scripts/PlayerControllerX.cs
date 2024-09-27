using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 600;
    private float boostSpeed = 700;
    private GameObject focalPoint;


    public GameObject smokeParticlePrefab;

    public bool hasPowerup;
    private bool canBoost = true; //allow for player boosting
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public float boostDuration = 0.5f;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point"); 
      
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        PlayerBoost();

    

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);

            StartCoroutine(PowerupCooldown());
        }
        
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            //Vector3 awayFromPlayer =  transform.position - other.gameObject.transform.position; 
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }


    private void PlayerBoost()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canBoost)
        {
            Debug.Log("Space bar key was pressed");
            if(smokeParticlePrefab != null)
            {
               GameObject smokeInstance = Instantiate(smokeParticlePrefab, transform.position, focalPoint.transform.rotation);
                ParticleSystem particleSystem = smokeInstance.GetComponent<ParticleSystem>();
                if(particleSystem != null)
                {
                    particleSystem.Play();
                    Destroy(smokeInstance, particleSystem.main.duration);
                }
                
                //smokeParticle.transform.position = transform.position + focalPoint.transform.position;
                //smokeParticlePrefab.Play();
                //playerRb.AddForce(focalPoint.transform.forward * boostSpeed * Time.deltaTime, ForceMode.Impulse);

            }
            else
            {
                Debug.LogWarning("Smoke Particle is not assigbed!");
            }

            playerRb.AddForce(focalPoint.transform.forward * boostSpeed * Time.deltaTime, ForceMode.Impulse);

            Debug.Log("Drastic push forward");
            StartCoroutine(BoostCooldown());
        }
    }

    // Coroutine to count down player boost
    IEnumerator BoostCooldown()
    {
        canBoost = false;
        Debug.Log("Boost cooldown started");
        yield return new WaitForSeconds(boostDuration); //wait for half a second before next boost to avoid spaming
        canBoost = true;
        Debug.Log("Boost cooldown ended");
    }


}
