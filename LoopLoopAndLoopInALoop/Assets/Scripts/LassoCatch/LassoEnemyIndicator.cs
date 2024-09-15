using TMPro;
using UnityEngine;

public class LassoEnemyIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject playerCam;
    [SerializeField]
    private TextMeshProUGUI distanceText;
    [SerializeField]
    private ArrowDir direction;

    SpriteRenderer spriteRenderer;

    private float yOffsetTop;
    private float yOffsetBot;

    private float xOffsetRight;
    private float xOffsetLeft;

    // Hysteresis to stop flickering
    private float showTimeBuffer = 0.3f;
    private float startShowWait = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 vTop = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1.0f)) - Vector3.up * 0.5f;
        Vector3 vBot = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f)) - Vector3.down * 0.4f;
        Vector3 vRight = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0f)) - Vector3.right * 0.5f;
        Vector3 vLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f)) - Vector3.left * 0.5f;
        Vector3 v2 = playerCam.transform.position;
        yOffsetTop = vTop.y - v2.y;
        yOffsetBot = vBot.y - v2.y;
        xOffsetRight = Mathf.Abs(vRight.x - v2.x);
        xOffsetLeft = Mathf.Abs(vLeft.x - v2.x);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = 0;
        if (direction == ArrowDir.Up)
        {
            Vector3 top = Camera.main.ViewportToWorldPoint(new(0f, 1f)) + Vector3.up * 0.5f;
            if (enemy.transform.position.y >= top.y)
            {
                if (Time.time - startShowWait > showTimeBuffer)
                {
                    spriteRenderer.enabled = true;
                    Vector3 c = playerCam.transform.position;
                    transform.position = new(enemy.transform.position.x, c.y + yOffsetTop, 0f);
                    distanceText.gameObject.SetActive(true);
                    distance = (enemy.transform.position.y - c.y) * 3;
                    distanceText.SetText($"{(int)distance}m");
                }
            }
            else
            {
                startShowWait = Time.time;
                spriteRenderer.enabled = false;
                distanceText.gameObject.SetActive(false);
            }
        }
        else if (direction == ArrowDir.Down)
        {
            Vector3 bot = Camera.main.ViewportToWorldPoint(new(0f, 0f)) + Vector3.down * 0.5f;
            if (enemy.transform.position.y < bot.y)
            {
                if (Time.time - startShowWait > showTimeBuffer)
                {
                    spriteRenderer.enabled = true;
                    Vector3 c = playerCam.transform.position;
                    transform.position = new(enemy.transform.position.x, c.y + yOffsetBot, 0f);
                    distanceText.gameObject.SetActive(true);
                    distance = Mathf.Abs(enemy.transform.position.y - c.y) * 3;
                    distanceText.SetText($"{(int)distance}m");
                }
            }
            else
            {
                startShowWait = Time.time;
                spriteRenderer.enabled = false;
                distanceText.gameObject.SetActive(false);
            }
        }
        else if (direction == ArrowDir.Right)
        {
            Vector3 right = Camera.main.ViewportToWorldPoint(new(1f, 0f)) + Vector3.right * 0.5f;
            if (enemy.transform.position.x >= right.x)
            {
                Debug.Log("Enemy: " + enemy.transform.position.x + " Indicator: " + right.x);
                if (Time.time - startShowWait > showTimeBuffer)
                {
                    spriteRenderer.enabled = true;
                    Vector3 c = playerCam.transform.position;
                    transform.position = new(c.x + xOffsetRight, enemy.transform.position.y, 0f);
                    distanceText.gameObject.SetActive(true);
                    distance = (enemy.transform.position.x - c.x) * 3;
                    distanceText.SetText($"{(int)distance}m");
                }
            }
            else
            {
                startShowWait = Time.time;
                spriteRenderer.enabled = false;
                distanceText.gameObject.SetActive(false);
            }
        }
        else if (direction == ArrowDir.Left)
        {
            Vector3 left = Camera.main.ViewportToWorldPoint(new(0f, 0f)) + Vector3.left * 0.5f;
            if (enemy.transform.position.x < left.x)
            {
                if (Time.time - startShowWait > showTimeBuffer)
                {
                    spriteRenderer.enabled = true;
                    Vector3 c = playerCam.transform.position;
                    transform.position = new(c.x - xOffsetLeft, enemy.transform.position.y, 0f);
                    distanceText.gameObject.SetActive(true);
                    distance = (enemy.transform.position.x + c.x) * 3;
                    distanceText.SetText($"{(int)distance}m");
                }
            }
            else
            {
                startShowWait = Time.time;
                spriteRenderer.enabled = false;
                distanceText.gameObject.SetActive(false);
            }
        }

        if (distance > 50)
        {
            GameManager.Instance.Lose();
        }
    }

    enum ArrowDir
    {
        Up,
        Down,
        Left,
        Right
    }
}
