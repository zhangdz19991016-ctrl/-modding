using System;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class HalfObsticle : MonoBehaviour
{
	// Token: 0x0600039E RID: 926 RVA: 0x0000FF78 File Offset: 0x0000E178
	private void Awake()
	{
		this.outline.enabled = false;
		this.defaultVisuals.SetActive(true);
		this.deadVisuals.SetActive(false);
		this.health.OnDeadEvent += this.Dead;
		if (this.airWallCollider)
		{
			this.airWallCollider.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000FFDE File Offset: 0x0000E1DE
	private void OnValidate()
	{
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0000FFE0 File Offset: 0x0000E1E0
	public void Dead(DamageInfo dmgInfo)
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.defaultVisuals.SetActive(false);
		this.deadVisuals.SetActive(true);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001000C File Offset: 0x0000E20C
	public void OnTriggerEnter(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.AddnearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = true;
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001004C File Offset: 0x0000E24C
	public void OnTriggerExit(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.RemoveNearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = false;
		}
	}

	// Token: 0x040002BA RID: 698
	public Outlinable outline;

	// Token: 0x040002BB RID: 699
	public HealthSimpleBase health;

	// Token: 0x040002BC RID: 700
	public List<GameObject> parts;

	// Token: 0x040002BD RID: 701
	public GameObject defaultVisuals;

	// Token: 0x040002BE RID: 702
	public GameObject deadVisuals;

	// Token: 0x040002BF RID: 703
	public Collider airWallCollider;

	// Token: 0x040002C0 RID: 704
	private bool dead;
}
