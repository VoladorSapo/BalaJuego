using UnityEngine.Playables;
using TMPro;
using UnityEngine;

public class DialogueBehaviour: PlayableBehaviour
{
    public string dialogText;
    public float leaveTime;
    public int startChars;
    int maxVisible;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text text = playerData as TMP_Text;
        text.ForceMeshUpdate();
        text.text = dialogText;
        Debug.Log(playable);
        if (text != null)
        {
            text.maxVisibleCharacters = startChars + Mathf.CeilToInt((text.textInfo.characterCount-startChars) * Mathf.Clamp(System.Convert.ToSingle(playable.GetTime() / (Mathf.Max(System.Convert.ToSingle(playable.GetDuration()-leaveTime),0))),0,1));
            if(maxVisible != text.maxVisibleCharacters)
            {
                maxVisible = text.maxVisibleCharacters;
               if(maxVisible == startChars +1)
                {
                    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (11, 2);
                    Debug.Log("PRIMERO");
                }
                if (text.textInfo.characterCount > 0)
                {
                    Debug.Log(maxVisible + "Bip");
                }
            }
        }
        else
        {
            text.text = "";
        }
        //text.maxVisibleCharacters = text.textInfo.characterCount / playable.GetDuration
    }

}
