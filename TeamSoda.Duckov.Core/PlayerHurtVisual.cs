using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x0200018C RID: 396
public class PlayerHurtVisual : MonoBehaviour
{
	// Token: 0x06000BE4 RID: 3044 RVA: 0x00032D30 File Offset: 0x00030F30
	private void Update()
	{
		if (!this.inited)
		{
			this.TryInit();
			return;
		}
		this.value = Mathf.MoveTowards(this.value, 0f, Time.deltaTime * this.speed);
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00032D94 File Offset: 0x00030F94
	private void TryInit()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		this.mainCharacterHealth = main.Health;
		if (!this.mainCharacterHealth)
		{
			return;
		}
		this.mainCharacterHealth.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		this.inited = true;
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00032DF5 File Offset: 0x00030FF5
	private void OnDestroy()
	{
		if (this.mainCharacterHealth)
		{
			this.mainCharacterHealth.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00032E20 File Offset: 0x00031020
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue < 1.5f)
		{
			return;
		}
		if (!this.mainCharacterHealth || !PlayerHurtVisual.hurtVisualOn)
		{
			this.value = 0f;
		}
		else if (this.mainCharacterHealth.CurrentHealth / this.mainCharacterHealth.MaxHealth > 0.5f)
		{
			this.value = 0.5f;
		}
		else
		{
			this.value = 1f;
		}
		if (this.volume.weight != this.value)
		{
			this.volume.weight = this.value;
		}
	}

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	private Volume volume;

	// Token: 0x04000A40 RID: 2624
	[SerializeField]
	private float speed = 4f;

	// Token: 0x04000A41 RID: 2625
	private Health mainCharacterHealth;

	// Token: 0x04000A42 RID: 2626
	private bool inited;

	// Token: 0x04000A43 RID: 2627
	public static bool hurtVisualOn = true;

	// Token: 0x04000A44 RID: 2628
	private float value;
}
