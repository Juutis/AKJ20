using UnityEngine;

public class SaloonLasso : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void Hide()
    {
        animator.Play("lassoHidden");
    }

    public void Whoop()
    {
        animator.Play("lassoWhoop");
    }

    public void Stop()
    {
        animator.Play("lassoIdle");
    }
}
