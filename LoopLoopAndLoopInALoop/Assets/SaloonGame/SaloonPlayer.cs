using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SaloonPlayer : MonoBehaviour
{
    public static SaloonPlayer main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private SaloonAimIndicator aimIndicator;
    [SerializeField]
    private SaloonThrownLasso thrownLasso;
    [SerializeField]
    private SaloonLassoTightener lassoTightener;

    [SerializeField]
    private SaloonLasso saloonLasso;

    [SerializeField]
    private Transform bottleContainer;

    [SerializeField]
    private Transform catchContainer;
    //private List<SaloonBottle> bottles;
    private List<SaloonBottle> caughtBottles;

    private SaloonBottle targetBottle;

    private bool isAiming = false;
    private bool isLassoing = false;
    private float difficulty = 1f;
    public Transform AimTransform { get { return aimIndicator.MovingTransform; } }
    void Start()
    {
        //bottles = bottleContainer.GetComponentsInChildren<SaloonBottle>().ToList();
        caughtBottles = new();
        if (GameManager.Instance != null)
        {
            difficulty = GameManager.Instance.GetDifficulty();
        }
        Debug.Log($"Difficulty is {difficulty}");
        aimIndicator.Initialize(difficulty);
        thrownLasso.Initialize(difficulty);
        lassoTightener.Initialize(difficulty);
    }

    void ResetLasso()
    {
        thrownLasso.Stop();
        thrownLasso.HideLoop();
        thrownLasso.ResetRope();
        saloonLasso.Stop();
        EnableAiming();
    }

    void Update()
    {
        if (!isLassoing)
        {
            HandleAiming();
            return;
        }
        if (thrownLasso.IsFinished && targetBottle == null)
        {
            Debug.Log("no bottle");
            ResetLasso();
            return;
        }
        if (targetBottle == null && Input.GetMouseButtonDown(0))
        {
            Debug.Log("no bottle2");
            ResetLasso();
            return;
        }
        if (!thrownLasso.IsFinished && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Good");
            if (thrownLasso.TargetWasHit)
            {
                Debug.Log("target was hit");
                thrownLasso.HideLoop();
                targetBottle.Catch();
                lassoTightener.Tighten(targetBottle, delegate
                {
                    Debug.Log("You caught the bottle!");
                    targetBottle.AnimateCatch(catchContainer);
                    caughtBottles.Add(targetBottle);
                    targetBottle = null;
                    thrownLasso.ResetRope();
                    saloonLasso.Stop();
                    EnableAiming();
                });
            }
            else
            {
                Debug.Log("You didn't hit the bottle!");
                ResetLasso();
            }
        }
        else if (thrownLasso.IsFinished && !targetBottle.IsCaught)
        {
            Debug.Log("Too late!");
            targetBottle.Kill();
            targetBottle = null;
            ResetLasso();
        }
    }

    public void EnableAiming()
    {
        isLassoing = false;
    }

    public void HandleAiming()
    {
        if (!isAiming && Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            saloonLasso.Whoop();
            aimIndicator.Move();
        }
        if (isAiming && Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            saloonLasso.Stop();
            Vector3 aimPos = aimIndicator.MovingTransform.position;
            aimIndicator.Stop();
            SaloonBottle hitBottle = SaloonManager.main.GetHighlightedBottle();
            saloonLasso.Hide();
            if (hitBottle != null)
            {
                targetBottle = hitBottle;
                HandleLassoing(hitBottle.transform.position);
            }
            else
            {
                HandleLassoing(aimPos);
            }
        }
    }

    public void HandleLassoing(Vector3 targetPos)
    {
        isLassoing = true;
        thrownLasso.Throw(targetPos);
    }

}
