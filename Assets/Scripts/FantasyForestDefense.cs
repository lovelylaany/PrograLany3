using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FantasyForestDefense : MonoBehaviour
{
    public int baseHealth = 10;
    public int enemiesPerWave = 5;
    public int waveCount = 3;
    public float timeBetweenEnemies = 2f;
    public float enemySpeed = 3f;

    private class Enemy
    {
        public GameObject gameObject;
        public Squad squad;
    }

    private readonly List<Enemy> enemies = new List<Enemy>();
    private readonly List<Vector3> path = new List<Vector3>();
    private readonly List<GameObject> rapidTowers = new List<GameObject>();
    private readonly List<GameObject> areaTowers = new List<GameObject>();
    private readonly List<float> rapidTimers = new List<float>();
    private readonly List<float> areaTimers = new List<float>();
    private Terrain terrain;
    private BuildingLocation selectedLocation;
    private bool showTowerMenu;
    private float spawnTimer;
    private int currentWave;
    private int spawnedInWave;

    private void Start()
    {
        terrain = FindFirstObjectByType<Terrain>();
        if (terrain == null) return;

        CreatePath();
        CreateTowerBases();
        CreateBase();
        PlaceCamera();
        SpawnEnemy();
    }

    private void Update()
    {
        if (terrain == null) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Camera sceneCamera = FindFirstObjectByType<Camera>();
            Ray ray = sceneCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                BuildingLocation location = hit.collider.GetComponent<BuildingLocation>();
                if (location != null) location.Select();
            }
        }

        AttackWithTowers(rapidTowers, rapidTimers, false);
        AttackWithTowers(areaTowers, areaTimers, true);

        if (currentWave < waveCount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= timeBetweenEnemies)
            {
                spawnTimer = 0f;
                SpawnEnemy();
            }
        }

    }

    private void OnGUI()
    {
        if (!showTowerMenu) return;

        if (GUI.Button(new Rect(10, 10, 150, 30), "Ataque rapido")) CreateTower(0);
        if (GUI.Button(new Rect(10, 45, 150, 30), "Escuadron")) CreateTower(1);
        if (GUI.Button(new Rect(10, 80, 150, 30), "Ataque en area")) CreateTower(2);
    }

    private void CreatePath()
    {
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 size = terrain.terrainData.size;

        AddPathPoint(terrainPosition, size, 0.10f, 0.18f);
        AddPathPoint(terrainPosition, size, 0.38f, 0.18f);
        AddPathPoint(terrainPosition, size, 0.38f, 0.45f);
        AddPathPoint(terrainPosition, size, 0.68f, 0.45f);
        AddPathPoint(terrainPosition, size, 0.68f, 0.76f);
        AddPathPoint(terrainPosition, size, 0.90f, 0.76f);

        for (int i = 1; i < path.Count; i++)
            CreatePathStones(path[i - 1], path[i]);
    }

    private void AddPathPoint(Vector3 terrainPosition, Vector3 size, float x, float z)
    {
        Vector3 point = terrainPosition + new Vector3(size.x * x, 0f, size.z * z);
        point.y = TerrainHeight(point);
        path.Add(point);
    }

    private void CreatePathStones(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int stoneCount = Mathf.CeilToInt(distance / 1.2f);

        for (int i = 0; i <= stoneCount; i++)
        {
            Vector3 position = Vector3.Lerp(start, end, i / (float)stoneCount);
            position.y = TerrainHeight(position) + 0.08f;

            GameObject stone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stone.name = "Camino";
            stone.transform.position = position;
            stone.transform.localScale = new Vector3(1.4f, 0.12f, 1.4f);
            SetColor(stone, new Color(0.35f, 0.18f, 0.08f));
            RemoveCollider(stone);
        }
    }

    private void CreateBase()
    {
        GameObject baseObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        baseObject.name = "Base - Vida " + baseHealth;
        baseObject.transform.position = path[path.Count - 1] + Vector3.up;
        baseObject.transform.localScale = new Vector3(2f, 1f, 2f);
        SetColor(baseObject, new Color(0.2f, 0.55f, 1f));
        RemoveCollider(baseObject);
    }

    private void CreateTowerBases()
    {
        CreateTowerBase(path[1] + new Vector3(0f, 0f, -3f), 1);
        CreateTowerBase(path[2] + new Vector3(-3f, 0f, 0f), 2);
        CreateTowerBase(path[3] + new Vector3(3f, 0f, 0f), 3);
    }

    private void CreateTowerBase(Vector3 position, int number)
    {
        position.y = TerrainHeight(position) + 0.15f;

        GameObject towerBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        towerBase.name = "Base de torre " + number;
        towerBase.transform.position = position;
        towerBase.transform.localScale = new Vector3(1.8f, 0.15f, 1.8f);
        SetColor(towerBase, new Color(0.65f, 0.65f, 0.7f));

        BuildingLocation location = towerBase.AddComponent<BuildingLocation>();
        location.OnSelected.AddListener(() => SelectLocation(location));
    }

    private void SelectLocation(BuildingLocation location)
    {
        if (location.IsOccupied) return;
        selectedLocation = location;
        showTowerMenu = true;
    }

    private void CreateTower(int type)
    {
        if (selectedLocation == null || selectedLocation.IsOccupied) return;

        PrimitiveType shape = type == 1 ? PrimitiveType.Cylinder : PrimitiveType.Cube;
        GameObject tower = GameObject.CreatePrimitive(shape);
        tower.name = type == 0 ? "Torre rapida" : type == 1 ? "Torre de escuadron" : "Torre de area";
        tower.transform.position = selectedLocation.transform.position + Vector3.up * (type == 1 ? 1.2f : 0.8f);
        tower.transform.localScale = type == 1
            ? new Vector3(1.2f, 1.2f, 1.2f)
            : new Vector3(1.2f, 1.6f, 1.2f);

        SetColor(tower, type == 0 ? Color.red : type == 1 ? Color.blue : Color.yellow);
        Vector3 towerLocation = selectedLocation.transform.position;
        selectedLocation.MarkOccupied();

        if (type == 0)
        {
            rapidTowers.Add(tower);
            rapidTimers.Add(0f);
        }
        if (type == 1) SpawnSoldiers(towerLocation);
        if (type == 2)
        {
            areaTowers.Add(tower);
            areaTimers.Add(0f);
        }

        selectedLocation = null;
        showTowerMenu = false;
    }

    private void SpawnSoldiers(Vector3 towerLocation)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 position = towerLocation + new Vector3((i - 1) * 0.8f, 0f, 1.5f);
            position.y = TerrainHeight(position) + 1f;

            GameObject soldier = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            soldier.name = "Soldado";
            soldier.transform.position = position;
            soldier.transform.localScale = new Vector3(0.65f, 0.8f, 0.65f);
            SetColor(soldier, new Color(0.7f, 0.4f, 0.9f));

            Squad squad = soldier.AddComponent<Squad>();
            squad.isEnemy = false;
            squad.groundTerrain = terrain;
            squad.defense = 0f;
            squad.health = 30f;
            squad.speed = 2f;
            squad.attack = 5f;
            squad.attackRange = 1.2f;
            squad.MoveForward(position + Vector3.forward * 5f);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemyObject.name = "Enemigo " + (currentWave + 1) + "-" + (spawnedInWave + 1);
        enemyObject.transform.position = path[0] + Vector3.up;
        enemyObject.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
        SetColor(enemyObject, new Color(0.35f, 0.05f, 0.05f));

        Squad squad = enemyObject.AddComponent<Squad>();
        squad.isEnemy = true;
        squad.groundTerrain = terrain;
        squad.defense = 1f;
        squad.health = 40f;
        squad.speed = enemySpeed;
        squad.attack = 7f;
        squad.FollowForestPath(path.ToArray(), this);

        enemies.Add(new Enemy { gameObject = enemyObject, squad = squad });
        spawnedInWave++;
        if (spawnedInWave >= enemiesPerWave)
        {
            currentWave++;
            spawnedInWave = 0;
            spawnTimer = 0f;
        }
    }

    public void LoseLife()
    {
        baseHealth = Mathf.Max(0, baseHealth - 1);
        Debug.Log("Un enemigo llego al final. Vida restante: " + baseHealth);
    }

    public void RemoveEnemy(Squad squad)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            if (enemy.squad == squad)
            {
                Destroy(enemy.gameObject);
                enemies.RemoveAt(i);
                return;
            }
        }
    }

    private void AttackWithTowers(List<GameObject> towers, List<float> timers, bool areaAttack)
    {
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            if (Time.time < timers[i]) continue;

            Squad target = FindNearestEnemy(towers[i].transform.position);
            if (target == null) continue;

            Vector3 origin = towers[i].transform.position + Vector3.up * 0.7f;
            Vector3 direction = (target.transform.position - origin).normalized;
            GameObject shot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shot.name = areaAttack ? "Bomba" : "Proyectil";
            shot.transform.position = origin;
            shot.transform.localScale = Vector3.one * (areaAttack ? 0.4f : 0.2f);
            SetColor(shot, areaAttack ? Color.yellow : Color.red);

            Collider shotCollider = shot.GetComponent<Collider>();
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
        Squad nearest = null;
        float closestDistance = 8f;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.gameObject == null) continue;

            float distance = Vector3.Distance(position, enemy.gameObject.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = enemy.squad;
            }
        }

        return nearest;
    }

    private float TerrainHeight(Vector3 position)
    {
        return terrain.SampleHeight(position) + terrain.transform.position.y;
    }

    private void PlaceCamera()
    {
        Camera sceneCamera = FindFirstObjectByType<Camera>();
        if (sceneCamera == null) return;

        Vector3 center = (path[0] + path[path.Count - 1]) * 0.5f;
        float size = Mathf.Max(terrain.terrainData.size.x, terrain.terrainData.size.z);
        sceneCamera.transform.position = center + new Vector3(0f, size * 1.15f, -size * 0.45f);
        sceneCamera.transform.LookAt(center);
    }

    private void SetColor(GameObject objectToColor, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.color = color;
        objectToColor.GetComponent<Renderer>().material = material;
    }

    private void RemoveCollider(GameObject objectWithoutCollision)
    {
        Collider objectCollider = objectWithoutCollision.GetComponent<Collider>();
        if (objectCollider != null) Destroy(objectCollider);
    }

    private void OnDrawGizmosSelected()
    {
        if (path.Count < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 1; i < path.Count; i++)
            Gizmos.DrawLine(path[i - 1], path[i]);
    }
}
