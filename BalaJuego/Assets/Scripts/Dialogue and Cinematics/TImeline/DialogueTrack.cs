using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Playables;
using UnityEngine;

[TrackBindingType(typeof (TMP_Text))]
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogueTrackMixer>.Create(graph, inputCount);
    }
}
