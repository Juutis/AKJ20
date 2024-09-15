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
    private List<Image> shoeImageMasks;
    [SerializeField]
    private Image boostCooldownImage;

    private Rigidbody2D body;

    private float lassoPower = 0f;
    private float maxPower = 1f;
    private List<Vector3> lassoIndicatorNodes = new();
    private float speed = 0f;
    private float maxSpeed = 5f;
    private float minSpeed = 0f;
    private float boostSpeedup = 0.55f;
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
        speed = boostSpeedup;
        float difficulty = GameManager.Instance?.GetDifficulty() ?? 0;
        boostCooldown = Mathf.Lerp(0.8f, 1.4f, difficulty);
        boostStackCooldown = Mathf.Lerp(2f, 3f, difficulty);
    }

    private Vector3 LassoOrigin()
    {
        return transform.position + Vector3.up * 0.3f + Vector3.right * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) && lasso.GetMode() != Lasso.LassoMode.Throw)
        {
            lassoPower = Mathf.Min(maxPower, lassoPower + Time.deltaTime * 0.5f);
            lasso.SpinLasso(LassoOrigin(), transform.rotation);
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector3[] asd = new Vector3[lassoIndicator.positionCount];
            lassoIndicator.GetPositions(asd);
            lasso.ThrowLasso(asd.ToList(), LassoOrigin(), transform.rotation);
            lassoPower = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && (Time.time - lastBoostCooldown > boostCooldown))
        {
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

        lasso.UpdateLasso(LassoOrigin(), transform.rotation, lassoPower);

        for (int i = 0; i < shoeImages.Count; i++)
        {
            shoeImages[i].enabled = (shoeImages.Count - boosts + 1) > i;
            shoeImageMasks[i].fillAmount = 1;
        }

        if (boosts > 0)
        {
            shoeImageMasks[shoeImageMasks.Count - boosts].fillAmount = (Time.time - lastBoostStackCooldown) / boostStackCooldown;
        }

        boostCooldownImage.fillAmount = 1 - (Time.time - lastBoostCooldown) / boostCooldown;

        lassoIndicator.enabled = lasso.GetMode() == Lasso.LassoMode.Spin;
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontal, speed);
    }
}
