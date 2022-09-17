using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    void Start()
    {
        RandomPostion();
    }

    public void RandomPostion()
    {
        float height = Utils.MainCamera.orthographicSize;
        float width = height / 9 * 16;

        transform.position = new Vector2(Random.Range(-width, width), Random.Range(-height, height));
    }
}