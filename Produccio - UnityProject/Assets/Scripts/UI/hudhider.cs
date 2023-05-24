using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hudhider : MonoBehaviour
{
    
    public GameObject MiniMap1;
    public GameObject MiniMap2;

    public GameObject Canvas;

    private bool hideAll = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            if(!hideAll)
            {
                Canvas.SetActive(false);
                MiniMap1.SetActive(false);
                MiniMap2.SetActive(false);
                hideAll = true;
            }
            else
            {
                Canvas.SetActive(true);
                MiniMap1.SetActive(true);
                MiniMap2.SetActive(true);
                hideAll = false;
            }
        }
    }
}
