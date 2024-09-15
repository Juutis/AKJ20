using UnityEngine;

public class LassoEnemyIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject playerCam;
    SpriteRenderer spriteRenderer;

    private float yOffset;

    // Hysteresis to stop flickering
    private float showTimeBuffer = 0.3f;
    private float startShowWait = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 v1 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1.0f)) - Vector3.up * 0.5f;
        Vector3 v2 = playerCam.transform.position;
        yOffset = v1.y - v2.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 top = Camera.main.ViewportToWorldPoint(new(0f, 1f)) + Vector3.up * 0.5f;
        if (enemy.transform.position.y >= top.y)
        {
            if (Time.time - startShowWait > showTimeBuffer)
            {
                spriteRenderer.enabled = true;
                Vector3 c = playerCam.transform.position;
                transform.position = new(enemy.transform.position.x, c.y + yOffset, 0f);
            }
        }
        else
        {
            startShowWait = Time.time;
            spriteRenderer.enabled = false;
        }
    }
}
