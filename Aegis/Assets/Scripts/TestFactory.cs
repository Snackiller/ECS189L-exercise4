using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class TestFactory : MonoBehaviour
{
    [SerializeField] private ProjectileFactory projectileFactory;
    [SerializeField] private Transform spawnPoint;

    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            int numProjectiles = Random.Range(5, 11);
            List<GameObject> projectiles = new List<GameObject>();

            for (int i = 1; i < numProjectiles; i++)
            {
                GameObject randomProjectile = projectileFactory.GenerateRandomProjectile();
                projectiles.Add(randomProjectile);
                randomProjectile.SetActive(false);
            }

            StartCoroutine(TestSchedule(projectiles));
        }
    }

    private IEnumerator TestSchedule(List<GameObject> projectiles)
    {
        foreach(GameObject projectile in projectiles)
        {
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation;
            projectile.SetActive(true);
            yield return new WaitForSeconds(controller.GetChargeDelay() + 1.0f);
        }
    }
}