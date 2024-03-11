using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 5f; // Enemy movement speed
    public float chaseDistance = 10f; // Distance within which the enemy will chase the player
    private Transform player; // Reference to the player's transform

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within chase distance, move towards the player
        if (distanceToPlayer < chaseDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
