using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x020001BA RID: 442
public class FXPool : MonoBehaviour
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000D23 RID: 3363 RVA: 0x000374DC File Offset: 0x000356DC
	// (set) Token: 0x06000D24 RID: 3364 RVA: 0x000374E3 File Offset: 0x000356E3
	public static FXPool Instance { get; private set; }

	// Token: 0x06000D25 RID: 3365 RVA: 0x000374EB File Offset: 0x000356EB
	private void Awake()
	{
		FXPool.Instance = this;
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x000374F4 File Offset: 0x000356F4
	private void FixedUpdate()
	{
		if (this.poolsDic == null)
		{
			return;
		}
		foreach (FXPool.Pool pool in this.poolsDic.Values)
		{
			pool.Tick();
		}
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x00037554 File Offset: 0x00035754
	private FXPool.Pool GetOrCreatePool(ParticleSystem prefab)
	{
		if (this.poolsDic == null)
		{
			this.poolsDic = new Dictionary<ParticleSystem, FXPool.Pool>();
		}
		FXPool.Pool result;
		if (this.poolsDic.TryGetValue(prefab, out result))
		{
			return result;
		}
		FXPool.Pool pool = new FXPool.Pool(prefab, base.transform, null, null, null, null, true, 10, 100);
		this.poolsDic[prefab] = pool;
		return pool;
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x000375AA File Offset: 0x000357AA
	private static ParticleSystem Get(ParticleSystem prefab)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		return FXPool.Instance.GetOrCreatePool(prefab).Get();
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x000375CC File Offset: 0x000357CC
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00037624 File Offset: 0x00035824
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation, Color color)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.main.startColor = color;
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x04000B6B RID: 2923
	private Dictionary<ParticleSystem, FXPool.Pool> poolsDic;

	// Token: 0x020004D6 RID: 1238
	private class Pool
	{
		// Token: 0x06002770 RID: 10096 RVA: 0x0008F4F4 File Offset: 0x0008D6F4
		public Pool(ParticleSystem prefab, Transform parent, Action<ParticleSystem> onCreate = null, Action<ParticleSystem> onGet = null, Action<ParticleSystem> onRelease = null, Action<ParticleSystem> onDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 100)
		{
			this.prefab = prefab;
			this.parent = parent;
			this.pool = new ObjectPool<ParticleSystem>(new Func<ParticleSystem>(this.Create), new Action<ParticleSystem>(this.OnEntryGet), new Action<ParticleSystem>(this.OnEntryRelease), new Action<ParticleSystem>(this.OnEntryDestroy), collectionCheck, defaultCapacity, maxSize);
			this.onCreate = onCreate;
			this.onGet = onGet;
			this.onRelease = onRelease;
			this.onDestroy = onDestroy;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x0008F580 File Offset: 0x0008D780
		private ParticleSystem Create()
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.prefab, this.parent);
			Action<ParticleSystem> action = this.onCreate;
			if (action != null)
			{
				action(particleSystem);
			}
			return particleSystem;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x0008F5B2 File Offset: 0x0008D7B2
		public void OnEntryGet(ParticleSystem obj)
		{
			this.activeEntries.Add(obj);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x0008F5C0 File Offset: 0x0008D7C0
		public void OnEntryRelease(ParticleSystem obj)
		{
			this.activeEntries.Remove(obj);
			obj.gameObject.SetActive(false);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x0008F5DB File Offset: 0x0008D7DB
		public void OnEntryDestroy(ParticleSystem obj)
		{
			Action<ParticleSystem> action = this.onDestroy;
			if (action == null)
			{
				return;
			}
			action(obj);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x0008F5EE File Offset: 0x0008D7EE
		public ParticleSystem Get()
		{
			return this.pool.Get();
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x0008F5FB File Offset: 0x0008D7FB
		public void Release(ParticleSystem obj)
		{
			this.pool.Release(obj);
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x0008F60C File Offset: 0x0008D80C
		public void Tick()
		{
			List<ParticleSystem> list = new List<ParticleSystem>();
			foreach (ParticleSystem particleSystem in this.activeEntries)
			{
				if (!particleSystem.isPlaying)
				{
					list.Add(particleSystem);
				}
			}
			foreach (ParticleSystem obj in list)
			{
				this.Release(obj);
			}
		}

		// Token: 0x04001D13 RID: 7443
		private ParticleSystem prefab;

		// Token: 0x04001D14 RID: 7444
		private Transform parent;

		// Token: 0x04001D15 RID: 7445
		private ObjectPool<ParticleSystem> pool;

		// Token: 0x04001D16 RID: 7446
		private Action<ParticleSystem> onCreate;

		// Token: 0x04001D17 RID: 7447
		private Action<ParticleSystem> onGet;

		// Token: 0x04001D18 RID: 7448
		private Action<ParticleSystem> onRelease;

		// Token: 0x04001D19 RID: 7449
		private Action<ParticleSystem> onDestroy;

		// Token: 0x04001D1A RID: 7450
		private List<ParticleSystem> activeEntries = new List<ParticleSystem>();
	}
}
