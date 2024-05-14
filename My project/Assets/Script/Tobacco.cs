using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Tobacco : MonoBehaviour
{
    [SerializeField] private CountCigarrete count;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            count.AddCount(0.5f);
            Destroy(gameObject);
        }
    }
}
