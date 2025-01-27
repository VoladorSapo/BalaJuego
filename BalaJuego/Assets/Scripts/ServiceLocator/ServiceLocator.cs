using System.Collections.Generic;
using UnityEngine;


    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance;


        private Dictionary<string, IService> _services;

        public T Get<T>() where T : IService //Para acceder a los servicios (managers)
        {
            _services ??= new();
            return (T)_services.GetValueOrDefault(typeof(T).Name);
        }

        public void Register<T>(IService service) where T : IService
        {
            _services ??= new();
            string name = typeof(T).Name;
            if (_services.TryAdd(name, service))
            {
                service.Instantiate();
                return;
            }

            Debug.LogError($"Service {nameof(service)} not registered " +
                           $"due to service {_services[name]} already registered as {name}.");
        }
        virtual protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                print("bootstrap");
                GetComponent<IServiceBootstrap>().Bootstrap();
            }
            else
            {
                Destroy(this);
            }
        }
    
}