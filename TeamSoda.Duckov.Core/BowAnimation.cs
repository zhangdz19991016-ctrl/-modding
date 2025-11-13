using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class BowAnimation : MonoBehaviour
{
	// Token: 0x06000B77 RID: 2935 RVA: 0x00030FDC File Offset: 0x0002F1DC
	private void Start()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent += this.OnShoot;
			this.gunAgent.OnLoadedEvent += this.OnLoaded;
			if (this.gunAgent.BulletCount > 0)
			{
				this.OnLoaded();
			}
		}
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00031039 File Offset: 0x0002F239
	private void OnDestroy()
	{
		if (this.gunAgent != null)
		{
			this.gunAgent.OnShootEvent -= this.OnShoot;
			this.gunAgent.OnLoadedEvent -= this.OnLoaded;
		}
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00031077 File Offset: 0x0002F277
	private void OnShoot()
	{
		this.animator.SetTrigger("Shoot");
		if (this.gunAgent.BulletCount <= 0)
		{
			this.animator.SetBool("Loaded", false);
		}
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x000310A8 File Offset: 0x0002F2A8
	private void OnLoaded()
	{
		this.animator.SetBool("Loaded", true);
	}

	// Token: 0x040009CC RID: 2508
	public ItemAgent_Gun gunAgent;

	// Token: 0x040009CD RID: 2509
	public Animator animator;

	// Token: 0x040009CE RID: 2510
	private int hash_Loaded = "Loaded".GetHashCode();

	// Token: 0x040009CF RID: 2511
	private int hash_Aiming = "Aiming".GetHashCode();

	// Token: 0x040009D0 RID: 2512
	private int hash_Shoot = "Shoot".GetHashCode();
}
