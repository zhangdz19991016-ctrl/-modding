using System;
using Duckov;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000132 RID: 306
public abstract class SkillBase : MonoBehaviour
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0002B0ED File Offset: 0x000292ED
	public float LastReleaseTime
	{
		get
		{
			return this.lastReleaseTime;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0002B0F5 File Offset: 0x000292F5
	public SkillContext SkillContext
	{
		get
		{
			return this.skillContext;
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0002B100 File Offset: 0x00029300
	public void ReleaseSkill(SkillReleaseContext releaseContext, CharacterMainControl from)
	{
		this.lastReleaseTime = Time.time;
		this.skillReleaseContext = releaseContext;
		this.fromCharacter = from;
		this.fromCharacter.UseStamina(this.staminaCost);
		if (this.hasReleaseSound && this.fromCharacter != null && this.onReleaseSound != "")
		{
			AudioManager.Post(this.onReleaseSound, from.gameObject);
		}
		this.OnRelease();
		Action onSkillReleasedEvent = this.OnSkillReleasedEvent;
		if (onSkillReleasedEvent == null)
		{
			return;
		}
		onSkillReleasedEvent();
	}

	// Token: 0x060009FA RID: 2554
	public abstract void OnRelease();

	// Token: 0x040008C3 RID: 2243
	public bool hasReleaseSound;

	// Token: 0x040008C4 RID: 2244
	public string onReleaseSound;

	// Token: 0x040008C5 RID: 2245
	public Sprite icon;

	// Token: 0x040008C6 RID: 2246
	public float staminaCost = 10f;

	// Token: 0x040008C7 RID: 2247
	public float coolDownTime = 1f;

	// Token: 0x040008C8 RID: 2248
	private float lastReleaseTime = -999f;

	// Token: 0x040008C9 RID: 2249
	[SerializeField]
	protected SkillContext skillContext;

	// Token: 0x040008CA RID: 2250
	protected SkillReleaseContext skillReleaseContext;

	// Token: 0x040008CB RID: 2251
	protected CharacterMainControl fromCharacter;

	// Token: 0x040008CC RID: 2252
	[HideInInspector]
	public Item fromItem;

	// Token: 0x040008CD RID: 2253
	public Action OnSkillReleasedEvent;
}
