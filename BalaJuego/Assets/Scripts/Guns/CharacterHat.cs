using UnityEngine;

public class CharacterHat : MonoBehaviour
{
    [SerializeField] int currentLives;
    [SerializeField] int startingLives;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem hatOffParticles;

    private void Start()
    {
    }
    //Returns true if no shield

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            setLives(Mathf.Abs(currentLives - 1));
            hatOffParticles.Play();
        }
    }
    public bool getHit()
    {
        currentLives--;
        if (currentLives < 0)
        {
            hatOffParticles.Play();
            return true;
        }
        if (animator)
        {
            setAnimator();
        }
        return false;
    }
    public void setLives(int lives)
    {
        currentLives = lives;
        if (animator)
        {
            setAnimator();
        }
    }
    public void resetHat()
    {
        currentLives = startingLives;
        if (animator)
        {
            setAnimator();
        }
    }
    void setAnimator()
    {
        animator.SetInteger("lives", currentLives);
    }

}