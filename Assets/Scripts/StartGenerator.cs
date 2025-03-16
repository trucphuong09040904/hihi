using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGenerator : MonoBehaviour
{
    public GameObject StartGo;
    public int MaxStarts;
    Color[] startColors =
    {
        new Color(0.5f, 0,5f, 1f),
        new Color(0, 1f, 1f),
        new Color (1f, 1f, 0),
        new Color ( 1f, 0 ,0), };
    // Start is called before the first frame update
    void Start()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        for (int i = 0; i < MaxStarts; i++)
        {
            GameObject start = (GameObject)Instantiate(StartGo);
            start.GetComponent<SpriteRenderer>().color = startColors[i % startColors.Length];
            start.transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            start.GetComponent<Start1>().speed = -(1f * Random.value + 0.5f);
            start.transform.parent = start.transform;



        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
