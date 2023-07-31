using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPairMats : MonoBehaviour
{
    public Material lightBase;
    public Material lightShaft;
    string alphaStr = "_Alpha";
    // Start is called before the first frame update
    void Awake()
    {
        lightBase = transform.GetChild(0).GetComponent<Renderer>().material;
        lightShaft = transform.GetChild(1).GetComponent<Renderer>().material;
        SetAlpha(0);
        this.enabled = false;
    }

    float tl = 0;
    // Update is called once per frame
    void Update()
    {
        tl += Time.deltaTime;
        if (tl <= 1)
        {
            SetAlpha(Mathf.Lerp(1,0,tl));
        }
        else
        {
            this.enabled = false;
        }
    }

    private void OnEnable()
    {
        tl = 0;
    }

    public void StartLightLerp()
    {
        tl = 0;
    }

    void SetAlpha(float alpha)
    {
        lightBase.SetFloat(alphaStr, alpha);
        lightShaft.SetFloat(alphaStr, alpha);
    }
}
