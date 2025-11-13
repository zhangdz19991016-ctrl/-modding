using System;
using System.Collections.Generic;
using Duckov;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200007A RID: 122
public class HitMarker : MonoBehaviour
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060004A3 RID: 1187 RVA: 0x000152D0 File Offset: 0x000134D0
	// (remove) Token: 0x060004A4 RID: 1188 RVA: 0x00015304 File Offset: 0x00013504
	public static event Action OnHitMarker;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060004A5 RID: 1189 RVA: 0x00015338 File Offset: 0x00013538
	// (remove) Token: 0x060004A6 RID: 1190 RVA: 0x0001536C File Offset: 0x0001356C
	public static event Action OnKillMarker;

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060004A7 RID: 1191 RVA: 0x000153A0 File Offset: 0x000135A0
	private Camera MainCam
	{
		get
		{
			if (!this._cam)
			{
				if (LevelManager.Instance == null)
				{
					return null;
				}
				if (LevelManager.Instance.GameCamera == null)
				{
					return null;
				}
				this._cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			return this._cam;
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000153F8 File Offset: 0x000135F8
	private void Awake()
	{
		Health.OnHurt += this.OnHealthHitEvent;
		Health.OnDead += this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit += this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead += this.OnSimpleHealthKill;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001544C File Offset: 0x0001364C
	private void OnDestroy()
	{
		Health.OnHurt -= this.OnHealthHitEvent;
		Health.OnDead -= this.OnHealthKillEvent;
		HealthSimpleBase.OnSimpleHealthHit -= this.OnSimpleHealthHit;
		HealthSimpleBase.OnSimpleHealthDead -= this.OnSimpleHealthKill;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001549D File Offset: 0x0001369D
	private void OnHealthHitEvent(Health _health, DamageInfo dmgInfo)
	{
		if (dmgInfo.isFromBuffOrEffect)
		{
			return;
		}
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000154C0 File Offset: 0x000136C0
	private void OnHit(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		Vector3 v = this.MainCam.WorldToScreenPoint(dmgInfo.damagePoint);
		Vector2 v2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, v, null, out v2);
		base.transform.localPosition = Vector3.ClampMagnitude(v2, 10f);
		ItemAgent_Gun gun = CharacterMainControl.Main.GetGun();
		if (gun != null)
		{
			this.scatterOnHit = gun.CurrentScatter;
		}
		int stateHashName = flag ? (this.hitMarkerIndex ? this.critHash1 : this.critHash2) : (this.hitMarkerIndex ? this.hitHash1 : this.hitHash2);
		int shortNameHash = this.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		if (shortNameHash != this.killHash && shortNameHash != this.killCritHash)
		{
			this.hitMarkerIndex = !this.hitMarkerIndex;
			this.animator.CrossFade(stateHashName, 0.02f);
		}
		Action onHitMarker = HitMarker.OnHitMarker;
		if (onHitMarker != null)
		{
			onHitMarker();
		}
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostHitMarker(flag);
		}
		UnityEvent unityEvent = this.hitEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001563D File Offset: 0x0001383D
	private void OnHealthKillEvent(Health _health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00015648 File Offset: 0x00013848
	private void OnKill(DamageInfo dmgInfo)
	{
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			return;
		}
		if (dmgInfo.toDamageReceiver && dmgInfo.toDamageReceiver.IsMainCharacter)
		{
			return;
		}
		bool flag = (float)dmgInfo.crit > 0f;
		int stateHashName = flag ? this.killCritHash : this.killHash;
		this.animator.CrossFade(stateHashName, 0.02f);
		if (!dmgInfo.toDamageReceiver || !dmgInfo.toDamageReceiver.useSimpleHealth)
		{
			AudioManager.PostKillMarker(flag);
		}
		Action onKillMarker = HitMarker.OnKillMarker;
		if (onKillMarker != null)
		{
			onKillMarker();
		}
		UnityEvent unityEvent = this.killEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000156FD File Offset: 0x000138FD
	private void OnSimpleHealthHit(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		if (dmgInfo.damageValue <= 1.01f)
		{
			return;
		}
		this.OnHit(dmgInfo);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00015714 File Offset: 0x00013914
	private void OnSimpleHealthKill(HealthSimpleBase health, DamageInfo dmgInfo)
	{
		this.OnKill(dmgInfo);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00015720 File Offset: 0x00013920
	private void LateUpdate()
	{
		foreach (RectTransform rectTransform in this.hitMarkerImages)
		{
			rectTransform.anchoredPosition += rectTransform.anchoredPosition.normalized * this.scatterOnHit * 3f;
		}
	}

	// Token: 0x040003E1 RID: 993
	public UnityEvent hitEvent;

	// Token: 0x040003E2 RID: 994
	public UnityEvent killEvent;

	// Token: 0x040003E5 RID: 997
	public Animator animator;

	// Token: 0x040003E6 RID: 998
	private readonly int hitHash1 = Animator.StringToHash("HitMarkerHit1");

	// Token: 0x040003E7 RID: 999
	private readonly int hitHash2 = Animator.StringToHash("HitMarkerHit2");

	// Token: 0x040003E8 RID: 1000
	private readonly int critHash1 = Animator.StringToHash("HitMarkerCrit1");

	// Token: 0x040003E9 RID: 1001
	private readonly int critHash2 = Animator.StringToHash("HitMarkerCrit2");

	// Token: 0x040003EA RID: 1002
	private bool hitMarkerIndex;

	// Token: 0x040003EB RID: 1003
	private readonly int killHash = Animator.StringToHash("HitMarkerKill");

	// Token: 0x040003EC RID: 1004
	private readonly int killCritHash = Animator.StringToHash("HitMarkerKillCrit");

	// Token: 0x040003ED RID: 1005
	public List<RectTransform> hitMarkerImages;

	// Token: 0x040003EE RID: 1006
	private float scatterOnHit;

	// Token: 0x040003EF RID: 1007
	private Camera _cam;
}
