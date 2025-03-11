using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO; //this us our enemy's bullet prefab

    // Start is called before the first frame update
    void Start()
    {
        //fire an enemy bullet after 1 second
        Invoke("FireEnemyBullet", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function to fire an enemy bullet
    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find ("PlayerGO");

        if ((playerShip != null)) //if the player is not dead
        {
            //instantiate an enemy bullet
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);

            //set the bullet's initial position
            bullet.transform.position = transform.position;

            //compute the bullet's direction towards the player's ship
            Vector2 direction = playerShip.transform.position - bullet.transform.position;

            //set the bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}
