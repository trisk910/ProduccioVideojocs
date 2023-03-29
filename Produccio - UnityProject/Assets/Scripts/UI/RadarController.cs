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
    public float radarRange = 50f;
    public float enemyIconScale = 0.5f;

    void Start()
    {
        // Initialize the list of enemies and enemy icons
        enemies = new List<Transform>();
        enemyIcons = new List<GameObject>();
    }

    void LateUpdate()
    {
        // Remove all enemy icons from the previous frame
        foreach (GameObject enemyIcon in enemyIcons)
        {
            Destroy(enemyIcon);
        }

        // Loop through all enemies and update their position and rotation on the radar
        foreach (Transform enemy in enemies)
        {
            Vector3 direction = enemy.position - playerTransform.position;
            float angle = Vector3.SignedAngle(direction, playerTransform.forward, Vector3.up);
            float distance = direction.magnitude;

            // Check if enemy is within the radar's range
            if (distance <= radarRange)
            {
                // Calculate the position of the enemy icon on the radar
                float xPos = direction.normalized.x * distance * (radarImage.rectTransform.rect.width / (2 * radarRange));
                float yPos = direction.normalized.z * distance * (radarImage.rectTransform.rect.width / (2 * radarRange));

                // Calculate the rotation of the radar image
                float zAngle = -playerTransform.eulerAngles.y;

                // Instantiate a new enemy icon and set its position, rotation, and scale
                GameObject enemyIcon = Instantiate(enemyIconPrefab, radarImage.transform);
                enemyIcon.transform.localPosition = new Vector3(xPos, yPos, 0f);
                enemyIcon.transform.localEulerAngles = new Vector3(0f, 0f, angle + zAngle);
                enemyIcon.transform.localScale = Vector3.one * enemyIconScale;

                // Add the enemy icon to the list of enemy icons
                enemyIcons.Add(enemyIcon);
            }
        }

        // Set the rotation of the radar image
        radarImage.transform.localEulerAngles = new Vector3(0f, 0f, -playerTransform.eulerAngles.y);
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
