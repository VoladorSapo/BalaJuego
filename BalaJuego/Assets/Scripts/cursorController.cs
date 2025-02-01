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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        rb = GetComponent<Rigidbody2D>();
        shouldMove = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //pos.z = 0;
          Vector3  mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            pos = Vector2.Lerp(transform.position, mousePosition, speed);
            pos.z = 0;

        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(pos);
    }
    void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.SlowDown:
                slow();
                shouldMove = true;


                break;
            case IGameState.gameState.NormalTime:
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
            default:
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
