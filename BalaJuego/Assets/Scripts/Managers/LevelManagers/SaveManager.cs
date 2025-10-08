using System.IO;
using UnityEngine;

public class SaveManager : ISaveManager
{
    public string getSavedScene()
    {
        //string dir = Application.persistentDataPath + "/saveData.txt";
        //Debug.Log(dir);

        //if (File.Exists(dir))
        //{
        //    return File.ReadAllText(dir);
        //}
        return null;
    }

    public void Instantiate() { }

    public void saveGame(string sceneName)
    {
        //string dir = Application.persistentDataPath + "/saveData.txt";
        //Debug.Log(dir);
        //if (!File.Exists(dir))
        //{
        //    File.Create(dir);

        //}
        //Debug.Log("Escrito");

        ////File.Open(dir,FileMode.);
        //File.WriteAllText(dir, sceneName);
        
    }
}