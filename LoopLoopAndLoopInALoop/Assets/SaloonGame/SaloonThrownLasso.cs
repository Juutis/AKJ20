using System;
using Unity.VisualScripting;
using UnityEngine;

public class SaloonThrownLasso : MonoBehaviour
{
    [SerializeField]
    private Transform bottomPoint;
    [SerializeField]
    private Transform movingTransform;
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private SaloonLassoRope rope;

    private bool isThrowing = false;
    private bool isHolding = false;

    [SerializeField]
    private float duration = 0.8f;
    private float throwTimer = 0f;
    [SerializeField]
    private float holdDuration = 0.2f;
    private float holdDurationTimer = 0f;

    [SerializeField]
    private float yModifier = 20f;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private float maxDistance = 0.5f;

    public bool IsFinished { get { return !isThrowing && !isHolding; } }
    public bool TargetWasHit
    {
        get
        {
            if (IsFinished)
            {
                Debug.Log("is finished :(");
                return false;
            }
            float distance = Vector2.Distance(movingTransform.transform.position, targetPosition);
            if (distance < maxDistance)
            {
                return true;
            }
            return false;
        }
    }


    public void Initialize(float difficulty)
    {
        float modifier = 0.5f;
        duration -= modifier * difficulty;
        holdDuration -= modifier * difficulty;
        maxDistance -= modifier * difficulty;
    }

    public void ResetRope()
    {
        rope.Reset();
    }

    public void HideLoop()
    {
        movingTransform.gameObject.SetActive(false);
    }

    public void Throw(Vector3 targetPos)
    {
        isThrowing = true;
        movingTransform.transform.position = startPosition.position;
        movingTransform.gameObject.SetActive(true);
        startingPosition = movingTransform.position;
        targetPosition = targetPos;
        throwTimer = 0f;
        rope.Reset();
        rope.Move(bottomPoint);
    }

    public void Stop()
    {
        isThrowing = false;
        isHolding = false;
        rope.Stop();
    }

    void Update()
    {
        if (isHolding)
        {
            holdDurationTimer += Time.deltaTime;
            if (holdDurationTimer >= holdDuration)
            {
                isHolding = false;
            }
            return;
        }
        if (!isThrowing)
        {
            return;
        }
        throwTimer += Time.deltaTime;
        float timing = throwTimer / duration;
        startingPosition.y += yModifier * Time.deltaTime;
        movingTransform.position = Vector2.Lerp(startingPosition, targetPosition, timing);
        if (timing >= 1f)
        {
            movingTransform.position = targetPosition;
            isThrowing = false;
            isHolding = true;
            holdDurationTimer = 0f;
            rope.Stop();
        }
    }
}
