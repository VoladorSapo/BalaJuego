using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
}
