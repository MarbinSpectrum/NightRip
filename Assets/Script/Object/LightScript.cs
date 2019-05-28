using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public GameObject Light;
    void Update()
    {
        Light.SetActive(Vector2.Distance(transform.position, StageManager.camera_static.transform.position) < 4);
    }
}
