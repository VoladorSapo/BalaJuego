using UnityEngine;

public class AnimatorLayerManager : MonoBehaviour
{
    [SerializeField] string[] pickfromLayers;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PickLayer(string targetLayer)
    {
        Debug.Log("PICK LAYER" + targetLayer);
        ResetPickfromLayers();
        anim.SetLayerWeight(anim.GetLayerIndex(targetLayer), 1);
    }
    
    public void ResetPickfromLayers()
    {
        foreach (var layer in pickfromLayers)
        {
            anim.SetLayerWeight(anim.GetLayerIndex(layer), 0);
        }
    }

     public void SwapAnim(RuntimeAnimatorController runtimeAnimatorController)
    {
        var info = anim.GetCurrentAnimatorStateInfo(0);
        int hash = info.fullPathHash;
        float t = info.normalizedTime;

        anim.runtimeAnimatorController = runtimeAnimatorController;
        anim.Play(hash, 0, t);
        anim.Update(0f);
    }
}
