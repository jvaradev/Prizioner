using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountCigarrete : MonoBehaviour
{
    public static float count;
    private TextMeshProUGUI textMesh;
    [SerializeField] private Image cigarrete;
    [SerializeField] private Image circle;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.enabled = false;
        cigarrete.enabled = false;
        circle.enabled = false;
    }

    private void Update()
    {
        if (count > 0)
        {
            cigarrete.enabled = true;
            textMesh.enabled = true;
            textMesh.text = count.ToString();
            circle.enabled = true;
        }

        if (count <= 0)
        {
            cigarrete.enabled = false;
            textMesh.enabled = false;
            circle.enabled = false;
        }
    }

    public void AddCount(float addNum)
    {
        count += addNum;
    }
    
}
