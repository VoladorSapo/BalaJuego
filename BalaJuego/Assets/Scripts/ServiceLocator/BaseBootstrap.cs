
using UnityEngine;

public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<ITimeManager>(FindAnyObjectByType<TimeManager>());
            ServiceLocator.Instance.Register<IGameState>(FindAnyObjectByType<gameStateManager>());
        ServiceLocator.Instance.Register<ILevelController>(FindAnyObjectByType<LevelController>());
        ServiceLocator.Instance.Register<IcutsceneManager>(FindAnyObjectByType<cutsceneManager>());
        ServiceLocator.Instance.Register<IShadowManger>(FindAnyObjectByType<ShadowManager>());

        ServiceLocator.Instance.Register<IsoftLock>(FindAnyObjectByType<SoftLockController>());
        ServiceLocator.Instance.Register<ISaveManager>(new SaveManager());
        ServiceLocator.Instance.Register<ITimer>(FindAnyObjectByType<TimerManager>());
            ServiceLocator.Instance.Register<IHitStop>(FindAnyObjectByType<HitStopManager>());


    }


}

