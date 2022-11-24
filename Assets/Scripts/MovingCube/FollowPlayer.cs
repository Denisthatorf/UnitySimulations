using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        transform.position = playerTransform.position + offset;
    }
}
