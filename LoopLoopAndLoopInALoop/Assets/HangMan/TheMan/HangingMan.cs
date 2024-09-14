using UnityEngine;

public class HangingMan : MonoBehaviour
{
    private float balance;
    private float currentHeading = 0;
    private float lastHeading = 0;
    private float targetHeading = 0;
    private float headingTimer = 0;
    private Animator anim;
    private float failTimer = 0;
    private bool inActive = false;

    [SerializeField]
    private ParticleSystem ropeGone;

    [SerializeField]
    private SpriteRenderer rope;

    [SerializeField]
    private Rigidbody2D man;

    [SerializeField]
    private Rigidbody2D chair;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("RandomizeHeading", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inActive) return;

        var t = Time.time - headingTimer;
        t = Mathf.Clamp01(t);
        currentHeading = Mathf.Lerp(lastHeading, targetHeading, t);

        balance += currentHeading * Time.deltaTime;

        var input = Input.GetAxis("HangingManHorizontal");
        balance += input * Time.deltaTime * 10.0f;

        balance = Mathf.Clamp(balance, -1.0f, 1.0f);

        var time = (balance + 1) / 2.0f;
        anim.SetFloat("time", time);

        if (Mathf.Abs(balance) < 0.99f) {
            failTimer = Time.time;
        }
        if (Time.time - failTimer > 0.3f) {
            var dieAnim = balance > 0 ? "die_right" : "die_left";
            anim.Play(dieAnim);
            inActive = true;
        }
    }

    public void RandomizeHeading() {
        lastHeading = currentHeading;
        targetHeading = Random.Range(-1.0f, 1.0f) * 3.0f;
        headingTimer = Time.time;
        Invoke("RandomizeHeading", 0.25f);
    }

    public void Free() {
        if (inActive) return;
        ropeGone.Play();
        rope.enabled = false;
        inActive = true;
        man.simulated = true;
        chair.simulated = true;
        anim.enabled = false;
        man.AddTorque(Random.Range(-1.0f, 1.0f), ForceMode2D.Impulse);
    }
}
