using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour
{

    public GameObject UICanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            UICanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        UICanvas.SetActive(false);
    }

}
