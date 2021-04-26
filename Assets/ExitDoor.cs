using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public Material _mat;

    private Vector2 offset = new Vector2();
    private Renderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.x = Mathf.Sin(Time.deltaTime*3.0f);
        offset.y = Mathf.Sin(Time.deltaTime*3.0f);
        
        _rend.material.SetTextureOffset("_MainTex",offset);
    }
}
