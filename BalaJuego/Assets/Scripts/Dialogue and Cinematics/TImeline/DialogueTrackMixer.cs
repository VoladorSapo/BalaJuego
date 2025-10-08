using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text text = playerData as TMP_Text;
        text.text = "";

        if (!text) { return; }

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if(inputWeight>0)
            {
                ScriptPlayable<DialogueBehaviour> inputPlayable = (ScriptPlayable<DialogueBehaviour>)playable.GetInput(i);
                DialogueBehaviour dialogue = inputPlayable.GetBehaviour();
                if (dialogue != null)
                {
                    text.text = dialogue.dialogTexts[dialogue.lang];

                    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(dialogue.width, 2);
                    text.ForceMeshUpdate();
                    text.ForceMeshUpdate();
                }


            }
        }
    }
}
