using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

class KeyHolder {
    KeyCode keyholderKEY;
    Image image;
    RectTransform rt;
    KeyHolder(GameObject go, KeyCode assignedKey)
    {
        image = go.GetComponent<Image>();
        rt = go.GetComponent<RectTransform>();
        keyholderKEY = assignedKey;
    }

    public void Initialize(GameObject go,KeyCode assignedKey)
    {
        image = go.GetComponent<Image>();
        rt = go.GetComponent<RectTransform>();
        keyholderKEY = assignedKey;
    }

    public bool isActive()
    {
        if(Input.GetKey(keyholderKEY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class KeyCheck : MonoBehaviour
{

    KeyHolder[] kh = new KeyHolder[5];
    [SerializeField] Image[] keyImages = new Image[5]; //ORDER = W, A, S, D, SPACE
    KeyCode[] keys = new KeyCode[5];

    [SerializeField] Color inactiveCol;
    [SerializeField] Color activeCol;
    [SerializeField] bool isInputCustomColors = false;

    private void Start()
    {
        if(!isInputCustomColors)
        {
            inactiveCol = new Color(1f, 1f, 1f, 1);
            activeCol = Color.green;
        }

        for (int i = 0; i < kh.Length; i++)
        {
            kh[i].Initialize(keyImages[i].gameObject, keys[i]);
        }
    }

    void Update()
    {
        
    }



    /* old aglo pretty decent ngl revamped it completely tho
    [SerializeField] Image WImage;
    [SerializeField] Image AImage;
    [SerializeField] Image SImage;
    [SerializeField] Image DImage;
    [SerializeField] Image SpaceImage;

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
    */
}
