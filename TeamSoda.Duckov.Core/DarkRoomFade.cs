using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class DarkRoomFade : MonoBehaviour
{
	// Token: 0x06000B89 RID: 2953 RVA: 0x00031398 File Offset: 0x0002F598
	public void StartFade()
	{
		this.started = true;
		base.enabled = true;
		this.startPos = CharacterMainControl.Main.transform.position;
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x000313BD File Offset: 0x0002F5BD
	private void Awake()
	{
		this.range = 0f;
		this.UpdateMaterial();
		if (!this.started)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x000313E0 File Offset: 0x0002F5E0
	private void Update()
	{
		if (!this.started)
		{
			base.enabled = false;
		}
		this.range += this.speed * Time.deltaTime;
		this.UpdateMaterial();
		if (this.range > this.maxRange)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00031430 File Offset: 0x0002F630
	private void UpdateMaterial()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_Range", this.range);
		materialPropertyBlock.SetVector("_CenterPos", this.startPos);
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00031488 File Offset: 0x0002F688
	private void Collect()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial.SetFloat("_Range", 0f);
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x000314D0 File Offset: 0x0002F6D0
	public void SetRenderers(bool enable)
	{
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = enable;
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000314FC File Offset: 0x0002F6FC
	public static void SetRenderersEnable(bool enable)
	{
		DarkRoomFade[] array = UnityEngine.Object.FindObjectsByType<DarkRoomFade>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetRenderers(enable);
		}
	}

	// Token: 0x040009DE RID: 2526
	public float maxRange = 100f;

	// Token: 0x040009DF RID: 2527
	public float speed = 20f;

	// Token: 0x040009E0 RID: 2528
	public Renderer[] renderers;

	// Token: 0x040009E1 RID: 2529
	private Vector3 startPos;

	// Token: 0x040009E2 RID: 2530
	private float range;

	// Token: 0x040009E3 RID: 2531
	private bool started;
}
