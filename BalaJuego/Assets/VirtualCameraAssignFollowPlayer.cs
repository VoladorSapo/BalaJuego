using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraAssignFollowPlayer : MonoBehaviour
{
    
    private void Awake()
    {
        GetComponent<CinemachineVirtualCamera>().LookAt = FindAnyObjectByType<PlayerMove>().transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
