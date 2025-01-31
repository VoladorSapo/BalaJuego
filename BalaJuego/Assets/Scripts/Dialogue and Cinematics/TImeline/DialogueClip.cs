using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    [TextArea(15, 20)]
    public string dialogText;
    public float leaveTime;
    public int startChars;
    public float width;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

        DialogueBehaviour behaviour = playable.GetBehaviour();
        behaviour.dialogText = dialogText;
        behaviour.leaveTime = leaveTime;
        behaviour.startChars = startChars;
        behaviour.width = width;
        return playable;

    }
}
