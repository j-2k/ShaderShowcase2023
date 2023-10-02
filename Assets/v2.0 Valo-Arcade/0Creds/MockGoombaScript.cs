using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockGoombaScript : MonoBehaviour
{
    [SerializeField] ArcadeDanceController startDance;
    [SerializeField] ParticleSystem pfxGoomba;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!startDance.isStartingAnim)
            {
                pfxGoomba.Play();

                startDance.isStartingAnim = true;
            }
        }
    }
}
