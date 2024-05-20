using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject nameBoss;
    [SerializeField] private Boss boss; // Referencia al script del jefe

    private BoxCollider2D bc2D;

    private void Start()
    {
        bossPanel.SetActive(false);
        nameBoss.SetActive(false);
        bc2D = wall.GetComponent<BoxCollider2D>();
        bc2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossPanel.SetActive(true);
            nameBoss.SetActive(true);
            bc2D.enabled = true;
            boss.ActivateBoss(); // Activar al jefe
        }
    }
}