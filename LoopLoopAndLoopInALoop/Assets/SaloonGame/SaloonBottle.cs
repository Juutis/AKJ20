using System.Diagnostics;
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
    private bool isFailing = false;

    public bool IsCaught { get { return isCaught; } }
    public bool IsHighlighted { get { return isHighlighted; } }

    [SerializeField]
    private float moveDuration = 0.4f;
    private Vector3 startPos;
    private Vector3 targetPos;

    private float moveTimer = 0f;

    private bool isMoving = false;

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

    public void AnimateCatch(Transform target)
    {
        isMoving = true;
        startPos = transform.position;
        targetPos = target.position;
        moveTimer = 0f;
    }
    public void Kill()
    {
        isFailing = true;
        animator.Play("bottleFail");
    }

    public void AfterFail()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float moveDelta = moveTimer / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, moveDelta);
            if (moveDelta >= 1f)
            {
                transform.position = targetPos;
                bottleContainer.gameObject.SetActive(false);
                isMoving = false;
            }
            return;
        }
        if (isCaught || isFailing)
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
