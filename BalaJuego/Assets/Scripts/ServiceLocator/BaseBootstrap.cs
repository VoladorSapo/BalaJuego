
using UnityEngine;

public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<ITimeManager>(new TimeManager());
            ServiceLocator.Instance.Register<IGameState>(FindObjectOfType<gameStateManager>());

    }


}

