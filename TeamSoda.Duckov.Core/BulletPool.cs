using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x0200010F RID: 271
public class BulletPool : MonoBehaviour
{
	// Token: 0x0600094B RID: 2379 RVA: 0x0002994B File Offset: 0x00027B4B
	private void Awake()
	{
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0002994D File Offset: 0x00027B4D
	public Projectile GetABullet(Projectile bulletPrefab)
	{
		return this.GetAPool(bulletPrefab).Get();
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0002995C File Offset: 0x00027B5C
	private ObjectPool<Projectile> GetAPool(Projectile pfb)
	{
		ObjectPool<Projectile> result;
		if (this.pools.TryGetValue(pfb, out result))
		{
			return result;
		}
		ObjectPool<Projectile> objectPool = new ObjectPool<Projectile>(() => this.CreateABulletInPool(pfb), new Action<Projectile>(this.OnGetABulletInPool), new Action<Projectile>(this.OnBulletRelease), null, true, 10, 10000);
		this.pools.Add(pfb, objectPool);
		return objectPool;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000299DC File Offset: 0x00027BDC
	private Projectile CreateABulletInPool(Projectile pfb)
	{
		Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(pfb);
		projectile.transform.SetParent(base.transform);
		ObjectPool<Projectile> apool = this.GetAPool(pfb);
		projectile.SetPool(apool);
		return projectile;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00029A0F File Offset: 0x00027C0F
	private void OnGetABulletInPool(Projectile bulletToGet)
	{
		bulletToGet.gameObject.SetActive(true);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00029A1D File Offset: 0x00027C1D
	private void OnBulletRelease(Projectile bulletToGet)
	{
		bulletToGet.transform.SetParent(base.transform);
		bulletToGet.gameObject.SetActive(false);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00029A3C File Offset: 0x00027C3C
	public bool Release(Projectile instance, Projectile prefab)
	{
		ObjectPool<Projectile> objectPool;
		if (this.pools.TryGetValue(prefab, out objectPool))
		{
			objectPool.Release(prefab);
			return true;
		}
		return false;
	}

	// Token: 0x0400085E RID: 2142
	public Dictionary<Projectile, ObjectPool<Projectile>> pools = new Dictionary<Projectile, ObjectPool<Projectile>>();
}
