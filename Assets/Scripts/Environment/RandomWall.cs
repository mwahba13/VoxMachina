using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class RandomWall : MonoBehaviour
{

    [SerializeField] private List<Texture> textureBank;
    
    // Start is called before the first frame update
    void Start()
    {
        Material mat = GetComponent<Renderer>().material;

        mat.mainTexture = textureBank[Random.Range(0,textureBank.Count)];
    }


}
