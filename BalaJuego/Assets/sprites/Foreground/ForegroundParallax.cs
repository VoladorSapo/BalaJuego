using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundParallax : MonoBehaviour
{
    [SerializeField] float parallaxEffect = 1.05f;
    [SerializeField] GameObject cam;

    Vector3 startPos;
    Vector3 cameraStartPos;

    // Start is called before the first frame update
    void Start()
    {
        //Cosa de Diego
        cam = Camera.main.gameObject;

        startPos = transform.position;
        cameraStartPos = cam.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraDelta = cam.transform.position - cameraStartPos;

        transform.position = new Vector3(
            startPos.x + cameraDelta.x * (parallaxEffect - 1f),
            startPos.y + cameraDelta.y * (parallaxEffect - 1f),
            transform.position.z);
    }
}