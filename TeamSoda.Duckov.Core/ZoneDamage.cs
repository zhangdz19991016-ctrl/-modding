using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000B8 RID: 184
[RequireComponent(typeof(Zone))]
public class ZoneDamage : MonoBehaviour
{
	// Token: 0x06000606 RID: 1542 RVA: 0x0001AE95 File Offset: 0x00019095
	private void Start()
	{
		if (this.zone == null)
		{
			this.zone = base.GetComponent<Zone>();
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001AEB4 File Offset: 0x000190B4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.timeSpace)
		{
			this.timer %= this.timeSpace;
			this.Damage();
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001AF04 File Offset: 0x00019104
	private void Damage()
	{
		foreach (Health health in this.zone.Healths)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (!(characterMainControl == null))
			{
				if (this.checkGasMask && characterMainControl.HasGasMask)
				{
					Item faceMaskItem = characterMainControl.GetFaceMaskItem();
					if (faceMaskItem && faceMaskItem.GetStat(this.hasMaskHash) != null)
					{
						faceMaskItem.Durability -= 0.1f * this.timeSpace;
					}
				}
				else if ((!this.checkElecProtection || characterMainControl.CharacterItem.GetStat(this.elecProtectionHash).Value <= 0.99f) && (!this.checkFireProtection || characterMainControl.CharacterItem.GetStat(this.fireProtectionHash).Value <= 0.99f))
				{
					this.damageInfo.fromCharacter = null;
					this.damageInfo.damagePoint = health.transform.position + Vector3.up * 0.5f;
					this.damageInfo.damageNormal = Vector3.up;
					health.Hurt(this.damageInfo);
				}
			}
		}
	}

	// Token: 0x0400058B RID: 1419
	public Zone zone;

	// Token: 0x0400058C RID: 1420
	public float timeSpace = 0.5f;

	// Token: 0x0400058D RID: 1421
	private float timer;

	// Token: 0x0400058E RID: 1422
	public DamageInfo damageInfo;

	// Token: 0x0400058F RID: 1423
	public bool checkGasMask;

	// Token: 0x04000590 RID: 1424
	public bool checkElecProtection;

	// Token: 0x04000591 RID: 1425
	public bool checkFireProtection;

	// Token: 0x04000592 RID: 1426
	private int hasMaskHash = "GasMask".GetHashCode();

	// Token: 0x04000593 RID: 1427
	private int elecProtectionHash = "ElecProtection".GetHashCode();

	// Token: 0x04000594 RID: 1428
	private int fireProtectionHash = "FireProtection".GetHashCode();
}
