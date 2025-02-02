using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ditherTransition : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float dither;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void goIn()
    {

    }
    public void goOut()
    {

    }
    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_dither", dither);
    }
}
