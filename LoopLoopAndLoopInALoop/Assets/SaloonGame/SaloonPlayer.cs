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
    private List<SaloonBottle> bottles;
    private List<SaloonBottle> caughtBottles;

    private SaloonBottle targetBottle;

    private bool isLassoing = false;

    public Transform AimTransform { get { return aimIndicator.MovingTransform; } }
    void Start()
    {
        bottles = bottleContainer.GetComponentsInChildren<SaloonBottle>().ToList();
        caughtBottles = new();
    }

    void Update()
    {
        if (!isLassoing)
        {
            HandleAiming();
            return;
        }
        if (!thrownLasso.IsFinished && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Good");
            if (thrownLasso.BottleWasHit)
            {
                Debug.Log("bottle was hit");
                thrownLasso.HideLoop();
                targetBottle.Catch();
                lassoTightener.Tighten(targetBottle, delegate
                {
                    Debug.Log("You caught the bottle!");
                    targetBottle.Hide();
                    caughtBottles.Add(targetBottle);
                    bottles.Remove(targetBottle);
                    thrownLasso.ResetRope();
                    saloonLasso.Stop();
                    EnableAiming();
                });
            }
            else
            {
                thrownLasso.Stop();
                thrownLasso.HideLoop();
                thrownLasso.ResetRope();
                saloonLasso.Stop();
                Debug.Log("You didn't hit the bottle!");
                EnableAiming();
            }
        }
        else if (thrownLasso.IsFinished && !targetBottle.IsCaught)
        {
            Debug.Log("Too late!");
            thrownLasso.Stop();
            thrownLasso.HideLoop();
            thrownLasso.ResetRope();
            saloonLasso.Stop();
            targetBottle.Kill();
            EnableAiming();
        }
    }

    public void EnableAiming()
    {
        isLassoing = false;
    }

    public void HandleAiming()
    {
        if (Input.GetMouseButtonDown(0))
        {
            saloonLasso.Whoop();
            aimIndicator.Move();
        }
        if (Input.GetMouseButtonUp(0))
        {
            saloonLasso.Stop();
            aimIndicator.Stop();
            SaloonBottle hitBottle = bottles.Find(bottle => bottle.IsHighlighted);
            if (hitBottle != null)
            {
                saloonLasso.Hide();
                HandleLassoing(hitBottle);
            }
        }
    }

    public void HandleLassoing(SaloonBottle bottle)
    {
        targetBottle = bottle;
        isLassoing = true;
        thrownLasso.Throw(bottle);
    }

}
