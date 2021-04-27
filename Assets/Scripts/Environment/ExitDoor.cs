using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    public Material _mat;
    public String nextLevel;
    
    
    private Vector2 offset = new Vector2();
    private Renderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
        offset.x = 0.0f;
        offset.y = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(nextLevel);
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += Time.deltaTime;
        offset.y += Time.deltaTime;
        
        _rend.material.SetTextureOffset("_MainTex",offset);
    }
}
