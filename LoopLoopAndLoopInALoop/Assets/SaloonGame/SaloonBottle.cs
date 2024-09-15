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

    [SerializeField]
    private float lifeTime = 5f;
    private float lifeTimer = 0;

    private bool isMoving = false;
    private bool isInitialized = false;

    public void Initialize(float difficulty)
    {
        lifeTime -= difficulty * 0.4f;
        aimTarget = SaloonPlayer.main.AimTransform;
        animator.Play("bottleInit");
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
        if (SoundManager.main != null)
        {
            SoundManager.main.PlaySound(GameSoundType.Bottle);
        }
    }
    public void Kill()
    {
        if (SoundManager.main != null)
        {
            SoundManager.main.PlaySound(GameSoundType.FallingBottle);
        }
        isFailing = true;
        animator.Play("bottleFail");
    }

    public void AfterFail()
    {
        SaloonManager.main.BreakBottle(this);
        Destroy(gameObject);
    }

    public void AfterInit()
    {
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float moveDelta = moveTimer / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, moveDelta);
            if (moveDelta >= 1f)
            {
                transform.position = targetPos;
                SaloonManager.main.GainBottle(this);
                bottleContainer.gameObject.SetActive(false);
                isMoving = false;
            }
            return;
        }
        if (isCaught || isFailing)
        {
            return;
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Kill();
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
        if (!isInitialized || isHighlighted)
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
