using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlanCotrollers : MonoBehaviour
{
    public GameObject[] Planet;
    // Start is called before the first frame update
    Queue<GameObject> availablePlanets = new Queue<GameObject>();
    void Start()
    {
        availablePlanets.Enqueue(Planet[0]);
        availablePlanets.Enqueue(Planet[1]);
        availablePlanets.Enqueue(Planet[2]);
        InvokeRepeating("MovePlanetDown", 0, 20f);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void MovePlanetDown()
    {
        {
            EuqueuePlanets();
            if (availablePlanets.Count == 0)
                return;
            GameObject aPlanet = availablePlanets.Dequeue();
            aPlanet.GetComponent<Planet>().isMoving = true;

        }
        void EuqueuePlanets()
        {
            foreach (GameObject aPlanet in Planet)
            {
                {
                    if ((aPlanet.transform.position.y < 0) && (!aPlanet.GetComponent<Planet>().isMoving))
                    {
                        aPlanet.GetComponent<Planet>().ResetPosition();
                        availablePlanets.Enqueue(aPlanet);
                    }
                }
            }
        }
    }
}

