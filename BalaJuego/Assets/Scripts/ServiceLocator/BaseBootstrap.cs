
using UnityEngine;

public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<ITimeManager>(FindObjectOfType<TimeManager>());
            ServiceLocator.Instance.Register<IGameState>(FindObjectOfType<gameStateManager>());
        ServiceLocator.Instance.Register<ILevelController>(FindObjectOfType<LevelController>());
        ServiceLocator.Instance.Register<IcutsceneManager>(FindObjectOfType<cutsceneManager>());


    }


}

