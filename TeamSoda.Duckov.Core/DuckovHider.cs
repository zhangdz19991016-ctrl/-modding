using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using FOW;

// Token: 0x0200017C RID: 380
public class DuckovHider : HiderBehavior
{
	// Token: 0x06000B98 RID: 2968 RVA: 0x0003167F File Offset: 0x0002F87F
	protected override void Awake()
	{
		base.Awake();
		LevelManager.OnMainCharacterDead += this.OnMainCharacterDie;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00031698 File Offset: 0x0002F898
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnMainCharacterDie;
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x000316AB File Offset: 0x0002F8AB
	protected override void OnHide()
	{
		if (!LevelManager.Instance || !LevelManager.Instance.IsRaidMap || this.mainCharacterDied)
		{
			return;
		}
		this.targetHide = true;
		this.HideDelay();
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x000316DC File Offset: 0x0002F8DC
	protected override void OnReveal()
	{
		this.targetHide = false;
		this.character.Show();
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000316F0 File Offset: 0x0002F8F0
	private UniTask HideDelay()
	{
		DuckovHider.<HideDelay>d__8 <HideDelay>d__;
		<HideDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<HideDelay>d__.<>4__this = this;
		<HideDelay>d__.<>1__state = -1;
		<HideDelay>d__.<>t__builder.Start<DuckovHider.<HideDelay>d__8>(ref <HideDelay>d__);
		return <HideDelay>d__.<>t__builder.Task;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00031733 File Offset: 0x0002F933
	private void OnMainCharacterDie(DamageInfo damageInfo)
	{
		this.mainCharacterDied = true;
		this.OnReveal();
	}

	// Token: 0x040009E9 RID: 2537
	public CharacterMainControl character;

	// Token: 0x040009EA RID: 2538
	private float hideDelay = 0.2f;

	// Token: 0x040009EB RID: 2539
	private bool targetHide;

	// Token: 0x040009EC RID: 2540
	private bool mainCharacterDied;
}
