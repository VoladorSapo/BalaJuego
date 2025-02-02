using System;
using tutorial;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class cutsceneCaller : MonoBehaviour
{
    [SerializeField] public postCutsceneAction actionType;

    [SerializeField] public Tutorial _tutorial;

    [SerializeField] string nextScene;

    public PlayableAsset timeline;

    [SerializeField] bool playOnAwakeNoLevel = false;

   // [SerializeField] bool intermediateCutscene = false;

    [SerializeField] Transform startPos;

    PlayerMove player;

    [SerializeField] bool onTrigger;

  [SerializeField]  bool hasStarted, hasFinished;

    private void Start()
    {
        hasStarted = false;
        hasFinished = false;
        if (playOnAwakeNoLevel)
        {
            hasStarted = hasFinished = true;
            PlayCutscene();
        }
    }
    private void Update()
    {
        if(hasStarted && !hasFinished)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 obj = new Vector3(startPos.position.x, playerPos.y, playerPos.z);
            player.transform.position = Vector3.MoveTowards(playerPos, obj,player.maxSpeed / 2* Time.deltaTime);
            if(Vector3.Distance(player.transform.position,obj) < 0.5f)
            {
                hasFinished = true;
                PlayCutscene();
            }
        }
    }
    public void PlayCutscene()
    {

        Action _action = () => { };
        switch (actionType)
        {
            case postCutsceneAction.Tutorial:
                print("preparaTutorial");
                _action = () => { _tutorial.startTutorial(); };
                break;
            case postCutsceneAction.StartLevel:
                _action = () => { ServiceLocator.Instance.Get<ILevelController>().reStart(); };
                break;
            case postCutsceneAction.Continue:
                _action = () => { ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.NormalTime); };
                break;
            case postCutsceneAction.changeScene:
                _action = () => { SceneManager.LoadScene(nextScene); };
                break;
            default:
                break;
        }

        ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(timeline, _action);
    }



    public enum postCutsceneAction
    {
        Tutorial,
        StartLevel,
        Continue,
        changeScene
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasStarted && onTrigger && collision.tag == "Player")
        {
            ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Cinematic);
            hasStarted = true;
            player = collision.GetComponent<PlayerMove>();
            int dir = player.transform.position.x > startPos.position.x ? 1 : -1;
            player.anim.SetFloat("velocity", dir);
            player.anim.SetBool("isRunning", true);
            player.anim.SetBool("direction", player.transform.position.x > startPos.position.x);


        }

    }
}
