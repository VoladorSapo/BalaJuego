using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace tutorial
{
    public class Tutorial : MonoBehaviour
    {
      protected  StateMachine machine;

     public    TMP_Text TutorialText;

        public EnemyController enemy;

        public PlayerMove player;

        public Light2D light2d;

        public float darkLight;

        public bulletDetector grabDetector;

        public baseBullet bul;

       public bool changeTutWait;

        [SerializeField]        bool restart;

        private void Update()
        {
            machine.Update();
        }
        protected  virtual void Start()
        {
            machine = new StateMachine();

        }
        public void endTutorial()
        {
            if (restart)
            {
                ServiceLocator.Instance.Get<ILevelController>().reStart();
            }
            else
            {
                ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.NormalTime);
            }
            Destroy(gameObject);
        }
        public void startTutorial()
        {
            print("startTutorial");
            if (!settingManager.Instance.modeSpeedRun)
            {
                machine.SetState(new startTutorialState(this));
            }
            else
            {
                endTutorial();
            }
        }
        public void waitTime(float time)
        {
            StartCoroutine(IEwaitTime(time));
        }
         IEnumerator IEwaitTime(float time)
        {

            yield return new WaitForSeconds(time);
            changeTutWait = true;
        }
    }
    public abstract class BaseTutorialState : IState
    {
        protected Tutorial tutorial;
        public virtual void OnEnter()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        public virtual void OnExit()
        {

        }
    }

    public class startTutorialState : BaseTutorialState
    {
        public startTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Tutorial);

        }
    }
    public class pulsaETutorialState : BaseTutorialState
    {
        public pulsaETutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.TutorialText.text = "Tu revolver solo puede tener una bala. Pero cuando está SIN BALAS puedes pulsar E para REALENTIZAR EL TIEMPO";
            tutorial.enemy.anim.Play("enemySpot");
            tutorial.enemy.GetComponentInChildren<IShoot>().getAnim().Play("enemyGunSpot");
            tutorial.enemy.GetComponentInChildren<gunRotate>().setRotation(tutorial.player.transform.position);

            tutorial.light2d.intensity = tutorial.darkLight;
        }
        public override void OnExit()
        {
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.2f,true);
            tutorial.grabDetector.gameObject.SetActive(true);
            tutorial.enemy.GetComponentInChildren<IShoot>().shoot();
        }
    }
    public class clickBalaTutorialState : BaseTutorialState
    {
        public clickBalaTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.grabDetector.reachableObjects[0].getObj().GetComponent<baseBullet>().changeTimeMagnitude(this, new timeData(1,0));
            tutorial.TutorialText.text = "Cuando el tiempo está realentizado puedes hacer click en las BALAS de los enemigos para AGARRARLAS y así conseguir munición";
        }
        public override void OnExit()
        {
            tutorial.grabDetector.gameObject.SetActive(false);
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1f);


        }
    }
    public class DisparaATipoTutorialState : BaseTutorialState
    {
        public DisparaATipoTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.TutorialText.text = "Haz Click en el enemigo para disparar";
        }
        public override void OnExit()
        {

        }
    }
    public class FinTutorial1State : BaseTutorialState
    {
        public FinTutorial1State(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.TutorialText.text = "Los enemigos tienen BALAS LIMITADAS. Gestiona el tiempo y la munición o tendrás que REINICIAR EL NIVEL dandole a ESC";
            tutorial.changeTutWait = false;
            tutorial.waitTime(5f);
            
        }
        public override void OnExit()
        {
            tutorial.TutorialText.text = "";

            tutorial.endTutorial();
        }
    }
    public class idleTutorialState : BaseTutorialState
    {
        public idleTutorialState(Tutorial _tut)
        {
            tutorial = _tut;
        }
    }

    public class StartMeleeTutorial : BaseTutorialState
    {
        public StartMeleeTutorial(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            Debug.Log("guachamin");
            tutorial.TutorialText.text = "No siempre podrás llegar armado a las peleas. Los enemigos SIN BALAS pueden rematarse a CORTA DISTANCIA pulsando F.";
            tutorial.changeTutWait = false;
            tutorial.waitTime(6f);
            tutorial.enemy.GetComponent<TutorialLife>().blockKill = true;
            tutorial.enemy.anim.Play("enemyStun");


        }
        public override void OnExit()
        {
            tutorial.TutorialText.text = "";
            tutorial.enemy.GetComponent<TutorialLife>().blockKill = false;
            tutorial.endTutorial();
        }
    }
    public class StartBotelTutorial : BaseTutorialState
    {
        botelTutorial tuto;
        public StartBotelTutorial(botelTutorial _tut)
        {
            tuto = _tut;
        }
        public override void OnEnter()
        {

            tuto.TutorialText.text = "Las botellas pueden aturdir a los enemigos, los enemigos estuneados pueden remartarse a corta distancia";

            tuto.botela.gameObject.SetActive(true);
            tuto.changeTutWait = false;
            tuto.waitTime(0.5f);


        }
        public override void OnExit()
        {
        }
    }
    public class GrabBotelState : BaseTutorialState
    {
        public GrabBotelState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.TutorialText.text = "Haz click en la botella para agarrarla";
            tutorial.changeTutWait = false;
            tutorial.waitTime(0.5f);

        }
        public override void OnExit()
        {
        }
    }
    public class ThrowBotelState : BaseTutorialState
    {
        public ThrowBotelState(Tutorial _tut)
        {
            tutorial = _tut;
        }
        public override void OnEnter()
        {
            tutorial.TutorialText.text = "Haz click al enemigo para lanzar la botella y aturdirle";
            tutorial.changeTutWait = false;
            tutorial.waitTime(0.5f);

        }
        public override void OnExit()
        {
            tutorial.TutorialText.text = "";

            tutorial.endTutorial();
        }
    }

}
