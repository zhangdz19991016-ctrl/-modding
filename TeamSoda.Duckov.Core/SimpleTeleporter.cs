using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x020000B0 RID: 176
public class SimpleTeleporter : InteractableBase
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001A39B File Offset: 0x0001859B
	public Transform TeleportPoint
	{
		get
		{
			if (!this.selfTeleportPoint)
			{
				return base.transform;
			}
			return this.selfTeleportPoint;
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001A3B7 File Offset: 0x000185B7
	protected override void Awake()
	{
		base.Awake();
		this.teleportVolume.gameObject.SetActive(false);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001A3D0 File Offset: 0x000185D0
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		this.Teleport(this.interactCharacter).Forget();
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0001A3F4 File Offset: 0x000185F4
	private UniTask Teleport(CharacterMainControl targetCharacter)
	{
		SimpleTeleporter.<Teleport>d__13 <Teleport>d__;
		<Teleport>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Teleport>d__.<>4__this = this;
		<Teleport>d__.targetCharacter = targetCharacter;
		<Teleport>d__.<>1__state = -1;
		<Teleport>d__.<>t__builder.Start<SimpleTeleporter.<Teleport>d__13>(ref <Teleport>d__);
		return <Teleport>d__.<>t__builder.Task;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001A440 File Offset: 0x00018640
	private UniTask VolumeFx(bool show, float time)
	{
		SimpleTeleporter.<VolumeFx>d__14 <VolumeFx>d__;
		<VolumeFx>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<VolumeFx>d__.<>4__this = this;
		<VolumeFx>d__.show = show;
		<VolumeFx>d__.time = time;
		<VolumeFx>d__.<>1__state = -1;
		<VolumeFx>d__.<>t__builder.Start<SimpleTeleporter.<VolumeFx>d__14>(ref <VolumeFx>d__);
		return <VolumeFx>d__.<>t__builder.Task;
	}

	// Token: 0x04000559 RID: 1369
	public Transform target;

	// Token: 0x0400055A RID: 1370
	[SerializeField]
	private Transform selfTeleportPoint;

	// Token: 0x0400055B RID: 1371
	[SerializeField]
	private SimpleTeleporter.TransitionTypes transitionType;

	// Token: 0x0400055C RID: 1372
	[FormerlySerializedAs("fxTime")]
	public float transitionTime = 0.28f;

	// Token: 0x0400055D RID: 1373
	private float delay = 0.3f;

	// Token: 0x0400055E RID: 1374
	public Volume teleportVolume;

	// Token: 0x0400055F RID: 1375
	private int fxShaderID = Shader.PropertyToID("TeleportFXStrength");

	// Token: 0x04000560 RID: 1376
	private bool blackScreen;

	// Token: 0x02000460 RID: 1120
	public enum TransitionTypes
	{
		// Token: 0x04001B34 RID: 6964
		volumeFx,
		// Token: 0x04001B35 RID: 6965
		blackScreen
	}
}
