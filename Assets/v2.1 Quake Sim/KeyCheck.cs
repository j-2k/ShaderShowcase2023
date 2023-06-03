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

    private void Start()
    {
        if(!isInputCustomColors)
        {
            inactiveCol = new Color(0.2f, 0.2f, 0.2f, 1);
            activeCol = Color.green;
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
