using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraControls : MonoBehaviour
{
    [SerializeField] KeyCode restartKey;
    [SerializeField] Vector3 respawnPos;
    [SerializeField] Quake1Move playerScript;


    // Start is called before the first frame update
    void Start()
    {
        respawnPos = GameObject.FindGameObjectWithTag("Respawn1").transform.position;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Quake1Move>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(restartKey))
        {
            playerScript.TeleportPlayer(respawnPos);
        }
    }
}
