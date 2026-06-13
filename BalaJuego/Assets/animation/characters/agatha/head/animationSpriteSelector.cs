using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSpriteSelector : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    public int frame;
void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Frame", (float)frame);
    }
}
