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
        if (SoundManager.main != null)
        {
            SoundManager.main.PlayLoop(GameSoundType.Whoop);
        }
    }

    public void Stop()
    {
        animator.Play("lassoIdle");
        if (SoundManager.main != null)
        {
            SoundManager.main.StopLoop(GameSoundType.Whoop);
        }
    }
}
