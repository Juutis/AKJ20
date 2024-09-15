using UnityEngine;

public class SaloonAimIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform leftBorder;
    [SerializeField]
    private Transform rightBorder;

    [SerializeField]
    private Transform movingTransform;

    public Transform MovingTransform
    {
        get { return movingTransform; }
    }

    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float noiseSpeedFactor = 5;
    private float noiseSpeed = 2;

    private float difficultyModifier = 5;
    private float difficultyFactor = 0;

    [SerializeField]
    private Transform startingPosition;
    [SerializeField]
    private int startingDirection = 1;
    private int direction = 1;

    private bool isMoving = false;
    private bool isStarted = false;

    public void Initialize(float difficulty)
    {
        difficultyFactor = difficulty * difficultyModifier;
    }

    public void Move()
    {
        isMoving = true;
        if (!isStarted)
        {
            direction = startingDirection;
        }
    }

    public void Stop()
    {
        if (!isMoving)
        {
            return;
        }
        isMoving = false;
        isStarted = false;
        movingTransform.position = startingPosition.position;
    }

    private float Noise()
    {
        return Mathf.PerlinNoise(Time.time * (noiseSpeed + difficultyFactor), 0.0f);
    }

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        Vector3 pos = movingTransform.position;
        pos.x += Time.deltaTime * ((speed + difficultyFactor) + noiseSpeedFactor * Noise()) * direction;

        bool isWithinBounds = leftBorder.position.x < pos.x && pos.x < rightBorder.position.x;
        bool isOutofBounds = pos.x > rightBorder.position.x || pos.x < leftBorder.position.x;
        if (!isStarted && isWithinBounds)
        {
            isStarted = true;
        }
        else if (isStarted && isOutofBounds)
        {
            direction = -direction;
        }

        movingTransform.position = pos;
    }
}
