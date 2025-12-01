using UnityEngine;

public abstract class BossEnemy : Enemy
{
    [Header("Boss attributes")]
    public int phase = 1;
    public int totalPhases = 2;
    public float phaseThreshold = 0.5f;
    public GameObject spawnOnDeath;

    protected Health bossHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        bossHealth = GetComponent<Health>();
        if(bossHealth != null)
        {
            bossHealth.OnDeath += OnBossDeath;
        }
    }

    protected override void Update()
    {
        base.Update();
        PhaseCheck();
    }

    protected virtual void PhaseCheck()
    {
        if (bossHealth != null && phase <= totalPhases)
        {
            float healthFract = bossHealth.currentHealth / bossHealth.maxHealth;
            if(healthFract <= phaseThreshold)
            {
                EnterNextPhase();
            }
        }
    }

    protected virtual void EnterNextPhase()
    {
        phase += 1;
    }

    protected virtual void OnBossDeath()
    {
        StopAllCoroutines();

        if (spawnOnDeath)
        {
            Instantiate(spawnOnDeath, transform.position, transform.rotation);
        }
    }

}
