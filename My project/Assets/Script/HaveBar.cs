using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveBar : MonoBehaviour
{
    public static bool haveBar;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            haveBar = true;
            Destroy(gameObject);
        }
    }
}
