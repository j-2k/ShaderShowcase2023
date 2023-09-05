using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    float t = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 0.1f;
       mat.SetTextureOffset("_MainTex", new Vector2(t, -0.1f));
    }
}
