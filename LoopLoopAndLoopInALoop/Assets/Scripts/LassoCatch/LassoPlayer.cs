using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lassoIndicator;
    [SerializeField]
    private Lasso lasso;
    [SerializeField]
    private List<Image> shoeImages;
    [SerializeField]
    private Image boostCooldownImage;

    private Rigidbody2D body;

    private float lassoPower = 0f;
    private List<Vector3> lassoIndicatorNodes = new();
    private float speed = 0f;
    private float maxSpeed = 5f;
    private float minSpeed = 0f;
    private float boostSpeedup = 0.33333f;
    private int boosts = 0;
    private int maxBoosts = 3;
    private float boostStackCooldown = 3f;
    private float lastBoostStackCooldown = 0f;
    private float horizontal;
    private float boostCooldown = 1f;
    private float lastBoostCooldown = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        for (int i = 0; i < lassoIndicator.positionCount; i++)
        {
            lassoIndicatorNodes.Add(lassoIndicator.GetPosition(i));
        }

        Initialize();
    }

    public void Initialize()
    {
        speed = minSpeed;
        float difficulty = GameManager.Instance.GetDifficulty();
        boostCooldown = Mathf.Lerp(0.8f, 1.4f, difficulty);
        boostStackCooldown = Mathf.Lerp(2f, 3f, difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        //dir -= horizontal * Time.deltaTime * rotateSpeed;
        //body.MoveRotation(dir);

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            lassoPower += Time.deltaTime * 0.5f;
            lasso.SpinLasso(transform.position, transform.rotation);
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector3[] asd = new Vector3[lassoIndicator.positionCount];
            lassoIndicator.GetPositions(asd);
            //Debug.Log(string.Join(',', asd));
            lasso.ThrowLasso(asd.ToList(), transform.position, transform.rotation);
            lassoPower = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && (Time.time - lastBoostCooldown > boostCooldown))
        {
            //Debug.Log("Pressed w: " + boosts + ", " + speed);
            if (boosts < maxBoosts)
            {
                boosts++;
                speed = Mathf.Min(maxSpeed, speed + boostSpeedup);
                lastBoostCooldown = Time.time;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && (Time.time - lastBoostCooldown > boostCooldown))
        {
            speed = Mathf.Max(minSpeed, speed - boostSpeedup);
            lastBoostCooldown = Time.time;
        }

            if (boosts == 0)
        {
            lastBoostStackCooldown = Time.time;
        }

        if (Time.time - lastBoostStackCooldown > boostStackCooldown && boosts > 0)
        {
            boosts--;
            lastBoostStackCooldown = Time.time;
        }

        for (int i = 0; i < lassoIndicator.positionCount; i++)
        {
            Vector3 pos = lassoIndicatorNodes[i];
            pos *= lassoPower;
            lassoIndicator.SetPosition(i, pos);
        }

        lasso.UpdateLasso(transform.position, transform.rotation, lassoPower);

        for (int i = 0; i < shoeImages.Count; i++)
        {
            shoeImages[i].enabled = boosts > i;
        }

        boostCooldownImage.fillAmount = 1 - (Time.time - lastBoostCooldown) / boostCooldown;

        lassoIndicator.enabled = lasso.GetMode() == Lasso.LassoMode.Spin;
    }

    private void FixedUpdate()
    {
        // transform.Rotate(Vector3.forward, -horizontal * Time.deltaTime * rotateSpeed);
        

        body.linearVelocity = new Vector2(horizontal, speed);// new Vector2(0f, speed) * Time.deltaTime;

    }
}
