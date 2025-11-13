using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class MoveVisual : MonoBehaviour
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00032015 File Offset: 0x00030215
	private CharacterMainControl Character
	{
		get
		{
			if (!this.characterModel)
			{
				return null;
			}
			return this.characterModel.characterMainControl;
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00032034 File Offset: 0x00030234
	private void Awake()
	{
		foreach (ParticleSystem particleSystem in this.runParticles)
		{
			particleSystem.emission.enabled = this.running;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00032094 File Offset: 0x00030294
	private void Update()
	{
		if (!this.Character)
		{
			return;
		}
		if (this.Character.Running != this.running)
		{
			this.running = this.Character.Running;
			foreach (ParticleSystem particleSystem in this.runParticles)
			{
				particleSystem.emission.enabled = this.running;
			}
		}
	}

	// Token: 0x04000A0C RID: 2572
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000A0D RID: 2573
	public List<ParticleSystem> runParticles;

	// Token: 0x04000A0E RID: 2574
	private bool running;
}
