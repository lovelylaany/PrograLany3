using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacement : MonoBehaviour
{
    [System.Serializable]
    private class EnemyWave
    {
        public int enemyCount = 5;
        public float timeBetweenEnemies = 2f;
    }

    [SerializeField] private Transform squadPrototypes;
    [SerializeField] private Material primitiveMaterial;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float attackRadius = 8f;
    [Header("Escenario")]
    [SerializeField] private Terrain groundTerrain;
    [SerializeField] private Vector3[] buildingPositions =
    {
        new Vector3(-4f, 0.15f, 0f),
        new Vector3(0f, 0.15f, 0f),
        new Vector3(4f, 0.15f, 0f)
    };
    [Header("Camino y vida")]
    public int baseHealth = 10;
    [SerializeField] private Vector3[] enemyPath =
    {
        new Vector3(-6f, 1f, 8f),
        new Vector3(-6f, 1f, 3f),
        new Vector3(2f, 1f, 3f),
        new Vector3(2f, 1f, -3f),
        new Vector3(6f, 1f, -3f)
    };
    [SerializeField] private EnemyWave[] waves =
    {
        new EnemyWave(), new EnemyWave(), new EnemyWave()
    };
    private BuildingLocation selectedLocation;
    private bool showTowerMenu;
    private readonly List<GameObject> rapidTowers = new List<GameObject>();
    private readonly List<GameObject> areaTowers = new List<GameObject>();
    private readonly List<float> rapidTimers = new List<float>();
    private readonly List<float> areaTimers = new List<float>();
    private readonly List<Squad> enemyPool = new List<Squad>();
    private int currentWave;
    private int enemiesSpawnedInWave;
    private float enemyTimer;

    private void Start()
    {
        foreach (Vector3 position in buildingPositions)
            CreateLocation(GroundPosition(position, 0.15f));
        foreach (Transform prototype in squadPrototypes) prototype.gameObject.SetActive(false);
        SpawnNextEnemy();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                BuildingLocation location = hit.collider.GetComponent<BuildingLocation>();
                if (location != null) location.Select();
            }
        }

        AttackWithTowers(rapidTowers, rapidTimers, false);
        AttackWithTowers(areaTowers, areaTimers, true);

        if (currentWave < waves.Length)
        {
            enemyTimer += Time.deltaTime;
            if (enemyTimer >= waves[currentWave].timeBetweenEnemies)
            {
                enemyTimer -= waves[currentWave].timeBetweenEnemies;
                SpawnNextEnemy();
            }
        }
    }

    private void SpawnNextEnemy()
    {
        if (currentWave >= waves.Length) return;

        CreateEnemy(Random.Range(0, 5));
        enemiesSpawnedInWave++;
        if (enemiesSpawnedInWave >= waves[currentWave].enemyCount)
        {
            currentWave++;
            enemiesSpawnedInWave = 0;
            enemyTimer = 0f;
        }
    }

    private void SelectLocation(BuildingLocation location)
    {
        if (location.IsOccupied) return;
        selectedLocation = location;
        showTowerMenu = true;
    }

    private void OnGUI()
    {
        if (!showTowerMenu) return;
        if (GUI.Button(new Rect(10, 10, 150, 30), "Ataque rapido")) CreateTower(0);
        if (GUI.Button(new Rect(10, 45, 150, 30), "Escuadron")) CreateTower(1);
        if (GUI.Button(new Rect(10, 80, 150, 30), "Ataque en area")) CreateTower(2);
    }

    private void CreateLocation(Vector3 position)
    {
        GameObject locationObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        SetMaterialColor(locationObject, Color.white);
        locationObject.name = "Ubicacion de construccion";
        locationObject.transform.position = position;
        locationObject.transform.localScale = new Vector3(1.4f, 0.15f, 1.4f);
        BuildingLocation location = locationObject.AddComponent<BuildingLocation>();
        location.OnSelected.AddListener(() => SelectLocation(location));
    }

    private void CreateTower(int type)
    {
        PrimitiveType shape = type == 1 ? PrimitiveType.Cylinder : PrimitiveType.Cube;
        GameObject tower = GameObject.CreatePrimitive(shape);
        tower.name = type == 0 ? "Torre rapida" : type == 1 ? "Torre de escuadron" : "Torre de area";
        tower.transform.position = selectedLocation.transform.position + Vector3.up * (type == 1 ? 1.2f : 0.8f);
        tower.transform.localScale = type == 1 ? new Vector3(1.2f, 1.2f, 1.2f) : new Vector3(1.2f, 1.6f, 1.2f);
        SetMaterialColor(tower, type == 0 ? Color.red : type == 1 ? Color.blue : Color.yellow);
        selectedLocation.MarkOccupied();

        if (type == 1) SpawnSoldiers();
        if (type == 0) { rapidTowers.Add(tower); rapidTimers.Add(0f); }
        if (type == 2) { areaTowers.Add(tower); areaTimers.Add(0f); }
        showTowerMenu = false;
    }

    private void SpawnSoldiers()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform prototype = squadPrototypes.GetChild(i % squadPrototypes.childCount);
            Vector3 location = selectedLocation.transform.position;
            GameObject soldier = Instantiate(prototype.gameObject, location + new Vector3((i - 1) * 0.8f, 1f, 0f), Quaternion.identity);
            soldier.name = "Soldado";
            soldier.SetActive(true);
            SetMaterialColor(soldier, new Color(0.7f, 0.4f, 0.9f));
            Squad squad = soldier.GetComponent<Squad>();
            if (squad == null) squad = soldier.AddComponent<Squad>();
            squad.isEnemy = false;
            squad.groundTerrain = groundTerrain;
            soldier.transform.position = GroundPosition(soldier.transform.position, 1f);
            squad.MoveForward(location + new Vector3((i - 1) * 0.8f, 1f, 5f));
        }
    }

    private void CreateEnemy(int index)
    {
        if (enemyPath == null || enemyPath.Length == 0) return;

        Squad stats = null;
        foreach (Squad pooledEnemy in enemyPool)
        {
            if (!pooledEnemy.gameObject.activeSelf)
            {
                stats = pooledEnemy;
                break;
            }
        }

        if (stats == null)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemigo";
            SetMaterialColor(enemy, Color.black);
            stats = enemy.AddComponent<Squad>();
            enemyPool.Add(stats);
        }

        GameObject enemyObject = stats.gameObject;
        Vector3 startDirection = enemyPath.Length > 1 ? enemyPath[0] - enemyPath[1] : Vector3.forward;
        startDirection.y = 0f;
        enemyObject.transform.position = GroundPosition(enemyPath[0] + startDirection.normalized * index * 1.2f, 1f);
        stats.isEnemy = true;
        stats.groundTerrain = groundTerrain;
        stats.defense = 1f;
        stats.health = 40f;
        stats.speed = 1.5f;
        stats.attack = 7f;
        stats.ResetEnemy(enemyPath, this);
        enemyObject.SetActive(true);
    }

    public void ReturnEnemy(Squad enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private Vector3 GroundPosition(Vector3 position, float height)
    {
        if (groundTerrain != null)
            position.y = groundTerrain.SampleHeight(position) + groundTerrain.transform.position.y + height;
        return position;
    }

    public void LoseLife()
    {
        baseHealth = Mathf.Max(0, baseHealth - 1);
        Debug.Log("Vida restante: " + baseHealth);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyPath == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < enemyPath.Length; i++)
        {
            Gizmos.DrawSphere(enemyPath[i], 0.2f);
            if (i > 0) Gizmos.DrawLine(enemyPath[i - 1], enemyPath[i]);
        }
    }

    private void AttackWithTowers(List<GameObject> towers, List<float> timers, bool areaAttack)
    {
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            if (towers[i] == null) { towers.RemoveAt(i); timers.RemoveAt(i); continue; }
            if (Time.time < timers[i]) continue;

            Squad target = FindNearestEnemy(towers[i].transform.position);
            if (target == null) continue;

            Vector3 origin = towers[i].transform.position + Vector3.up * 0.7f;
            Vector3 direction = (target.transform.position - origin).normalized;
            GameObject shot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shot.name = areaAttack ? "Bomba" : "Proyectil";
            shot.transform.position = origin;
            shot.transform.localScale = Vector3.one * (areaAttack ? 0.4f : 0.2f);
            SetMaterialColor(shot, areaAttack ? Color.yellow : Color.red);
            Collider shotCollider = shot.GetComponent<Collider>();
            shotCollider.isTrigger = false;
            Physics.IgnoreCollision(shotCollider, towers[i].GetComponent<Collider>());
            Rigidbody body = shot.AddComponent<Rigidbody>();
            body.useGravity = areaAttack;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            body.linearVelocity = areaAttack ? direction * 10f + Vector3.up * 4f : direction * 18f;
            Projectile projectile = shot.AddComponent<Projectile>();
            projectile.damage = areaAttack ? 15f : 10f;
            projectile.explosionRadius = areaAttack ? 2.5f : 0f;
            projectile.isAreaAttack = areaAttack;
            timers[i] = Time.time + (areaAttack ? 2.5f : 0.6f);
        }
    }

    private Squad FindNearestEnemy(Vector3 position)
    {
        Squad[] units = FindObjectsByType<Squad>(FindObjectsSortMode.None);
        Squad nearest = null;
        float closest = attackRadius;
        foreach (Squad unit in units)
        {
            if (!unit.isEnemy) continue;
            float distance = Vector3.Distance(position, unit.transform.position);
            if (distance < closest) { closest = distance; nearest = unit; }
        }
        return nearest;
    }

    private void SetMaterialColor(GameObject objectToColor, Color color)
    {
        Material material = new Material(primitiveMaterial);
        material.SetColor("_BaseColor", color);
        material.color = color;
        objectToColor.GetComponent<Renderer>().material = material;
    }
}
