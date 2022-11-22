using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float sidewaySpeed;
    [SerializeField] private GameManager gameManager;
    private Rigidbody playerRigidbody; 

    void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        playerRigidbody.AddForce(0, 0, forwardSpeed * Time.deltaTime);          

        if(Input.GetKey("d"))
            playerRigidbody.AddForce(sidewaySpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        if(Input.GetKey("a"))
            playerRigidbody.AddForce(-sidewaySpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    void OnTrigerEnter(Collider other)
    {
        Debug.Log(other.GetType());
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            gameManager.EndGame();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.GetType());
        //TODO: timer before game over
        if(other.collider.TryGetComponent<Barrier>(out Barrier barrier))
        {
            gameManager.EndGame();
        }
    }
}
