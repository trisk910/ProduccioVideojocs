using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReloadingText : MonoBehaviour
{
    public float speed = 500000000f; // Change this to adjust the speed of color change

    private TMP_Text textComponent;
    private float timer;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        timer += Time.deltaTime * speed;
        float alpha = Mathf.Sin(timer) * 0.5f + 0.5f; // Calculate alpha value using sine wave
        textComponent.color = Color.Lerp(Color.white, Color.red, alpha); // Use Color.Lerp to smoothly transition between white and red
    }
}
