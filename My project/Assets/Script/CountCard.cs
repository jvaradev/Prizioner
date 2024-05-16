using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountCard : MonoBehaviour
{
    public static float count;
    private TextMeshProUGUI textMesh;
    [SerializeField] private Image card;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.enabled = false;
        card.enabled = false;
    }

    private void Update()
    {
        if (count > 0)
        {
            card.enabled = true;
        }

        if (count <= 0)
        {
            card.enabled = false;
        }
    }

    public void AddCount(float addNum)
    {
        count += addNum;
    }
    
}
