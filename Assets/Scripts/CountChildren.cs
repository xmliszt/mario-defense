using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountChildren : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 0)
        {
            FindObjectOfType<GameManager>().NextLevel();
        }
    }
}
