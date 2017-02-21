using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour {


    public GameObject Projectile;
    public void ShootBullet()
    {
        var bullet = Instantiate(Projectile,
            transform.position,
            transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        Destroy(bullet, 2.0f);
    }

}
