using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaloonManager : MonoBehaviour
{
    public static SaloonManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private SaloonBottle bottlePrefab;
    [SerializeField]
    private GameObject bottleCountPrefab;
    [SerializeField]
    private GameObject bottleCountDeadPrefab;
    [SerializeField]
    private SaloonBottleSlot slotPrefab;

    [SerializeField]
    private int bottlesToSpawn = 20;
    private int spawnDifficultyFactor = 10;

    private List<SaloonBottleSlot> slots = new();

    private float spawnTimer = 0f;
    [SerializeField]
    private float spawnIntervalMax = 5f;
    [SerializeField]
    private float spawnIntervalMin = 2f;
    private float spawnInterval = 0f;
    private float spawnIntervalDifficultyFactor = 1f;

    [SerializeField]
    private Transform bottleContainer;
    [SerializeField]
    private Transform slotContainer;

    [SerializeField]
    private Transform leftBound;
    [SerializeField]
    private Transform rightBound;

    private float slotMarginMin = 0.2f;
    private float slotMarginMax = 1f;
    private float slotStep = 0.4f;

    private int bottlesSpawned = 0;
    private int bottlesCaught = 0;
    private int bottlesDied = 0;

    private bool isSpawning = false;
    private float difficulty = 1f;

    [SerializeField]
    private Transform bottleCountContainer;
    private void Start()
    {
        MusicPlayer.main.PlayMusic(MusicType.Saloon);
        if (GameManager.Instance != null)
        {
            difficulty = GameManager.Instance.GetDifficulty();
        }
        spawnIntervalMin -= difficulty * spawnIntervalDifficultyFactor;
        spawnIntervalMax -= difficulty * spawnIntervalDifficultyFactor;
        bottlesToSpawn += Mathf.FloorToInt(difficulty * spawnDifficultyFactor);
        float currentX = leftBound.position.x;
        while (currentX < rightBound.position.x)
        {
            currentX += slotStep + Random.Range(slotMarginMin, slotMarginMax);
            SaloonBottleSlot slot = Instantiate(slotPrefab, slotContainer);
            slot.Initialize(new Vector2(currentX, leftBound.position.y));
            slots.Add(slot);
        }
        Debug.Log($"Created {slots.Count} slots!");
        StartSpawning();
    }

    public void GainBottle(SaloonBottle bottle)
    {
        SaloonBottleSlot slot = slots.Find(slot => slot.Bottle == bottle);
        if (slot != null)
        {
            slot.Bottle = null;
        }
        UpdateBottleCount();
    }

    public void BreakBottle(SaloonBottle bottle)
    {
        SaloonBottleSlot slot = slots.Find(slot => slot.Bottle == bottle);
        if (slot != null)
        {
            slot.Bottle = null;
        }
        UpdateBottleCount(true);
    }

    private void UpdateBottleCount(bool dead = false)
    {
        var countPrefab = Instantiate(dead ? bottleCountDeadPrefab : bottleCountPrefab, bottleCountContainer);
        if (bottlesCaught == 0)
        {
            countPrefab.transform.localPosition = Vector2.zero;
        }
        else
        {
            float step = bottleCountPrefab.transform.localScale.x / 2f;
            float dir = bottlesCaught % 2 == 0 ? -1 : 1;
            countPrefab.transform.localPosition = new Vector2(step * dir * bottlesCaught, 0f);
        }
        if (dead)
        {
            bottlesDied += 1;
        }

        bottlesCaught += 1;
        if (bottlesCaught >= bottlesToSpawn)
        {
            Debug.Log("finalized!");
            if (bottlesDied > bottlesToSpawn / 2)
            {
                Debug.Log($"you didn't get 50% {bottlesCaught - bottlesDied} / {bottlesToSpawn}");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.Lose();
                }
            }
            else
            {
                Debug.Log($"you Win {bottlesCaught - bottlesDied} / {bottlesToSpawn}!");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoadNextLevel();
                }
            }
        }

    }

    public void StartSpawning()
    {
        isSpawning = true;
        SetSpawnInterval();
    }

    private void SetSpawnInterval()
    {

        spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    public SaloonBottle GetHighlightedBottle()
    {
        var slot = slots.Find(slot => slot.Bottle != null && slot.Bottle.IsHighlighted);
        if (slot != null)
        {
            return slot.Bottle;
        }
        return null;
    }

    void Update()
    {
        if (!isSpawning)
        {
            return;
        }
        if (bottlesSpawned >= bottlesToSpawn)
        {
            isSpawning = false;
            return;
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0f;
            var openSlots = slots.Where(slot => slot.Bottle == null).ToList();
            if (openSlots.Count == 0)
            {
                SetSpawnInterval();
                return;
            }
            var slot = openSlots[Random.Range(0, openSlots.Count)];
            if (slot == null)
            {
                Debug.Log("no slots!");
            }
            else
            {
                slot.Bottle = Instantiate(bottlePrefab, slot.Position, Quaternion.identity, bottleContainer);
                slot.Bottle.Initialize(difficulty);
                bottlesSpawned += 1;
            }
            SetSpawnInterval();
        }
    }
}

