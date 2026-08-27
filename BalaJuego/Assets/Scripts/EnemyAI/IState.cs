
public interface IState
{
    void OnEnter();
    void Update();
    bool ShouldEnd();
    void FixedUpdate();
    void OnExit();



}