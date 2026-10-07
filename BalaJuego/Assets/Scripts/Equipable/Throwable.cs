using UnityEngine;

public class Throwable : ABaseEquipable, IEquipable
{
    public override void Action(CharacterInteractor shooter, float angle)
    {
        shooter.GetComponent<PlayerInteractor>().throwObject();
        transform.parent = null;
        musicManager.Instance.PlaySoundPitch("snd_lanzabotella");
    }
}
