using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform myPlayerHead;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // LateUpdate is called once per frame after first Update call
    private void LateUpdate()
    {
        CamMovment();
    }

    private void CamMovment()
    {
        transform.position = myPlayerHead.position;
        transform.rotation = myPlayerHead.rotation;
    }
}
