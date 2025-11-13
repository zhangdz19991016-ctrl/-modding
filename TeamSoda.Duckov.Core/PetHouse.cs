using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class PetHouse : MonoBehaviour
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000960 RID: 2400 RVA: 0x00029CE8 File Offset: 0x00027EE8
	public static PetHouse Instance
	{
		get
		{
			return PetHouse.instance;
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00029CEF File Offset: 0x00027EEF
	private void Awake()
	{
		PetHouse.instance = this;
		if (LevelManager.LevelInited)
		{
			this.OnLevelInited();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnLevelInited;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00029D16 File Offset: 0x00027F16
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnLevelInited;
		if (this.petTarget)
		{
			this.petTarget.SetStandBy(false, this.petTarget.transform.position);
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00029D54 File Offset: 0x00027F54
	private void OnLevelInited()
	{
		CharacterMainControl petCharacter = LevelManager.Instance.PetCharacter;
		petCharacter.SetPosition(this.petMarker.position);
		this.petTarget = petCharacter.GetComponentInChildren<PetAI>();
		if (this.petTarget != null)
		{
			this.petTarget.SetStandBy(true, this.petMarker.position);
		}
	}

	// Token: 0x04000868 RID: 2152
	private static PetHouse instance;

	// Token: 0x04000869 RID: 2153
	public Transform petMarker;

	// Token: 0x0400086A RID: 2154
	private PetAI petTarget;
}
