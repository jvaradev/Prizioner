using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveGun : MonoBehaviour
{
    public static bool haveGun;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            haveGun = true;
            Destroy(gameObject);
        }
    }
}
