using UnityEngine;

namespace tutorial
{
    public class firstTutorial: Tutorial
    {
        protected override void Start()
        {
            print("setTUT");
            machine = new StateMachine();
            startTutorialState start = new startTutorialState(this);
            pulsaETutorialState pressE = new pulsaETutorialState(this);
            clickBalaTutorialState clickBala = new clickBalaTutorialState(this);
            idleTutorialState idle = new idleTutorialState(this);
            DisparaATipoTutorialState dispararTipo = new DisparaATipoTutorialState(this);
            FinTutorial1State fin = new FinTutorial1State(this);

            machine.AddTransition(start, pressE, new FuncPredicate(() => true));
            machine.AddTransition(pressE, idle, new FuncPredicate(() => Input.GetKeyDown(KeyCode.E)));
            machine.AddTransition(idle, clickBala, new FuncPredicate(() =>grabDetector.reachableObjects.Count > 0));
            machine.AddTransition(clickBala, dispararTipo, new FuncPredicate(() => grabDetector.reachableObjects.Count == 0));
            machine.AddTransition(dispararTipo, fin, new FuncPredicate(() => enemy == null));
            machine.AddTransition(fin, idle, new FuncPredicate(() => changeTutWait == true));



        }
    }
}
