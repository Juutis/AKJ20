using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Lasso : MonoBehaviour
{
    [SerializeField]
    private float lassoTime;

    private float lassoSpeed = 0f;
    private List<Vector3> wayPoints = new();
    private int currentWaypointIndex = 0;
    private Vector3 origin;
    private float lerpT = 0;
    private Quaternion rotation;
    private float spinRotation = 0;

    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private CircleCollider2D circleCollider;

    private float lassoPower = 0f;
    private LassoMode mode = LassoMode.None;
    private float prepareT = 0f;
    private Vector3 prepareStart;
    private float prepareSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LassoMode.None == mode)
        {
            spriteRenderer.enabled = false;
        }
        else if (LassoMode.Throw == mode)
        {
            transform.rotation = Quaternion.identity;
            spriteRenderer.enabled = true;
            Vector3 start = (origin + rotation * wayPoints[currentWaypointIndex]);
            Vector3 end = (origin + rotation * wayPoints[currentWaypointIndex + 1]);
            transform.position = Vector3.Lerp(start, end, lerpT);
            lerpT += lassoSpeed * Time.deltaTime;

            if (lerpT >= 1)
            {
                currentWaypointIndex++;
                lerpT = 0;

                if (currentWaypointIndex == wayPoints.Count - 1)
                {
                    currentWaypointIndex = 0;
                    spriteRenderer.enabled = false;
                    mode = LassoMode.None;
                    return;
                }
            }
        }
        else if (LassoMode.Prepare == mode)
        {
            spriteRenderer.enabled = true;
            transform.position = Vector3.Lerp(prepareStart, this.origin, prepareT);
            prepareT += Time.deltaTime * prepareSpeed;

            if (prepareT >= 1)
            {
                mode = LassoMode.Throw;
                prepareT = 0;
                transform.position = origin;
            }

        }
        else if (LassoMode.Spin == mode)
        {
            spriteRenderer.enabled = true;
            Vector3 offSet = origin + Quaternion.Euler(0, 0, spinRotation) * rotation * ( Vector3.up * lassoPower);
            prepareStart = offSet;
            transform.position = offSet;
            transform.rotation = Quaternion.Euler(0, 0, spinRotation);
            spinRotation += Time.deltaTime * 400f;
        }

        lineRenderer.enabled = LassoMode.None != mode;
        circleCollider.enabled = LassoMode.Throw == mode;

        if (LassoMode.None != mode)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, transform.position - transform.up * 0.25f);
        }
    }

    public void ThrowLasso(List<Vector3> points, Vector3 origin, Quaternion rotation)
    {
        wayPoints.Clear();
        wayPoints.AddRange(points);
        lassoSpeed = 5; // wayPointDistance() / lassoTime;
        mode = LassoMode.Prepare;
        this.origin = origin;
        currentWaypointIndex = 0;
    }

    public void UpdateLasso(Vector3 origin, Quaternion rotation, float power)
    {
        this.origin = origin;
        this.rotation = rotation;
        lassoPower = power;
    }

    public void SpinLasso(Vector3 origin, Quaternion rotation)
    {
        if (LassoMode.Spin != mode)
        {
            this.origin = origin;
            this.rotation = rotation;
            transform.rotation = Quaternion.identity;
            mode = LassoMode.Spin;
            spinRotation = 0;
        }
    }

    private float wayPointDistance()
    {
        float dist = 0f;
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            dist += (wayPoints[i + 1] - wayPoints[i]).magnitude;
        }

        return dist;
    }

    public LassoMode GetMode()
    {
        return mode;
    }

    public enum LassoMode
    {
        None,
        Spin,
        Prepare,
        Throw
    }
}
