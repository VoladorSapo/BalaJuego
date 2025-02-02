using UnityEngine.Playables;
using TMPro;
using UnityEngine;

public class DialogueBehaviour: PlayableBehaviour
{
    public string dialogText;
    public float leaveTime;
    public int startChars;
    int maxVisible;
  public  float width;
    bool first = true;
    public int personaje;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        
        TMP_Text text = playerData as TMP_Text;
        text.ForceMeshUpdate();
        text.text = dialogText;
        //if (first)
        //{
        //    Debug.Log("chunda");
        //    first = false;
        //    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 2);
        //}
        Debug.Log(playable);
        if (text != null)
        {
            text.maxVisibleCharacters = startChars + Mathf.CeilToInt((text.textInfo.characterCount-startChars) * Mathf.Clamp(System.Convert.ToSingle(playable.GetTime() / (Mathf.Max(System.Convert.ToSingle(playable.GetDuration()-leaveTime),0))),0,1));
            if(maxVisible != text.maxVisibleCharacters)
            {
                maxVisible = text.maxVisibleCharacters;
               if(maxVisible == startChars +1)
                {
                    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (width, 2);
                    Debug.Log("PRIMERO");
                }
                if (text.textInfo.characterCount > 0)
                {
                    //Debug.Log(maxVisible + "Bip");

                    int numeroAleatorio = Random.Range(0, 3);

                    if (numeroAleatorio == 0)
                    {
                        if (personaje == 0) // Es la prota
                        {
                            musicManager.Instance.PlaySoundPitch("snd_voicemedium", 0.2f);
                        }
                        else if (personaje == 1) // Es el cura
                        {
                            musicManager.Instance.PlaySoundPitch("snd_voicehigh", 0.2f);
                        }
                        else // Otros
                        {
                            musicManager.Instance.PlaySoundPitch("snd_voicelow", 0.2f);
                        }
                    }


                }
            }
        }
        else
        {
            text.text = "";
        }
        //text.maxVisibleCharacters = text.textInfo.characterCount / playable.GetDuration
        
    }

    int QuienHabla()
    {
        // devolver 0 si es la prota, 1 si es el cura y 2 si son los malos
        return 1;
    }

}
