using UnityEngine;

namespace tutorial
{
    public class botelTutorial : Tutorial
    {
        PlayerShoot Pshoot;
        [SerializeField] botleDetector failDetector;
       public botella botela;
        protected override void Start()
        {
            print("setTUT");
            machine = new StateMachine();
            startTutorialState start = new startTutorialState(this);
            StartBotelTutorial startBotel = new StartBotelTutorial(this);
            GrabBotelState grabBotel = new GrabBotelState(this);
            ThrowBotelState throwBotel = new ThrowBotelState(this);
            idleTutorialState idle = new idleTutorialState(this);

            machine.AddTransition(start, startBotel, new FuncPredicate(() => true));
            machine.AddTransition(startBotel, grabBotel, new FuncPredicate(() => changeTutWait == true));
            machine.AddTransition(grabBotel, throwBotel, new FuncPredicate(() =>Pshoot.hasBottle == true));
            machine.AddTransition(throwBotel, idle, new FuncPredicate(() => enemy.stateMachine.currentState().ToString() == "StunedState"));

            machine.AddTransition(throwBotel, startBotel, new FuncPredicate(() => failDetector.reachableObjects.Count > 0));



        }
    }
}
