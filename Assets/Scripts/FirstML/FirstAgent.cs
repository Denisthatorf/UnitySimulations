using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FirstAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterail;
    [SerializeField] private Material loseMaterail;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3( Random.Range(1.0f, -2.0f), 0, Random.Range(-1.5f, +1.0f));
        targetTransform.localPosition= new Vector3( Random.Range(2.0f, -2.0f), 0, Random.Range(2.0f, 3.3f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        //base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 2f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Goal>(out Goal goal))
        {
            floorMeshRenderer.material = winMaterail;
            SetReward(1f);
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            floorMeshRenderer.material = loseMaterail;
            SetReward(-1.5f);
            EndEpisode();
        }

    }
}
