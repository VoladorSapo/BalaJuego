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
            ServiceLocator.Instance.Get<ILevelController>().reStart();
            Destroy(gameObject);
        }
        public void startTutorial()
        {
            print("startTutorial");
            machine.SetState(new startTutorialState(this));
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
            tutorial.TutorialText.text = "Pulsa E para parar el Tiempo";
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
            tutorial.TutorialText.text = "Haz Click a la bala para recogerla";
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
            tutorial.TutorialText.text = "Enhorabuena";
            tutorial.changeTutWait = false;
            tutorial.waitTime(0.5f);
            
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
}
