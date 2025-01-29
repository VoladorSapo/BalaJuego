using UnityEngine.Playables;
using TMPro;
using UnityEngine;

public class DialogueBehaviour: PlayableBehaviour
{
    public string dialogText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text text = playerData as TMP_Text;
        text.text = dialogText;
        text.ForceMeshUpdate();
        if(text != null)
        {
           Debug.Log(playable.GetDuration());
            Debug.Log(playable.GetTime());
            Debug.Log("clip");

        }
        //text.maxVisibleCharacters = text.textInfo.characterCount / playable.GetDuration
    }
}
