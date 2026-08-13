
using UnityEngine;

public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<ITimeManager>(FindObjectOfType<TimeManager>());
            ServiceLocator.Instance.Register<IGameState>(FindObjectOfType<gameStateManager>());
        ServiceLocator.Instance.Register<ILevelController>(FindObjectOfType<LevelController>());
        ServiceLocator.Instance.Register<IcutsceneManager>(FindObjectOfType<cutsceneManager>());

        ServiceLocator.Instance.Register<IsoftLock>(FindObjectOfType<SoftLockController>());
        ServiceLocator.Instance.Register<ISaveManager>(new SaveManager());
        ServiceLocator.Instance.Register<ITimer>(FindObjectOfType<TimerManager>());
            ServiceLocator.Instance.Register<IHitStop>(FindObjectOfType<HitStopManager>());


    }


}

