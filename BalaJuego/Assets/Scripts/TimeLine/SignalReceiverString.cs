using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class SignalReceiverString : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair
    {
        public SignalAsset signalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<string> { }
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        print("TESTTNotified");
        if (notification is ParameterizedEmitter<string> stringEmitter)
        {
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, stringEmitter.asset));
            foreach (var m in matches)
            {
                m.events.Invoke(stringEmitter.parameter);
            }
        }
    }
}
