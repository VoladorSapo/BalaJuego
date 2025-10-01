using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    [TextArea(15, 20)]
    public string dialogText;
    [TextArea(15, 20)]
    public string[] dialogTexts;

    public float leaveTime;
    public int startChars;
    public int[] startCharsList;
    public float width;
    public int personaje;
    public Language LanguageForEditor;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);

        DialogueBehaviour behaviour = playable.GetBehaviour();
        //behaviour.dialogText = dialogText;
        behaviour.leaveTime = leaveTime;
        behaviour.startChars = startChars;
        behaviour.width = width;
        behaviour.personaje = personaje;
        behaviour.dialogTexts = dialogTexts;
        behaviour.startCharsList = startCharsList;
        behaviour.languageForEditor = LanguageForEditor;
        return playable;

    }
}
