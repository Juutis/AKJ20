using UnityEngine;

public class LassoEnemy : MonoBehaviour
{
    //start
    private float startTime = 0.5f;
    private float started = 0f;

    private float initialT = 0f;

    //run
    private float maxSpeed = 2.5f;
    private float minSpeed = 1f;
    private float currentSpeed = 0f;
    private float lastSpeedChange = 0f;
    private float minSpeedTime = 1f;
    private float maxSpeedTime = 5f;
    private float currentSpeedCooldown = 0f;
    private float speedChangeT = 0f;
    private float targetSpeed = 0f;

    private float maxStrafeSpeed = 1f;
    private float currentStrafeSpeed = 0f;
    private float lastStrafeChange = 0f;
    private float minStrafeTime = 2f;
    private float maxStrafeTime = 5f;
    private float currentStrafeCooldown = 0f;

    private EnemyMode mode;
    private Rigidbody2D body;
    private CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        Initialize();
    }

    public void Initialize()
    {
        mode = EnemyMode.Start;
        started = Time.time;
        currentSpeed = 0f;
        initialT = 0f;

        float difficulty = GameManager.Instance.GetDifficulty();
        minSpeed = Mathf.Lerp(1f, 2.5f, difficulty);
        maxSpeed = Mathf.Lerp(2f, 5f, difficulty);
        circleCollider.radius = Mathf.Lerp(0.2f, 0.1f, difficulty);
        maxStrafeSpeed = Mathf.Lerp(1f, 2f, difficulty);
        maxSpeedTime = 5f - 2f * difficulty;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (EnemyMode.Start == mode)
        {
            if (Time.time - started > startTime)
            {
                mode = EnemyMode.InitialAcceleration;
            }
        }
        else if (EnemyMode.InitialAcceleration == mode)
        {
            currentSpeed = Mathf.Lerp(0f, minSpeed, initialT);
            initialT += Time.deltaTime * 5;

            if (initialT >= 1f)
            {
                targetSpeed = currentSpeed;
                mode = EnemyMode.Running;
            }
        }
        else if (EnemyMode.Running == mode)
        {
            // change strafing randomly
            if (Time.time - lastStrafeChange > currentStrafeCooldown)
            {
                // 50-50 enemy strafes
                if (Random.Range(0, 2) > 0)
                {
                    float strafeDir = -1 * Mathf.Sign(currentStrafeSpeed);
                    float halfStrafeSpeed = maxStrafeSpeed / 2f;
                    currentStrafeSpeed = Random.Range(1, 3) * halfStrafeSpeed * strafeDir;
                }
                else
                {
                    currentStrafeSpeed = 0f;
                }

                currentStrafeCooldown = Random.Range(minStrafeTime, maxStrafeTime);
                lastStrafeChange = Time.time;
            }

            // change running speed randomly
            if (Time.time - lastSpeedChange > currentSpeedCooldown)
            {
                // 50-50 enemy changes speed
                if (Random.Range(0, 2) > 0)
                {
                    float minMaxDiff = maxSpeed - minSpeed;
                    targetSpeed = minSpeed + Random.Range(0f, 1f) * minMaxDiff;
                    speedChangeT = 0;
                }

                // Debug.Log("Speed CD: " + currentSpeedCooldown + " target speed: " + targetSpeed);
                currentSpeedCooldown = Random.Range(minSpeedTime, maxSpeedTime);
                lastSpeedChange = Time.time;
            }

            if (speedChangeT <= 1)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeT);
                speedChangeT += Time.deltaTime;
            }
        }

        body.linearVelocityY = currentSpeed;
        body.linearVelocityX = currentStrafeSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            Debug.Log("Caught!");
            // UnityEditor.EditorApplication.isPlaying = false;
            Invoke("Win", 0.5f);
        }
    }

    private void Win()
    {
        GameManager.Instance.LoadNextLevel();
    }

    enum EnemyMode
    {
        Start,
        InitialAcceleration,
        Running,
        Caught
    }
}
