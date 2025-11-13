using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class OcclusionFadeManager : MonoBehaviour
{
	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0003242F File Offset: 0x0003062F
	public static OcclusionFadeManager Instance
	{
		get
		{
			if (!OcclusionFadeManager.instance)
			{
				OcclusionFadeManager.instance = UnityEngine.Object.FindFirstObjectByType<OcclusionFadeManager>();
			}
			return OcclusionFadeManager.instance;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003244C File Offset: 0x0003064C
	public float startFadeHeight
	{
		get
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (!main || !main.gameObject.activeInHierarchy)
			{
				return 0.25f;
			}
			return main.transform.position.y + 0.25f;
		}
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00032490 File Offset: 0x00030690
	private void Awake()
	{
		this.materialDic = new Dictionary<Material, Material>();
		this.aimOcclusionFadeChecker.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
		this.characterOcclusionFadeChecker.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
		this.SetShader();
		Shader.SetGlobalTexture("FadeNoiseTexture", this.fadeNoiseTexture);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x000324F2 File Offset: 0x000306F2
	private void OnValidate()
	{
		this.SetShader();
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x000324FC File Offset: 0x000306FC
	private void SetShader()
	{
		Shader.SetGlobalFloat(this.viewRangeHash, this.viewRange);
		Shader.SetGlobalFloat(this.viewFadeRangeHash, this.viewFadeRange);
		Shader.SetGlobalFloat(this.startFadeHeightHash, this.startFadeHeight);
		Shader.SetGlobalFloat(this.heightFadeRangeHash, this.heightFadeRange);
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00032550 File Offset: 0x00030750
	private void Update()
	{
		if (!this.character)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			this.character = LevelManager.Instance.MainCharacter;
			this.cam = LevelManager.Instance.GameCamera.renderCamera;
			return;
		}
		else
		{
			if (!OcclusionFadeManager.FadeEnabled)
			{
				Shader.SetGlobalVector(this.charactetrPosHash, Vector3.one * -9999f);
				Shader.SetGlobalVector(this.aimPosHash, Vector3.one * -9999f);
				if (this.aimOcclusionFadeChecker.gameObject.activeInHierarchy)
				{
					this.aimOcclusionFadeChecker.gameObject.SetActive(false);
				}
				if (this.characterOcclusionFadeChecker.gameObject.activeInHierarchy)
				{
					this.characterOcclusionFadeChecker.gameObject.SetActive(false);
				}
				return;
			}
			this.aimOcclusionFadeChecker.transform.position = LevelManager.Instance.InputManager.InputAimPoint;
			Vector3 normalized = (this.aimOcclusionFadeChecker.transform.position - this.cam.transform.position).normalized;
			this.aimOcclusionFadeChecker.transform.rotation = Quaternion.LookRotation(-normalized);
			Shader.SetGlobalVector(this.aimViewDirHash, normalized);
			Shader.SetGlobalVector(this.aimPosHash, this.aimOcclusionFadeChecker.transform.position);
			this.characterOcclusionFadeChecker.transform.position = this.character.transform.position;
			Vector3 normalized2 = (this.characterOcclusionFadeChecker.transform.position - this.cam.transform.position).normalized;
			this.characterOcclusionFadeChecker.transform.rotation = Quaternion.LookRotation(-normalized2);
			Shader.SetGlobalVector(this.characterViewDirHash, normalized2);
			Shader.SetGlobalFloat(this.startFadeHeightHash, this.startFadeHeight);
			Shader.SetGlobalVector(this.charactetrPosHash, this.character.transform.position);
			return;
		}
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00032770 File Offset: 0x00030970
	public Material GetMaskedMaterial(Material mat)
	{
		if (mat == null)
		{
			return null;
		}
		if (!this.supportedShaders.Contains(mat.shader))
		{
			return mat;
		}
		if (this.materialDic.ContainsKey(mat))
		{
			return this.materialDic[mat];
		}
		Material material = new Material(this.maskedShader);
		material.CopyPropertiesFromMaterial(mat);
		this.materialDic.Add(mat, material);
		return material;
	}

	// Token: 0x04000A1B RID: 2587
	private static OcclusionFadeManager instance;

	// Token: 0x04000A1C RID: 2588
	public OcclusionFadeChecker aimOcclusionFadeChecker;

	// Token: 0x04000A1D RID: 2589
	public OcclusionFadeChecker characterOcclusionFadeChecker;

	// Token: 0x04000A1E RID: 2590
	private CharacterMainControl character;

	// Token: 0x04000A1F RID: 2591
	private Camera cam;

	// Token: 0x04000A20 RID: 2592
	public Dictionary<Material, Material> materialDic;

	// Token: 0x04000A21 RID: 2593
	public List<Shader> supportedShaders;

	// Token: 0x04000A22 RID: 2594
	public Shader maskedShader;

	// Token: 0x04000A23 RID: 2595
	public Material testMat;

	// Token: 0x04000A24 RID: 2596
	[Range(0f, 4f)]
	public float viewRange;

	// Token: 0x04000A25 RID: 2597
	[Range(0f, 8f)]
	public float viewFadeRange;

	// Token: 0x04000A26 RID: 2598
	public Texture2D fadeNoiseTexture;

	// Token: 0x04000A27 RID: 2599
	public static bool FadeEnabled = true;

	// Token: 0x04000A28 RID: 2600
	public float heightFadeRange;

	// Token: 0x04000A29 RID: 2601
	private int aimViewDirHash = Shader.PropertyToID("OC_AimViewDir");

	// Token: 0x04000A2A RID: 2602
	private int aimPosHash = Shader.PropertyToID("OC_AimPos");

	// Token: 0x04000A2B RID: 2603
	private int characterViewDirHash = Shader.PropertyToID("OC_CharacterViewDir");

	// Token: 0x04000A2C RID: 2604
	private int charactetrPosHash = Shader.PropertyToID("OC_CharacterPos");

	// Token: 0x04000A2D RID: 2605
	private int viewRangeHash = Shader.PropertyToID("ViewRange");

	// Token: 0x04000A2E RID: 2606
	private int viewFadeRangeHash = Shader.PropertyToID("ViewFadeRange");

	// Token: 0x04000A2F RID: 2607
	private int startFadeHeightHash = Shader.PropertyToID("StartFadeHeight");

	// Token: 0x04000A30 RID: 2608
	private int heightFadeRangeHash = Shader.PropertyToID("HeightFadeRange");
}
