using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TMP_Text scoreText;

    void Awake()
    {
        scoreText = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        scoreText.text = playerTransform.position.z.ToString("0");
    }
}
