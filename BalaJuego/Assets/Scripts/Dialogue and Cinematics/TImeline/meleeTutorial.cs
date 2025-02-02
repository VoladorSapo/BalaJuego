using UnityEngine;

namespace tutorial
{
    public class meleeTutorial : Tutorial
    {
        [SerializeField] bulletDetector failDetector;
        protected override void Start()
        {
            print("setTUT");
            machine = new StateMachine();
            startTutorialState start = new startTutorialState(this);
            StartMeleeTutorial startMel = new StartMeleeTutorial(this);
            FinTutorial1State fin = new FinTutorial1State(this);
            idleTutorialState idle = new idleTutorialState(this);

            machine.AddTransition(start, startMel, new FuncPredicate(() => true));
          
            machine.AddTransition(startMel, idle, new FuncPredicate(() => changeTutWait == true));



        }
    }
}
