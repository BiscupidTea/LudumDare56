using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefault : BaseTower
{
    private List<Bullet> activeBullets = new List<Bullet>();
    private List<Bullet> bulletsPool = new List<Bullet>();

    [SerializeField] private TurretAudio turretAudio;
    
    private bool canShoot = true;
    private Transform lastKnownTarget;

    public Action OnShoot;

    protected override void Shoot(Transform _lastKnownTarget)
    {
        if (!canShoot) return;

        lastKnownTarget = _lastKnownTarget;
        StartCoroutine(ShootSequence());
    }

    private IEnumerator ShootSequence()
    {
        canShoot = false;
        OnShoot?.Invoke();

        yield return new WaitForSeconds(towerSo.shootAnimationDuration);

        GetBulletFromPool(); 
        turretAudio.PlayShootSound();
        
        yield return new WaitForSeconds(towerSo.fireRate);
        canShoot = true;
    }

    private Bullet GetBulletFromPool()
    {
        Bullet bullet;

        if (bulletsPool.Count == 0)
        {
            bullet = Instantiate(bulletPrefab, aimPoint.position, Quaternion.identity);
            bullet.OnDeactivated += OnBulletDeactivated;
            activeBullets.Add(bullet);
        }

        else
        {
            bullet = bulletsPool[0];

            bullet.transform.position = aimPoint.position;
            bullet.RotateTowardsTarget();
            bullet.gameObject.SetActive(true);

            activeBullets.Add(bullet);
            bulletsPool.Remove(bullet);
        }

        bullet.SetTarget(lastKnownTarget.transform, towerSo.damage, towerSo.bulletSpeed);
        return bullet;
    }

    private void OnBulletDeactivated(Bullet bullet)
    {
        activeBullets.Remove(bullet);
        bulletsPool.Add(bullet);
    }
}
