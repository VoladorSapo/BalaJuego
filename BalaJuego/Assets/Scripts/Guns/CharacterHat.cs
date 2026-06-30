using UnityEngine;

public class CharacterHat : MonoBehaviour
{
    [SerializeField] int currentLives;
    [SerializeField] int startingLives;
    [SerializeField] Animator animator;

    private void Start()
    {
    }
    //Returns true if no shield
    public bool getHit()
    {
        currentLives--;
        if (currentLives < 0)
        {
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