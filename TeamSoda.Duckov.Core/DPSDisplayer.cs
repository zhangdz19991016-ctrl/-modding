using System;
using TMPro;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class DPSDisplayer : MonoBehaviour
{
	// Token: 0x06000B91 RID: 2961 RVA: 0x00031545 File Offset: 0x0002F745
	private void Awake()
	{
		Health.OnHurt += this.OnHurt;
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00031558 File Offset: 0x0002F758
	private void Update()
	{
		if (Time.time - this.lastTimeMarker > 3f)
		{
			this.empty = true;
			this.totalDamage = 0f;
			this.RefreshDisplay();
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00031585 File Offset: 0x0002F785
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHurt;
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00031598 File Offset: 0x0002F798
	private void OnHurt(Health health, DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		this.totalDamage += dmgInfo.finalDamage;
		if (this.empty)
		{
			this.firstTimeMarker = Time.time;
			this.lastTimeMarker = Time.time;
			this.empty = false;
			return;
		}
		this.lastTimeMarker = Time.time;
		this.RefreshDisplay();
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0003160C File Offset: 0x0002F80C
	private void RefreshDisplay()
	{
		float num = this.CalculateDPS();
		this.dpsText.text = num.ToString("00000");
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00031638 File Offset: 0x0002F838
	private float CalculateDPS()
	{
		if (this.empty)
		{
			return 0f;
		}
		float num = this.lastTimeMarker - this.firstTimeMarker;
		if (num <= 0f)
		{
			return 0f;
		}
		return this.totalDamage / num;
	}

	// Token: 0x040009E4 RID: 2532
	[SerializeField]
	private TextMeshPro dpsText;

	// Token: 0x040009E5 RID: 2533
	private bool empty;

	// Token: 0x040009E6 RID: 2534
	private float totalDamage;

	// Token: 0x040009E7 RID: 2535
	private float firstTimeMarker;

	// Token: 0x040009E8 RID: 2536
	private float lastTimeMarker;
}
