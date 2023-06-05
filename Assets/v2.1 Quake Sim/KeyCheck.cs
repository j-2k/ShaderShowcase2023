using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class KeyCheck : MonoBehaviour
{
    [SerializeField] Image WImage;
    [SerializeField] Image AImage;
    [SerializeField] Image SImage;
    [SerializeField] Image DImage;
    [SerializeField] Image SpaceImage;
    [SerializeField] Color inactiveCol;
    [SerializeField] Color activeCol;
    [SerializeField] bool isInputCustomColors = false;
    RectTransform[] rts = new RectTransform[5];
    //GameObject[] images =

    private void Start()
    {
        if(!isInputCustomColors)
        {
            inactiveCol = new Color(1f, 1f, 1f, 1);
            activeCol = Color.green;
        }
        for (int i = 0; i < rts.Length; i++)
        {
            rts[i].GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LightImageWhenKeyDown(KeyCode.W, WImage);
        LightImageWhenKeyDown(KeyCode.A, AImage);
        LightImageWhenKeyDown(KeyCode.S, SImage); 
        LightImageWhenKeyDown(KeyCode.D, DImage);
        LightImageWhenKeyDown(KeyCode.Space, SpaceImage);
    }

    void LightImageWhenKeyDown(KeyCode key, Image image)
    {
        if(Input.GetKey(key))
        {
            image.color = activeCol;
        }
        else
        {
            image.color = inactiveCol;
        }
    }
}
