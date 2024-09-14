using Mono.Cecil;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool ropeIndicator = false;
    private float ropeIndicatorTimer;
    [SerializeField]
    private Color ropeIndicatorColor;
    private Color ropeOrigColor;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("RandomizeHeading", 3.0f);
        ropeOrigColor = rope.color;
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
            rope.color = ropeOrigColor;
        }

        if (ropeIndicator && !inActive) {
            var ct = Mathf.Sin((Time.time - ropeIndicatorTimer) * 5.0f);
            var c = Color.Lerp(ropeOrigColor, ropeIndicatorColor, ct);
            rope.color = c;
        }
    }

    public void RandomizeHeading() {
        lastHeading = currentHeading;
        targetHeading = Random.Range(-1.0f, 1.0f) * Mathf.Lerp(1.0f, 5.0f, HangManManager.Instance.Difficulty);
        headingTimer = Time.time;
        Invoke("RandomizeHeading", Random.Range(0.6f, 1.0f) * Mathf.Lerp(0.4f, 0.2f, HangManManager.Instance.Difficulty));
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
        Invoke("Win", 3.0f);
    }

    public void ShowRopeIndicator() {
        ropeIndicator = true;
        ropeIndicatorTimer = Time.time;
    }

    public void Win() {
        GameManager.Instance.LoadNextLevel();
    }
}
