using UnityEngine;
using UnityEngine.InputSystem;

public class HangingManGun : MonoBehaviour
{
    Vector3 targetPosition;
    Vector2 targetOffset;
    Vector2 lastHeading;
    Vector2 currentHeading;
    Vector2 targetHeading;
    float headingTimer;

    float maxTargetPosOffset = 0.1f;

    float lastFired = 0;

    [SerializeField]
    ParticleSystem boom;

    [SerializeField]
    Transform shootTarget;

    int ropeLayerMask;

    bool controllable = false;
    bool active = false;
    
    [SerializeField]
    private Transform moveTo;

    [SerializeField]
    private GameObject birbs;

    private HangingMan hMan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Invoke("RandomizeHeading", 0.0f);
        ropeLayerMask = LayerMask.GetMask("HangingManRope");
        Invoke("Activate", 10.0f);
        hMan = FindAnyObjectByType<HangingMan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        var t = Time.time - headingTimer;
        t = Mathf.Clamp01(t);
        currentHeading = Vector2.Lerp(lastHeading, targetHeading, t);

        if (controllable) {
            var inputX = Input.GetAxis("HangingManMouse X");
            var inputY = Input.GetAxis("HangingManMouse Y");
            var input = new Vector2(inputX, inputY) * 0.1f;
            transform.position = transform.position + (Vector3)input;
        } else {
            transform.position = Vector3.MoveTowards(transform.position, moveTo.position, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveTo.position) < 0.1f) {
                controllable = true;
            }
        }
        transform.position += (Vector3)currentHeading * Time.deltaTime * Mathf.Lerp(1.0f, 4.0f, HangManManager.Instance.Difficulty);
        
        if(controllable) {
            var clampedX = Mathf.Clamp(transform.position.x, -3, 3);
            var clampedY = Mathf.Clamp(transform.position.y, -3, 3);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        var idealTargetPosition = transform.position + Vector3.forward;

        targetOffset = new Vector2(Mathf.Sin(Time.time * 2.827f) * 1.112f, Mathf.Cos(Time.time * 3.87f) * 1.223f);
        targetOffset = new Vector2(Mathf.Clamp(targetOffset.x, -1, 1), Mathf.Clamp(targetOffset.y, -1, 1));
        targetPosition = Vector3.MoveTowards(targetPosition, idealTargetPosition, 1.0f * Time.deltaTime);

        var clampedTargetPosX = Mathf.Clamp(targetPosition.x, idealTargetPosition.x - maxTargetPosOffset, idealTargetPosition.x + maxTargetPosOffset);
        var clampedTargetPosy = Mathf.Clamp(targetPosition.y, idealTargetPosition.y - maxTargetPosOffset, idealTargetPosition.y + maxTargetPosOffset);
        targetPosition = new Vector3(clampedTargetPosX, clampedTargetPosy, targetPosition.z);

        var finalTargePos = targetPosition + (Vector3)targetOffset * Mathf.Lerp(0.02f, 0.03f, HangManManager.Instance.Difficulty);
        transform.LookAt(finalTargePos);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (lastFired < Time.time - 0.8f) {
                lastFired = Time.time;
                var newBoom = Instantiate(boom);
                newBoom.transform.position = shootTarget.position;
                var hit = Physics2D.Raycast(shootTarget.position, Vector2.zero, Mathf.Infinity, ropeLayerMask);
                if (hit.collider != null) {
                    hMan.Free();
                }
                birbs.SetActive(true);
            }
        }
    }

    public void RandomizeHeading() {
        lastHeading = currentHeading;
        targetHeading = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        headingTimer = Time.time;
        Invoke("RandomizeHeading", 0.5f);
    }

    public void Activate() {
        active = true;
        hMan.ShowRopeIndicator();
    }
}
