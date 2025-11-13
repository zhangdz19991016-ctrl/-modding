using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000067 RID: 103
public class HurtVisual : MonoBehaviour
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001106B File Offset: 0x0000F26B
	public GameObject HitFx
	{
		get
		{
			if (!GameManager.BloodFxOn && this.hitFX_NoBlood != null)
			{
				return this.hitFX_NoBlood;
			}
			return this.hitFX;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060003DA RID: 986 RVA: 0x0001108F File Offset: 0x0000F28F
	public GameObject DeadFx
	{
		get
		{
			if (!GameManager.BloodFxOn && this.deadFx_NoBlood != null)
			{
				return this.deadFx_NoBlood;
			}
			return this.deadFx;
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x000110B4 File Offset: 0x0000F2B4
	public void SetHealth(Health _health)
	{
		if (this.useSimpleHealth)
		{
			return;
		}
		if (this.health != null)
		{
			this.health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
		this.health = _health;
		_health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		_health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		this.Init();
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001114C File Offset: 0x0000F34C
	private void Awake()
	{
		if (this.useSimpleHealth && this.simpleHealth != null)
		{
			this.simpleHealth.OnHurtEvent += this.OnHurt;
			this.simpleHealth.OnDeadEvent += this.OnDead;
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001119D File Offset: 0x0000F39D
	private void Init()
	{
	}

	// Token: 0x060003DE RID: 990 RVA: 0x000111A0 File Offset: 0x0000F3A0
	private void Update()
	{
		if (this.hurtValue > 0f)
		{
			this.SetRendererValue(this.hurtValue);
			this.hurtValue -= Time.unscaledDeltaTime * this.hurtCoolSpeed;
			if (this.hurtValue <= 0f)
			{
				this.SetRendererValue(0f);
			}
		}
	}

	// Token: 0x060003DF RID: 991 RVA: 0x000111F7 File Offset: 0x0000F3F7
	public void ForceHurt()
	{
		this.hurtValue = 1f;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00011204 File Offset: 0x0000F404
	private void OnHurt(DamageInfo dmgInfo)
	{
		bool flag = this.health && this.health.Hidden;
		if (this.HitFx && !flag)
		{
			PlayHurtEventProxy component = UnityEngine.Object.Instantiate<GameObject>(this.HitFx, dmgInfo.damagePoint, Quaternion.LookRotation(dmgInfo.damageNormal)).GetComponent<PlayHurtEventProxy>();
			if (component)
			{
				component.Play(dmgInfo.crit > 0);
			}
		}
		this.hurtValue = 1f;
		this.SetRendererValue(this.hurtValue);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00011290 File Offset: 0x0000F490
	private void SetRendererValue(float value)
	{
		int count = this.renderers.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(this.renderers[i] == null))
			{
				if (this.materialPropertyBlock == null)
				{
					this.materialPropertyBlock = new MaterialPropertyBlock();
				}
				this.renderers[i].GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetFloat(HurtVisual.hurtHash, value * this.hurtValueMultiplier);
				this.renderers[i].SetPropertyBlock(this.materialPropertyBlock);
			}
		}
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00011324 File Offset: 0x0000F524
	private void OnDead(DamageInfo dmgInfo)
	{
		if (this.DeadFx)
		{
			PlayHurtEventProxy component = UnityEngine.Object.Instantiate<GameObject>(this.DeadFx, base.transform.position, base.transform.rotation).GetComponent<PlayHurtEventProxy>();
			if (component)
			{
				component.Play(dmgInfo.crit > 0);
			}
		}
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001137C File Offset: 0x0000F57C
	private void OnDestroy()
	{
		if (this.health)
		{
			this.health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x000113CE File Offset: 0x0000F5CE
	private void AutoSet()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>(true).ToList<Renderer>();
		this.renderers.RemoveAll((Renderer e) => e == null || e.GetComponent<ParticleSystem>() != null);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001140D File Offset: 0x0000F60D
	public void SetRenderers(List<Renderer> _renderers)
	{
		this.renderers = _renderers;
	}

	// Token: 0x040002EF RID: 751
	public bool useSimpleHealth;

	// Token: 0x040002F0 RID: 752
	public HealthSimpleBase simpleHealth;

	// Token: 0x040002F1 RID: 753
	private Health health;

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private GameObject hitFX;

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private GameObject hitFX_NoBlood;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private GameObject deadFx;

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private GameObject deadFx_NoBlood;

	// Token: 0x040002F6 RID: 758
	public List<Renderer> renderers;

	// Token: 0x040002F7 RID: 759
	public static readonly int hurtHash = Shader.PropertyToID("_HurtValue");

	// Token: 0x040002F8 RID: 760
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x040002F9 RID: 761
	public float hurtCoolSpeed = 8f;

	// Token: 0x040002FA RID: 762
	public float hurtValueMultiplier = 1f;

	// Token: 0x040002FB RID: 763
	private float hurtValue;
}
