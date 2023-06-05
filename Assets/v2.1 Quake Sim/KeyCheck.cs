using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

class KeyHolder {
    KeyCode keyholderKEY;
    Image image;
    RectTransform rt;
    Color inactiveCol = Color.white;    //defaulted to white for inactive
    Color activeCol = Color.green;      //defaulted to green for active
    float downShiftAmount = -3;

    public KeyHolder()
    {
        Initialize(null,KeyCode.None);
    }

    public KeyHolder(GameObject go, KeyCode assignedKey)
    {
        Initialize(go, assignedKey);
    }

    public void Initialize(GameObject go,KeyCode assignedKey)
    {
        image = go.GetComponent<Image>();
        rt = go.GetComponent<RectTransform>();
        keyholderKEY = assignedKey;
    }

    public void ChangeColorStates(Color inactive, Color active)
    {
        inactiveCol = inactive;
        activeCol = active;
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

    public void LightImageWhenKeyPressed()
    {
        if(isActive())
        {
            image.color = activeCol;
            rt.anchoredPosition = new Vector2(0,downShiftAmount);
        }
        else
        {
            image.color = inactiveCol;
            rt.anchoredPosition = Vector2.zero;
        }
    }
}

public class KeyCheck : MonoBehaviour
{
    //PLANS FOR THIS SCRIPT
    //ADD A FUNCTIONALITY THAT CAN CHANGE KEY BINDS

    [SerializeField] Image[] keyImages = new Image[5];  //ORDER = W, A, S, D, SPACE
    [SerializeField] KeyCode[] keys = new KeyCode[5];   //ORDER = W, A, S, D, SPACE
    KeyHolder[] keyHolders = new KeyHolder[5];

    [SerializeField] Color inactiveCol;
    [SerializeField] Color activeCol;
    [SerializeField] bool isInputCustomColors = false;

    private void Start()
    {
        for (int i = 0; i < keyHolders.Length; i++)
        {
            //keyHolders[i].Initialize(keyImages[i].gameObject, keys[i]);
            keyHolders[i] = new KeyHolder(keyImages[i].gameObject, keys[i]);
        }

        if(isInputCustomColors)
        {
            for (int i = 0; i < keyHolders.Length; i++)
            {
                keyHolders[i].ChangeColorStates(inactiveCol, activeCol);
            }
        }

    }

    void Update()
    {
        for (int i = 0; i < keyHolders.Length; i++)
        {
            keyHolders[i].LightImageWhenKeyPressed();
        }
    }


    #region old key algo
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
    #endregion
}
