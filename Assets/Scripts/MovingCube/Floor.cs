using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private Material winMaterail;
    [SerializeField] private Material loseMaterail;
    [SerializeField] private Material defaultMaterail;

    public void Reset()
    {
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterail;
    }

    public void Win()
    {
        gameObject.GetComponent<MeshRenderer>().material = winMaterail;
    }

    public void Lose()
    {
        gameObject.GetComponent<MeshRenderer>().material = loseMaterail;
    }
}
