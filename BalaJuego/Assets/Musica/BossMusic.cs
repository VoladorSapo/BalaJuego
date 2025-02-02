using UnityEngine;

public class BossMusic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!musicManager.Instance.IsMusicPlaying())
            {
                musicManager.Instance.SetSong("boss");
                musicManager.Instance.SetPhase(0);
            }
        }
    }
}
