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
    private Canvas enemyIconsCanvas;

    void Start()
    {
        // Initialize the list of enemies and enemy icons
        enemies = new List<Transform>();
        enemyIcons = new List<GameObject>();

        // Create a new canvas for the enemy icons
        enemyIconsCanvas = new GameObject("EnemyIconsCanvas").AddComponent<Canvas>();
        enemyIconsCanvas.gameObject.AddComponent<CanvasScaler>();
        enemyIconsCanvas.gameObject.AddComponent<GraphicRaycaster>();
        enemyIconsCanvas.transform.SetParent(radarImage.transform, false);
    }

    void Update()
    {
        // Remove all enemy icons from the previous frame
        foreach (GameObject enemyIcon in enemyIcons)
        {
            Destroy(enemyIcon);
        }

        // Get the player's look direction
        Vector3 playerLookDirection = playerTransform.forward;
        playerLookDirection.y = 0f; // Set y direction to 0 to prevent rotation on y-axis

        // Calculate the rotation angle based on the player's look direction
        float angle = Mathf.Atan2(playerLookDirection.x, playerLookDirection.z) * Mathf.Rad2Deg;

        // Rotate the radar image to match the player's look direction
        radarImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -angle));

        // Loop through all enemies and update their position and rotation on the radar
        foreach (Transform enemy in enemies)
        {
            Vector3 direction = enemy.position - playerTransform.position;
            float distance = direction.magnitude;

            // Check if enemy is within the radar's range
            if (distance <= radarImage.rectTransform.rect.width / 1.5)
            {
                // Calculate the position of the enemy icon on the radar
                float xPos = direction.normalized.x * distance;
                float yPos = direction.normalized.z * distance;

                // Instantiate a new enemy icon under the enemy icons canvas
                GameObject enemyIcon = Instantiate(enemyIconPrefab, enemyIconsCanvas.transform);

                // Set the position of the enemy icon on the radar
                enemyIcon.transform.localPosition = new Vector3(xPos, yPos, 0f);

                // Set the scale of the enemy icon
                float iconSize = .1f; // Change this value to adjust the size of the enemy icons
                enemyIcon.transform.localScale = new Vector3(iconSize, iconSize, iconSize);

                // Calculate the rotation angle of the enemy icon based on the player's look direction
                float iconAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // Rotate the enemy icon to face towards the player
                enemyIcon.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, iconAngle));

                // Add the enemy icon to the list of enemy icons
                enemyIcons.Add(enemyIcon);
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
