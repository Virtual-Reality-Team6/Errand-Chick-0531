using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 interactionPosition;
    public Vector3 interactionRotation;

    private void Update()
    {
        // Player를 따라가는 카메라
        transform.position = player.position + interactionPosition;
        transform.LookAt(player.position);
        transform.rotation = Quaternion.Euler(interactionRotation);
    }
}
