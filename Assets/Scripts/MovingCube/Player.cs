using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Player : Agent
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float sidewaySpeed;
    [SerializeField] private Floor floor;
    private Rigidbody playerRigidbody;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3( 0, 1.2f, 1);
        //floor.Reset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //base.CollectObservations(sensor);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }
        
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        playerRigidbody.AddForce(0, 0, forwardSpeed * Time.deltaTime);          
        playerRigidbody.AddForce(moveX * sidewaySpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

        if(((int)transform.localPosition.z) % 20 == 0)
        {
            floor.Win();
            AddReward(+0.2f); 
        }

        //transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    // ------ MonoBehaiviour Logic -------
    private void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            floor.Lose();
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.TryGetComponent<Barrier>(out Barrier barrier))
        {
            floor.Lose();
            SetReward(-1.0f);
            EndEpisode();
        }
    }
}
