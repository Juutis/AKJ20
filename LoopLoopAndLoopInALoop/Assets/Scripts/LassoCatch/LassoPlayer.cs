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
    private float maxSpeed;
    private float boostSpeedup = 20f;
    private float dir;
    private int boosts = 0;
    private int maxBoosts = 3;
    private float boostStackCooldown = 5f;
    private float lastBoostStackCooldown = 0f;
    private float rotateSpeed = 15f;
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
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        dir -= horizontal * Time.deltaTime * rotateSpeed;
        //body.MoveRotation(dir);

        if (Input.GetMouseButton(0))
        {
            lassoPower += Time.deltaTime * 0.5f;
            lasso.SpinLasso(transform.position, transform.rotation);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3[] asd = new Vector3[lassoIndicator.positionCount];
            lassoIndicator.GetPositions(asd);
            //Debug.Log(string.Join(',', asd));
            lasso.ThrowLasso(asd.ToList(), transform.position, transform.rotation);
            lassoPower = 0;
        }

        Debug.Log(body.linearVelocity  + ", " + transform.rotation.eulerAngles);
        if (Input.GetKeyDown(KeyCode.W) && (Time.time - lastBoostCooldown > boostCooldown))
        {
            //Debug.Log("Pressed w: " + boosts + ", " + speed);
            if (boosts < maxBoosts)
            {
                boosts++;
                speed += boostSpeedup;
                lastBoostCooldown = Time.time;
            }
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
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, -horizontal * Time.deltaTime * rotateSpeed);

        body.linearVelocity = (transform.rotation * Vector2.up).normalized * speed * Time.deltaTime;// new Vector2(0f, speed) * Time.deltaTime;

    }
}
