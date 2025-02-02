using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ditherTransition : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float dither;
    Animator anim;
    string NextScene;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dither = 1;
        goOut();
    }
    public void goIn(string scene)
    {
        anim.Play("ditherIn");
        NextScene = scene;

    }
    public void finishIn()
    {
        SceneManager.LoadScene(NextScene);

    }
    public void finishOut()
    {
        ServiceLocator.Instance.Get<ILevelController>().trueStart();

    }
    public void goOut()
    {
        anim.Play("ditherOut");
    }
    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_dither", dither);
    }
}
