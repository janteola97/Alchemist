using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //using https://unity3d.com/learn/tutorials/projects/2d-ufo-tutorial/following-player-camera
    //TBH, i am not sure that this needs to be used anymore, since I just put the camera on the player, but it works, so this is staying for now 

    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
