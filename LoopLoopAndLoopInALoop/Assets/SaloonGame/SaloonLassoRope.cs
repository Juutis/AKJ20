using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaloonLassoRope : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    private Transform followTarget;

    private bool isMoving = false;
    public void Move(Transform target)
    {
        followTarget = target;
        addNodeTimer = 0f;
        isMoving = true;
    }
    public void Reset()
    {
        positions = new();
        UpdateLine();
    }

    public void Stop()
    {
        AddPosition(followTarget.transform.position);
        Vector3 pos = followTarget.transform.position;
        pos.y += 0.4f;
        AddPosition(pos);
        isMoving = false;
    }

    [SerializeField]
    private float addNodeInterval = 0.5f;
    private float addNodeTimer = 0;

    private List<Vector3> positions = new();
    public void AddPosition(Vector3 position)
    {
        positions.Add(position);
        UpdateLine();
    }

    private void UpdateLine()
    {
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
    }

    void Update()
    {
        if (!isMoving)
        {
            return;
        }
        addNodeTimer += Time.deltaTime;
        if (addNodeTimer > addNodeInterval)
        {
            AddPosition(followTarget.transform.position);
            addNodeTimer = 0f;
        }
    }
}
