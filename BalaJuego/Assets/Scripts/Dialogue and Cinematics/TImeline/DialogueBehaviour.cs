using UnityEngine.Playables;
using TMPro;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;

public class DialogueBehaviour: PlayableBehaviour
{
    public string dialogText;
    public string[] dialogTexts;
    public float leaveTime;
    public int startChars;
    public int[] startCharsList;
    public Language languageForEditor;
    int maxVisible;
  public  float width;
    bool first = true;
    public int personaje;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int lang = 0;
        if (settingManager.Instance != null)
        {
            Debug.Log("findinstance");
            lang = (int)settingManager.Instance.getLanguage();
        }
        else
        {

            lang = (int)languageForEditor;
            Debug.Log("findeditor" + lang+" " + startCharsList.Length);
        }
        TMP_Text text = playerData as TMP_Text;
        text.ForceMeshUpdate();
        if (lang < startCharsList.Length)
        {
            text.text = dialogTexts[lang];
            Debug.Log(dialogTexts[lang]);
        }
        else
        {
            text.text = dialogText;
            Debug.Log("Fac");
        }
        //if (first)
        //{
        //    Debug.Log("chunda");
        //    first = false;
        //    text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 2);
        //}
        if (text != null)
        {
            int startCharacters;
            if (lang < startCharsList.Length) {
                 startCharacters = startCharsList[lang];
            }
            else
            {
                 startCharacters = startChars;
            }
                text.maxVisibleCharacters = startCharacters + Mathf.CeilToInt((text.textInfo.characterCount - startCharacters) * Mathf.Clamp(System.Convert.ToSingle(playable.GetTime() / (Mathf.Max(System.Convert.ToSingle(playable.GetDuration() - leaveTime), 0))), 0, 1));
            if(maxVisible != text.maxVisibleCharacters)
            {
                maxVisible = text.maxVisibleCharacters;
               if(maxVisible == startCharacters +1)
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
