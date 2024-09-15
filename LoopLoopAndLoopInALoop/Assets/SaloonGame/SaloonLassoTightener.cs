using UnityEngine;
using UnityEngine.Events;

public class SaloonLassoTightener : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private UnityAction afterTightenCallback;

    public void Initialize(float difficulty)
    {
        animator.speed = 1 + difficulty * 0.5f;
    }
    public void Tighten(SaloonBottle bottle, UnityAction action)
    {
        transform.position = bottle.transform.position;
        afterTightenCallback = action;
        animator.Play("lassoTighten");
    }

    public void TightenEnd()
    {
        Debug.Log("bottle gained!");
        animator.Play("lassoTightenerIdle");
        afterTightenCallback();
    }
}
