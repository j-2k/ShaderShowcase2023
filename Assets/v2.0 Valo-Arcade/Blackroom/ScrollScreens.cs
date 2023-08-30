using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScreens : MonoBehaviour
{
    [SerializeField] GameObject[] tvScreens;
    public bool isScrollingTV = false;
    bool isWaiting = false;
    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if(isScrollingTV && !isWaiting)
        {
            isWaiting = true;
            isScrollingTV = false;
            if(index == 0)
            { tvScreens[index].SetActive(true); Invoke(nameof(DisableFirstScreen), 1); return; }

            tvScreens[index - 1].SetActive(false);
            tvScreens[index].SetActive(true);
            Invoke(nameof(DisableCurrentScreen), 1);
        }
    }

    void DisableCurrentScreen()
    {
        tvScreens[index].SetActive(false);
        index++;
        isWaiting = false;
        isScrollingTV = false;
        if (index > tvScreens.Length - 1)
        {
            this.enabled = false;
        }
    }

    void DisableFirstScreen()
    {
        tvScreens[index].SetActive(false);
        index++;
        isWaiting = false;
        isScrollingTV = false;
    }
}
