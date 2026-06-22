using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorController : MonoBehaviour
{
    bool shouldMove;
    Animator anim;
    int bulets;
    Rigidbody2D rb;
    Vector3 pos;
    [SerializeField] float speed;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        rb = GetComponent<Rigidbody2D>();
        shouldMove = false;
        sprite = GetComponent<SpriteRenderer>();
        FindAnyObjectByType<PlayerShoot>().subscribeToPlayerGunChange(playerGunChange);
    }

    private void playerGunChange(characterGunChangeData data)
    {
        if (data.hasSomething)
        {
            full();

        }
        else
        {
            empty();
        }
    }

    private void LateUpdate()
    {

        if (shouldMove)
        {
            //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //pos.z = 0;
          Vector3  mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = Vector3.Lerp(transform.position,mousePosition,speed*Time.deltaTime);
        }

    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (shouldMove)
    //    {
    //        //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        //pos.z = 0;
    //      Vector3  mousePosition = Input.mousePosition;
    //        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //        pos = Vector2.Lerp(transform.position, mousePosition, speed);
    //        pos.z = 0;

    //    }
    //}
    //private void FixedUpdate()
    //{
    //    rb.MovePosition(pos);
    //}
    void changeState(object sender, stateData data)
    {
        sprite.enabled = true;
        switch (data.currentState)
        {
            case IGameState.gameState.SlowDown:
                Cursor.visible = false;

                slow();
                shouldMove = true;


                break;
            case IGameState.gameState.NormalTime:
                Cursor.visible = false;

                shouldMove = true;

                if (bulets>0)
                {
                    full();
                }
                else
                {
                    empty();
                }
                break;
            case IGameState.gameState.Tutorial:
                Cursor.visible = false;

                if (bulets > 0)
                {
                    full();
                }
                else
                {
                    empty();
                }
                shouldMove = true;
                break;
            case IGameState.gameState.Cinematic:
                Cursor.visible = false;

                break;
            default:
                sprite.enabled = false;
                Cursor.visible = true;

                shouldMove = false;
                break;
        }
    }
    public void full()
    {
        bulets = 1;
        anim.Play("full");
    }
    public void empty()
    {
        bulets = 0;
        anim.Play("empty");

    }
    public void slow()
    {
        anim.Play("slow",-1,0);

    }
    private void OnMouseOver()
    {
        print("over");
    }

    public void endSlow()
    {
        ServiceLocator.Instance.Get<ITimeManager>().endSlow();
    }


}
