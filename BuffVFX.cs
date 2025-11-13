using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000177 RID: 375
public class BuffVFX : MonoBehaviour
{
	// Token: 0x06000B7C RID: 2940 RVA: 0x000310F3 File Offset: 0x0002F2F3
	private void Awake()
	{
		if (!this.buff)
		{
			this.buff = base.GetComponent<Buff>();
		}
		this.buff.OnSetupEvent.AddListener(new UnityAction(this.OnSetup));
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0003112C File Offset: 0x0002F32C
	private void OnSetup()
	{
		if (this.shockFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.shockFxInstance);
		}
		if (!this.buff || !this.buff.Character || !this.shockFxPfb)
		{
			return;
		}
		this.shockFxInstance = UnityEngine.Object.Instantiate<GameObject>(this.shockFxPfb, this.buff.Character.transform);
		this.shockFxInstance.transform.localPosition = this.offsetFromCharacter;
		this.shockFxInstance.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x000311CB File Offset: 0x0002F3CB
	private void OnDestroy()
	{
		if (this.shockFxInstance != null)
		{
			UnityEngine.Object.Destroy(this.shockFxInstance);
		}
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x000311E6 File Offset: 0x0002F3E6
	public void AutoSetup()
	{
		this.buff = base.GetComponent<Buff>();
	}

	// Token: 0x040009D1 RID: 2513
	public Buff buff;

	// Token: 0x040009D2 RID: 2514
	public GameObject shockFxPfb;

	// Token: 0x040009D3 RID: 2515
	private GameObject shockFxInstance;

	// Token: 0x040009D4 RID: 2516
	public Vector3 offsetFromCharacter;
}
