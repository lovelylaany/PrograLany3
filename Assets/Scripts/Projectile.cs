using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float explosionRadius = 0f;
    public bool isAreaAttack;
    private float lifeTime = 6f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Squad unit = collision.collider.GetComponent<Squad>();
        if (unit != null)
        {
            if (!isAreaAttack)
            {
                unit.TakeDamage(damage);
                Destroy(gameObject);
            }
            return;
        }

        if (isAreaAttack && (collision.collider is TerrainCollider || collision.collider.gameObject.name == "Suelo"))
            Explode();
        else if (!isAreaAttack)
            Destroy(gameObject);
    }

    private void Explode()
    {
        Squad[] units = FindObjectsByType<Squad>(FindObjectsSortMode.None);
        foreach (Squad unit in units)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance <= explosionRadius) unit.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
