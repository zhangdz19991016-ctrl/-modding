using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

// Token: 0x02000186 RID: 390
public class NightVisionVisual : MonoBehaviour
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x0003212C File Offset: 0x0003032C
	public void Awake()
	{
		this.CollectRendererData();
		this.Refresh();
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003213A File Offset: 0x0003033A
	private void OnDestroy()
	{
		this.nightVisionType = 0;
		this.Refresh();
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0003214C File Offset: 0x0003034C
	private void CollectRendererData()
	{
		if (this.rendererData == null)
		{
			return;
		}
		for (int i = 0; i < this.rendererData.rendererFeatures.Count; i++)
		{
			if (this.rendererData.rendererFeatures[i].name == this.thermalCharacterRednerFeatureKey)
			{
				this.thermalCharacterRednerFeature = this.rendererData.rendererFeatures[i];
			}
			else if (this.rendererData.rendererFeatures[i].name == this.thermalBackgroundRednerFeatureKey)
			{
				this.thermalBackgroundRednerFeature = this.rendererData.rendererFeatures[i];
			}
		}
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x000321FC File Offset: 0x000303FC
	private void Update()
	{
		bool flag = false;
		int num = this.CheckNightVisionType();
		if (num >= this.nightVisionTypes.Length)
		{
			num = 1;
		}
		if (this.nightVisionType != num)
		{
			this.nightVisionType = num;
			flag = true;
		}
		if (LevelManager.LevelInited != this.levelInited)
		{
			this.levelInited = LevelManager.LevelInited;
			flag = true;
		}
		if (flag)
		{
			this.Refresh();
		}
		if (this.character && this.nightVisionLight.gameObject.activeInHierarchy)
		{
			this.nightVisionLight.transform.position = this.character.transform.position + Vector3.up * 2f;
		}
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000322A7 File Offset: 0x000304A7
	private int CheckNightVisionType()
	{
		if (!this.character)
		{
			if (LevelManager.LevelInited)
			{
				this.character = CharacterMainControl.Main;
			}
			return 0;
		}
		return Mathf.RoundToInt(this.character.NightVisionType);
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x000322DC File Offset: 0x000304DC
	public void Refresh()
	{
		bool flag = this.nightVisionType > 0;
		this.thermalVolume.gameObject.SetActive(flag);
		this.nightVisionLight.gameObject.SetActive(flag);
		NightVisionVisual.NightVisionType nightVisionType = this.nightVisionTypes[this.nightVisionType];
		bool flag2 = nightVisionType.thermalOn && flag;
		bool active = nightVisionType.thermalBackground && flag;
		this.thermalVolume.profile = nightVisionType.profile;
		this.thermalCharacterRednerFeature.SetActive(flag2);
		this.thermalBackgroundRednerFeature.SetActive(active);
		Shader.SetGlobalFloat("ThermalOn", flag2 ? 1f : 0f);
		if (LevelManager.LevelInited)
		{
			if (flag2)
			{
				LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
				return;
			}
			LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayers;
		}
	}

	// Token: 0x04000A0F RID: 2575
	private int nightVisionType;

	// Token: 0x04000A10 RID: 2576
	public Volume thermalVolume;

	// Token: 0x04000A11 RID: 2577
	public NightVisionVisual.NightVisionType[] nightVisionTypes;

	// Token: 0x04000A12 RID: 2578
	private CharacterMainControl character;

	// Token: 0x04000A13 RID: 2579
	public ScriptableRendererData rendererData;

	// Token: 0x04000A14 RID: 2580
	public List<string> renderFeatureNames;

	// Token: 0x04000A15 RID: 2581
	private ScriptableRendererFeature thermalCharacterRednerFeature;

	// Token: 0x04000A16 RID: 2582
	private ScriptableRendererFeature thermalBackgroundRednerFeature;

	// Token: 0x04000A17 RID: 2583
	public Transform nightVisionLight;

	// Token: 0x04000A18 RID: 2584
	public string thermalCharacterRednerFeatureKey = "ThermalCharacter";

	// Token: 0x04000A19 RID: 2585
	public string thermalBackgroundRednerFeatureKey = "ThermalBackground";

	// Token: 0x04000A1A RID: 2586
	private bool levelInited;

	// Token: 0x020004C2 RID: 1218
	[Serializable]
	public struct NightVisionType
	{
		// Token: 0x04001CCA RID: 7370
		public string intro;

		// Token: 0x04001CCB RID: 7371
		public VolumeProfile profile;

		// Token: 0x04001CCC RID: 7372
		[FormerlySerializedAs("thermalCharacter")]
		public bool thermalOn;

		// Token: 0x04001CCD RID: 7373
		public bool thermalBackground;
	}
}
