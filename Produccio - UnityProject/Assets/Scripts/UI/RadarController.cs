using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{
    public Image radarImage;
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
            float angle = Vector3.Angle(direction, playerTransform.forward);
            float distance = direction.magnitude;

            // Check if enemy is within the radar's range
            if (distance <= radarImage.rectTransform.rect.width / 2)
            {
                // Calculate the position of the enemy icon on the radar
                float xPos = direction.normalized.x * distance;
                float yPos = direction.normalized.z * distance;

                // Instantiate a new enemy icon and set its position
                GameObject enemyIcon = Instantiate(enemyIconPrefab);
                enemyIcon.transform.SetParent(radarImage.transform);
                enemyIcon.transform.localPosition = new Vector3(xPos, yPos, 0f);

                // Add the enemy icon to the list of enemy icons
                enemyIcons.Add(enemyIcon);
            }
        }

        // Calculate the rotation angle of the radar image based on the player's forward direction
        float radarRotationAngle = -playerTransform.eulerAngles.y;

        // Set the rotation of the radar image
        radarImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, radarRotationAngle);
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