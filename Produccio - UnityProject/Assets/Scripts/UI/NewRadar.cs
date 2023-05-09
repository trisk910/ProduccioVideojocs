using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRadar : MonoBehaviour
{
    public Camera radarCamera;
    public Transform playerTransform;
    public GameObject enemyIconPrefab;
    public List<Transform> enemies;
    private List<GameObject> enemyIcons;

    void Start()
    {
        // Initialize the list of enemies and enemy icons
        enemies = new List<Transform>();
        enemyIcons = new List<GameObject>();
    }

    void Update()
    {
        // Remove all enemy icons from the previous frame
        foreach (GameObject enemyIcon in enemyIcons)
        {
            Destroy(enemyIcon);
        }

        // Loop through all enemies and update their position on the radar
        foreach (Transform enemy in enemies)
        {
            Vector3 direction = enemy.position - playerTransform.position;
            float distance = direction.magnitude;

            // Check if enemy is within the radar's range
            if (distance <= radarCamera.orthographicSize)
            {
                // Calculate the position of the enemy icon on the radar
                Vector3 viewportPosition = radarCamera.WorldToViewportPoint(enemy.position);

                // Check if the enemy is in front of the player
                if (viewportPosition.z > 0f)
                {
                    Vector2 radarSize = GetComponent<RectTransform>().sizeDelta;
                    float xPos = (viewportPosition.x - 0.5f) * radarSize.x;
                    float yPos = (viewportPosition.y - 0.5f) * radarSize.y;

                    // Instantiate a new enemy icon and set its position
                    GameObject enemyIcon = Instantiate(enemyIconPrefab);
                    enemyIcon.transform.SetParent(transform);
                    enemyIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

                    // Add the enemy icon to the list of enemy icons
                    enemyIcons.Add(enemyIcon);
                }
            }
        }
    }

    // Method to add a new enemy to the radar
    public void AddEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }

    // Method to remove an enemy from the radar
    public void RemoveEnemy(Transform enemyTransform)
    {
        enemies.Remove(enemyTransform);
    }
}