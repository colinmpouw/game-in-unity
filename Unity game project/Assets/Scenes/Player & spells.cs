using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public GameObject fireballPrefab;
    public GameObject lightningStrikePrefab;
    public GameObject meteorShowerPrefab;
    public Transform firePoint;
    public float abilityCooldown = 5f;
    private float lastAbilityTime;
    private float ultimateCooldown = 20f;
    private float lastUltimateTime;
    private int fireballLevel = 1;
    private int lightningStrikeLevel = 1;

    void Update()
    {
        Move();
        if (Input.GetMouseButtonDown(0)) Shoot();
        if (Input.GetKeyDown(KeyCode.Q)) CastFireball();
        if (Input.GetKeyDown(KeyCode.E)) CastLightningStrike();
        if (Input.GetKeyDown(KeyCode.X)) CastUltimate();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.position += new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void CastFireball()
    {
        if (Time.time - lastAbilityTime >= abilityCooldown)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            fireball.GetComponent<Fireball>().damage = fireballLevel * 2;
            lastAbilityTime = Time.time;
        }
    }

    void CastLightningStrike()
    {
        if (Time.time - lastAbilityTime >= abilityCooldown)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 3f + (1f * lightningStrikeLevel));
            foreach (var enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Enemy>().TakeDamage(3 * lightningStrikeLevel);
                }
            }
            lastAbilityTime = Time.time;
        }
    }

    void CastUltimate()
    {
        if (Time.time - lastUltimateTime >= ultimateCooldown)
        {
            Instantiate(meteorShowerPrefab, transform.position, Quaternion.identity);
            lastUltimateTime = Time.time;
        }
    }

    public void UpgradeFireball()
    {
        fireballLevel++;
    }

    public void UpgradeLightningStrike()
    {
        lightningStrikeLevel++;
    }
}

public class Fireball : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 2;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

public class MeteorShower : MonoBehaviour
{
    public GameObject meteorPrefab;
    public int meteorCount = 10;
    public float radius = 5f;
    public float delayBetweenMeteors = 0.2f;

    void Start()
    {
        StartCoroutine(SpawnMeteors());
    }

    IEnumerator SpawnMeteors()
    {
        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * radius;
            Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenMeteors);
        }
        Destroy(gameObject);
    }
}

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 1;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}

public class EasyEnemy : Enemy
{
    void Awake()
    {
        speed = 3f;
        health = 1;
    }
}

public class MediumEnemy : Enemy
{
    void Awake()
    {
        speed = 2f;
        health = 3;
    }
}

public class TankEnemy : Enemy
{
    void Awake()
    {
        speed = 1f;
        health = 5;
    }
}

public class GameManager : MonoBehaviour
{
    public GameObject easyEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public float spawnInterval = 3f;
    public Transform[] spawnPoints;
    public PlayerController player;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int enemyType = Random.Range(0, 3);
        GameObject enemyToSpawn;

        if (enemyType == 0)
            enemyToSpawn = easyEnemyPrefab;
        else if (enemyType == 1)
            enemyToSpawn = mediumEnemyPrefab;
        else
            enemyToSpawn = tankEnemyPrefab;

        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
    }

    public void UpgradeAbility(string abilityType)
    {
        if (abilityType == "Fireball")
        {
            player.UpgradeFireball();
        }
        else if (abilityType == "LightningStrike")
        {
            player.UpgradeLightningStrike();
        }
    }
}
