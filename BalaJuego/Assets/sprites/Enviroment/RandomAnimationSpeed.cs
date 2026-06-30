using UnityEngine;

public class RandomAnimationSpeed : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minSpeed = 0.3f;
    [SerializeField] private float maxSpeed = 0.5f;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null)
            animator.speed = Random.Range(minSpeed, maxSpeed);
    }
}