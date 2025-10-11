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
    [SerializeField] bool isEndLevel = false;


    [SerializeField] bool playOnAwakeNoLevel = false;

   // [SerializeField] bool intermediateCutscene = false;

    [SerializeField] Transform startPos;

    PlayerMove player;

    [SerializeField] bool onTrigger;
    [SerializeField] bool canSkipped;

  [SerializeField]  bool hasStarted, hasFinished;

    [SerializeField] GameObject[] objectsTurnOff;
    [SerializeField] bool playOnRestart = false;
    


    private void Start()
    {
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);

        hasStarted = false;
        hasFinished = false;
        if (playOnAwakeNoLevel)
        {
            hasStarted = hasFinished = true;
            PlayCutscene();
        }
        if (actionType == postCutsceneAction.changeScene)
        {
            isEndLevel = true;
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
    public void restart()
    {
        if (playOnRestart)
        {
            hasStarted = false;
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
                _action = () => {FindObjectOfType<ditherTransition>().goIn(nextScene); };
                break;
            case postCutsceneAction.continuewithbossmusic:
                print("boos music");
                _action = () => { ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.NormalTime);
                    print("empezar boss");
                    musicManager.Instance.SetSong("boss");
                    musicManager.Instance.SetPhase(0);
                };
                break;
            default:
                break;
        }
        CutsceneData data = new CutsceneData(canSkipped,isEndLevel,objectsTurnOff);
        ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(timeline, _action,data);
    }



    public enum postCutsceneAction
    {
        Tutorial,
        StartLevel,
        Continue,
        changeScene,
        continuewithbossmusic
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
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        }

    }
}