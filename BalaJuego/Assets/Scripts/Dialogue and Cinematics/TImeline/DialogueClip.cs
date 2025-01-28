using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public string dialogText;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

        DialogueBehaviour behaviour = playable.GetBehaviour();
        behaviour.dialogText = dialogText;
        return playable;

    }
}
