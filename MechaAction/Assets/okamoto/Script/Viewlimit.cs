using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewlimit : MonoBehaviour
{
    [SerializeField]
    public Vector2 cameraMin;
    public Vector2 cameraMax;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GManager.Instance.SetCameraBounds(cameraMin, cameraMax);
        }
    }
}
