public interface ISaveManager:IService
{
    public string getSavedScene();

    public void saveGame(string sceneName);
}
