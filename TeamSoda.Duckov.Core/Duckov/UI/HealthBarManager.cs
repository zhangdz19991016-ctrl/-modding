using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AC RID: 940
	public class HealthBarManager : MonoBehaviour
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000761BE File Offset: 0x000743BE
		public static HealthBarManager Instance
		{
			get
			{
				return HealthBarManager._instance;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x000761C8 File Offset: 0x000743C8
		private PrefabPool<HealthBar> PrefabPool
		{
			get
			{
				if (this._prefabPool == null)
				{
					this._prefabPool = new PrefabPool<HealthBar>(this.healthBarPrefab, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this._prefabPool;
			}
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00076206 File Offset: 0x00074406
		private void Awake()
		{
			if (HealthBarManager._instance == null)
			{
				HealthBarManager._instance = this;
			}
			this.UnregisterStaticEvents();
			this.RegisterStaticEvents();
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00076227 File Offset: 0x00074427
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0007622F File Offset: 0x0007442F
		private void RegisterStaticEvents()
		{
			Health.OnRequestHealthBar += this.Health_OnRequestHealthBar;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x00076242 File Offset: 0x00074442
		private void UnregisterStaticEvents()
		{
			Health.OnRequestHealthBar -= this.Health_OnRequestHealthBar;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00076258 File Offset: 0x00074458
		private HealthBar GetActiveHealthBar(Health health)
		{
			if (health == null)
			{
				return null;
			}
			return this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000762A0 File Offset: 0x000744A0
		private HealthBar CreateHealthBarFor(Health health, DamageInfo? damage = null)
		{
			if (health == null)
			{
				return null;
			}
			if (this.PrefabPool.ActiveEntries.FirstOrDefault((HealthBar e) => e.target == health))
			{
				Debug.Log("Health bar for " + health.name + " already exists");
				return null;
			}
			HealthBar newBar = this.PrefabPool.Get(null);
			newBar.Setup(health, damage, delegate
			{
				this.PrefabPool.Release(newBar);
			});
			return newBar;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0007634C File Offset: 0x0007454C
		private void Health_OnRequestHealthBar(Health health)
		{
			HealthBar activeHealthBar = this.GetActiveHealthBar(health);
			if (activeHealthBar != null)
			{
				activeHealthBar.RefreshOffset();
				return;
			}
			this.CreateHealthBarFor(health, null);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00076382 File Offset: 0x00074582
		public static void RequestHealthBar(Health health, DamageInfo? damage = null)
		{
			if (HealthBarManager.Instance == null)
			{
				return;
			}
			HealthBarManager.Instance.CreateHealthBarFor(health, damage);
		}

		// Token: 0x040016DE RID: 5854
		private static HealthBarManager _instance;

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		public HealthBar healthBarPrefab;

		// Token: 0x040016E0 RID: 5856
		private PrefabPool<HealthBar> _prefabPool;
	}
}
