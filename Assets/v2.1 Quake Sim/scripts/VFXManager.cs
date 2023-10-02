using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] Quake1Move moveData;
    [SerializeField] ParticleSystem slideParticlesDistance;
    [SerializeField] ParticleSystem slideParticlesBurst;
    [SerializeField] GameObject TrailAfterShadowManager;

    ParticleSystem.EmissionModule slideDistanceEmission;

    // Start is called before the first frame update
    void Start()
    {
        slideDistanceEmission = slideParticlesDistance.emission;
    }

    [SerializeField] bool isAirLastFrame = false;

    // Update is called once per frame
    void Update()
    {
        if(moveData.currentSpeed > 15)
        {
            if(moveData.isSlidingCheck())
            {
                TrailAfterShadowManager.SetActive(false);
                if(!slideParticlesDistance.isEmitting)
                {
                    slideParticlesDistance.Play();
                }
            }

            if(!moveData.isGroundedCheck())
            {
                slideParticlesDistance.Stop();
                TrailAfterShadowManager.SetActive(true);
                isAirLastFrame = true;
            }

            if (isAirLastFrame && moveData.isGroundedCheck())
            {
                isAirLastFrame = false;
                if (!slideParticlesDistance.isEmitting)
                {
                    slideParticlesBurst.Play();
                }
            }
        }
        else
        {
            if (moveData.isGroundedCheck())
            {
                isAirLastFrame = false;
            }
        }
    }
}
