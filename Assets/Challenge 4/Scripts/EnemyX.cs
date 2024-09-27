using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject playerGoal;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");

    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        //try
        //{
        //    Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        //    //enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        //    enemyRb.AddForce(lookDirection * speed);
        //    Debug.Log("initiate enemy movement");
        //}
        //catch (NullReferenceException ex)
        //{
        //    Debug.LogError($"Object reference issue: {ex.Message}");
        //}

        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);




    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }

    }

}
