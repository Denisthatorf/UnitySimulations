using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private TMP_Text scoreText;

    void Awake()
    {
        scoreText = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        float score = playerTransform.position.z;
        scoreText.text = score.ToString("0");
    }
}
