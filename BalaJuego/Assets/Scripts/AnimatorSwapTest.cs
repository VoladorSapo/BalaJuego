using UnityEditor.Animations;
using UnityEngine;

public class AnimatorSwapTest : MonoBehaviour
{
    Animator agathaAnim;
    [SerializeField] RuntimeAnimatorController defaultAnim;
    [SerializeField] RuntimeAnimatorController customAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agathaAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (agathaAnim.runtimeAnimatorController == customAnim)
                SwapAnim(defaultAnim);
            else
                SwapAnim(customAnim);

        }
    }

    void SwapAnim(RuntimeAnimatorController runtimeAnimatorController)
    {
        var info = agathaAnim.GetCurrentAnimatorStateInfo(0);
        int hash = info.fullPathHash;
        float t = info.normalizedTime;

        agathaAnim.runtimeAnimatorController = runtimeAnimatorController;
        agathaAnim.Play(hash, 0, t);
        agathaAnim.Update(0f); 
    }
}
