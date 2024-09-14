using UnityEngine;

public class SaloonBottle : MonoBehaviour
{
    private Transform aimTarget;
    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform bottleContainer;

    private bool isHighlighted = false;
    private bool isCaught = false;

    public bool IsCaught { get { return isCaught; } }
    public bool IsHighlighted { get { return isHighlighted; } }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        aimTarget = SaloonPlayer.main.AimTransform;
    }

    public void Catch()
    {
        isCaught = true;
    }

    public void Hide()
    {
        bottleContainer.gameObject.SetActive(false);
    }
    public void Kill()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (isCaught)
        {
            return;
        }
        if (Mathf.Abs(aimTarget.position.x - transform.position.x) < maxDistance)
        {
            Highlight();
        }
        else
        {
            Unhighlight();
        }
    }

    public void Highlight()
    {
        if (isHighlighted)
        {
            return;
        }
        animator.Play("bottleHighlight");
        isHighlighted = true;
    }

    public void Unhighlight()
    {
        if (!isHighlighted)
        {
            return;
        }
        animator.Play("bottleIdle");
        isHighlighted = false;
    }
}
