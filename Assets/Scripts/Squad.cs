using UnityEngine;

public class Squad : MonoBehaviour
{
    [Header("Estadisticas")]
    public float defense = 0f;
    public float health = 30f;
    public float speed = 2f;
    public float attack = 5f;
    public float attackRange = 1.2f;
    public bool isEnemy;
    public Terrain groundTerrain;

    private Squad target;
    private float nextAttack;
    private Vector3 destination;
    private Vector3[] path;
    private int pathIndex;
    private BuildingPlacement baseBuilding;
    private FantasyForestDefense forestDefense;

    public void ResetEnemy(Vector3[] points, BuildingPlacement building)
    {
        target = null;
        nextAttack = 0f;
        pathIndex = 0;
        enabled = true;
        baseBuilding = building;
        forestDefense = null;
        path = points;
    }

    public void FollowPath(Vector3[] points, BuildingPlacement building)
    {
        path = points;
        pathIndex = 0;
        baseBuilding = building;
    }

    public void FollowForestPath(Vector3[] points, FantasyForestDefense defense)
    {
        target = null;
        nextAttack = 0f;
        pathIndex = 0;
        path = points;
        baseBuilding = null;
        forestDefense = defense;
    }

    public void MoveForward(Vector3 point)
    {
        destination = point;
        destination.y = transform.position.y;
    }

    private void Update()
    {
        if (isEnemy && path != null && path.Length > 0)
        {
            // Solo se detiene si hay un soldado a distancia de ataque.
            FindTarget();
            if (target != null && Vector3.Distance(transform.position, target.transform.position) <= attackRange)
            {
                if (Time.time >= nextAttack)
                {
                    target.TakeDamage(attack);
                    nextAttack = Time.time + 1f;
                }
            }
            else
            {
                MoveAlongPath();
            }
            return;
        }
        if (target == null || !target.gameObject.activeInHierarchy)
            FindTarget();

        if (target == null)
        {
            if (!isEnemy)
                transform.position = GroundPosition(Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime));
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRange)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            nextPosition.y = transform.position.y;
            transform.position = GroundPosition(nextPosition);
        }
        else if (Time.time >= nextAttack)
        {
            target.TakeDamage(attack);
            nextAttack = Time.time + 1f;
        }
    }

    private void MoveAlongPath()
    {
        Vector3 nextPoint = GroundPosition(path[pathIndex]);
        transform.position = GroundPosition(Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, nextPoint) > 0.05f) return;

        pathIndex++;
        if (pathIndex >= path.Length)
        {
            if (baseBuilding != null)
            {
                baseBuilding.LoseLife();
                baseBuilding.ReturnEnemy(this);
            }
            else if (forestDefense != null)
            {
                forestDefense.LoseLife();
                forestDefense.RemoveEnemy(this);
            }
        }
    }

    private Vector3 GroundPosition(Vector3 position)
    {
        if (groundTerrain != null)
            position.y = groundTerrain.SampleHeight(position) + groundTerrain.transform.position.y + 1f;
        return position;
    }

    private void FindTarget()
    {
        Squad[] units = FindObjectsByType<Squad>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (Squad unit in units)
        {
            if (unit == this || unit.isEnemy == isEnemy) continue;

            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = unit;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= Mathf.Max(0f, damage - defense);
        if (health <= 0f && isEnemy)
        {
            if (baseBuilding != null)
                baseBuilding.ReturnEnemy(this);
            else if (forestDefense != null)
                forestDefense.RemoveEnemy(this);
            else
                Destroy(gameObject);
        }
        else if (health <= 0f)
            Destroy(gameObject);
    }
}
