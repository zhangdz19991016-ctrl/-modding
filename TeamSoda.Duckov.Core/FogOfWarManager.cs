using System;
using FOW;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200017D RID: 381
public class FogOfWarManager : MonoBehaviour
{
	// Token: 0x06000B9F RID: 2975 RVA: 0x00031755 File Offset: 0x0002F955
	private void Start()
	{
		LevelManager.OnMainCharacterDead += this.OnCharacterDie;
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00031768 File Offset: 0x0002F968
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0003177B File Offset: 0x0002F97B
	private void Init()
	{
		this.inited = true;
		if (!LevelManager.Instance.IsRaidMap || !LevelManager.Rule.FogOfWar)
		{
			this.allVision = true;
		}
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x000317A4 File Offset: 0x0002F9A4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.character)
		{
			this.character = CharacterMainControl.Main;
			if (!this.character)
			{
				return;
			}
		}
		if (!this.inited)
		{
			this.Init();
		}
		if (!this.timeOfDayController)
		{
			this.timeOfDayController = LevelManager.Instance.TimeOfDayController;
			if (!this.timeOfDayController)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + Vector3.up * this.mianVisYOffset;
		this.mainVis.transform.position = vector;
		vector = new Vector3((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y), (float)Mathf.RoundToInt(vector.z));
		this.fogOfWar.UpdateWorldBounds(vector, new Vector3(128f, 1f, 128f));
		Vector3 forward = this.character.GetCurrentAimPoint() - this.character.transform.position;
		Debug.DrawLine(this.character.GetCurrentAimPoint(), this.character.GetCurrentAimPoint() + Vector3.up * 2f, Color.green, 0.2f);
		forward.y = 0f;
		forward.Normalize();
		float t = Mathf.Clamp01(this.character.NightVisionAbility + (this.character.FlashLight ? 0.3f : 0f));
		float num = this.character.ViewAngle;
		float num2 = this.character.SenseRange;
		float num3 = this.character.ViewDistance;
		num *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, t);
		num2 *= Mathf.Lerp(TimeOfDayController.NightSenseRangeFactor, 1f, t);
		num3 *= Mathf.Lerp(TimeOfDayController.NightViewDistanceFactor, 1f, t);
		if (num3 < num2 - 2.5f)
		{
			num3 = num2 - 2.5f;
		}
		if (this.allVision)
		{
			num = 360f;
			num2 = 50f;
			num3 = 50f;
		}
		if (num != this.viewAgnel)
		{
			if (this.viewAgnel < 0f)
			{
				this.viewAgnel = num;
			}
			this.viewAgnel = Mathf.MoveTowards(this.viewAgnel, num, 180f * Time.deltaTime);
			this.mainVis.ViewAngle = this.viewAgnel;
		}
		if (num2 != this.senseRange)
		{
			if (this.senseRange < 0f)
			{
				this.senseRange = num2;
			}
			this.senseRange = Mathf.MoveTowards(this.senseRange, num2, 15f * Time.deltaTime);
			this.mainVis.UnobscuredRadius = this.senseRange;
		}
		if (num3 != this.viewDistance)
		{
			if (this.viewDistance < 0f)
			{
				this.viewDistance = num3;
			}
			this.viewDistance = Mathf.MoveTowards(this.viewDistance, num3, 30f * Time.deltaTime);
			this.mainVis.ViewRadius = this.viewDistance;
		}
		this.mainVis.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00031AC9 File Offset: 0x0002FCC9
	private void OnCharacterDie(DamageInfo dmgInfo)
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040009ED RID: 2541
	[FormerlySerializedAs("mianVis")]
	public FogOfWarRevealer3D mainVis;

	// Token: 0x040009EE RID: 2542
	public float mianVisYOffset = 1f;

	// Token: 0x040009EF RID: 2543
	private CharacterMainControl character;

	// Token: 0x040009F0 RID: 2544
	public FogOfWarWorld fogOfWar;

	// Token: 0x040009F1 RID: 2545
	private float viewAgnel = -1f;

	// Token: 0x040009F2 RID: 2546
	private float senseRange = -1f;

	// Token: 0x040009F3 RID: 2547
	private float viewDistance = -1f;

	// Token: 0x040009F4 RID: 2548
	private TimeOfDayController timeOfDayController;

	// Token: 0x040009F5 RID: 2549
	private bool allVision;

	// Token: 0x040009F6 RID: 2550
	private bool inited;
}
