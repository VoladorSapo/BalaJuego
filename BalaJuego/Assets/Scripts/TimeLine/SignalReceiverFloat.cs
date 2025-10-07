using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SignalReceiverFloat : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair
    {
        public SignalAsset signalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<float> { }
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        print("TESTTNotified");
        if (notification is ParameterizedEmitter<float> floatEmitter)
        {
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, floatEmitter.asset));
            foreach (var m in matches)
            {
                m.events.Invoke(floatEmitter.parameter);
            }
        }
    }
}

