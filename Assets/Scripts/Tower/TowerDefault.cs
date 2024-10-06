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

    private float _currentFireRate;

    protected override void Awake()
    {
        base.Awake();
        _currentFireRate = towerSoP.fireRate;
    }

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

        yield return new WaitForSeconds(towerSoP.shootAnimationDuration);

        GetBulletFromPool(); 
        turretAudio.PlayShootSound();
        
        yield return new WaitForSeconds(_currentFireRate);
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
            bullet.gameObject.SetActive(true);

            activeBullets.Add(bullet);
            bulletsPool.Remove(bullet);
        }

        bullet.SetTarget(lastKnownTarget.transform, towerSoP.damage, towerSoP.bulletSpeed);
        return bullet;
    }

    private void OnBulletDeactivated(Bullet bullet)
    {
        activeBullets.Remove(bullet);
        bulletsPool.Add(bullet);
    }

    public override void Upgrade()
    {
        p_currentUpgradePrice += towerSoP.upgradePriceAdd;
        if(_currentFireRate <= 0.5)
        {
            _currentFireRate = 0.5f;
        }
        else
        {
            _currentFireRate -= 0.5f;
        }
    }
}
