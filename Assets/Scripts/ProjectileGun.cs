using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    public GameObject bullet;
    
    public float shootForce, upwardForce;
    
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    
    public int magazineSize, bulletsPerTap;
    
    public bool allowButtonHold;
   
    int bulletsLeft, bulletsShot;
    
    public Rigidbody playerRb;
    
    public float recoilForce;
    
    bool shooting, readyToShoot, reloading;
    
    public Camera fpsCam;
    
    public Transform attackPoint;
    
    //public GameObject muzzleFlash;
    
    //public TextMeshProUGUI ammunitionDisplay;
    
    public bool allowInvoke = true;
    public ToolManager toolManager;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //if (ammunitionDisplay != null)
        //    ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        bullet.SetActive(true);
    }

    private void MyInput()
    {
        if (toolManager.isAxeEquipped)
        {
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
            if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = 0;
                Shoot();
            }

            bullet.SetActive(true);

        }

        
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Use the attackPoint as the ray origin
        Ray ray = new Ray(attackPoint.position, attackPoint.forward);

        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //currentBullet.transform.rotation = Quaternion.LookRotation(directionWithoutSpread.normalized, Vector3.up);

        // Constantly rotate the bullet on the Z-axis
        currentBullet.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 30f);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            playerRb.AddForce(-directionWithoutSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        StartCoroutine(DestroyProjectile(currentBullet, 5f));
    }

    private IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
