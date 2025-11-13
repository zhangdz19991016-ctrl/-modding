using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class ShootRangeRing : MonoBehaviour
{
	// Token: 0x06000BED RID: 3053 RVA: 0x00032EFE File Offset: 0x000310FE
	private void Awake()
	{
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00032F00 File Offset: 0x00031100
	private void Update()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			this.character.OnHoldAgentChanged += this.OnAgentChanged;
			this.OnAgentChanged(this.character.CurrentHoldItemAgent);
			return;
		}
		if (this.ringRenderer.gameObject.activeInHierarchy && !this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00032F84 File Offset: 0x00031184
	private void LateUpdate()
	{
		if (!this.character)
		{
			return;
		}
		base.transform.rotation = Quaternion.LookRotation(this.character.CurrentAimDirection, Vector3.up);
		base.transform.position = this.character.transform.position;
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00032FDA File Offset: 0x000311DA
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnHoldAgentChanged -= this.OnAgentChanged;
		}
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00033000 File Offset: 0x00031200
	private void OnAgentChanged(DuckovItemAgent agent)
	{
		if (agent == null)
		{
			return;
		}
		this.gunAgent = this.character.GetGun();
		if (this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(true);
			this.ringRenderer.transform.localScale = Vector3.one * this.character.GetAimRange() * 0.5f;
			return;
		}
		this.ringRenderer.gameObject.SetActive(false);
	}

	// Token: 0x04000A48 RID: 2632
	private CharacterMainControl character;

	// Token: 0x04000A49 RID: 2633
	public MeshRenderer ringRenderer;

	// Token: 0x04000A4A RID: 2634
	private ItemAgent_Gun gunAgent;
}
