using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalFace : MonoBehaviour
{
    
    
    private Renderer _rend;
    private float scrollSpeed = 0.5f;

    public bool _isPlayerInZone;

    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerInZone)
        {
            float offset = Time.time * scrollSpeed;
            _rend.material.SetTextureOffset("_MainTex",new Vector2(0,offset));
        }

    }
}
