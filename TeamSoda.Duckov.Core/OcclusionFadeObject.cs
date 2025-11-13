using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200018A RID: 394
public class OcclusionFadeObject : MonoBehaviour
{
	// Token: 0x06000BD2 RID: 3026 RVA: 0x00032877 File Offset: 0x00030A77
	private void Collect()
	{
		this.CollectTriggers();
		this.CollectRenderers();
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00032888 File Offset: 0x00030A88
	private void CollectTriggers()
	{
		this.triggers = new OcclusionFadeTrigger[0];
		this.triggers = base.GetComponentsInChildren<OcclusionFadeTrigger>();
		if (this.triggers.Length != 0)
		{
			foreach (OcclusionFadeTrigger occlusionFadeTrigger in this.triggers)
			{
				occlusionFadeTrigger.parent = this;
				Collider[] componentsInChildren = occlusionFadeTrigger.GetComponentsInChildren<Collider>(true);
				if (componentsInChildren.Length != 0)
				{
					Collider[] array2 = componentsInChildren;
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j].isTrigger = true;
					}
				}
			}
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00032900 File Offset: 0x00030B00
	private void CollectRenderers()
	{
		this.topTransform = this.FindFirst(base.transform, this.topName);
		if (this.topTransform == null)
		{
			return;
		}
		this.renderers = this.topTransform.GetComponentsInChildren<Renderer>(true);
		this.originMaterials.Clear();
		foreach (Renderer renderer in this.renderers)
		{
			this.originMaterials.AddRange(renderer.sharedMaterials);
		}
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0003297B File Offset: 0x00030B7B
	public void OnEnter()
	{
		this.enterCounter++;
		this.Refresh();
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00032991 File Offset: 0x00030B91
	public void OnLeave()
	{
		this.enterCounter--;
		this.Refresh();
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x000329A8 File Offset: 0x00030BA8
	private void Refresh()
	{
		this.SyncEnable();
		if (!this.triggerEnabled)
		{
			this.hiding = false;
			this.Sync();
			return;
		}
		if (this.enterCounter > 0 && !this.hiding)
		{
			this.hiding = true;
			this.Sync();
			return;
		}
		if (this.enterCounter <= 0 && this.hiding)
		{
			this.hiding = false;
			this.Sync();
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00032A0E File Offset: 0x00030C0E
	private void OnEnable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00032A16 File Offset: 0x00030C16
	private void OnDisable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00032A20 File Offset: 0x00030C20
	private void SyncEnable()
	{
		if (this.triggerEnabled != base.enabled)
		{
			OcclusionFadeTrigger[] array = this.triggers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(base.enabled);
			}
			this.triggerEnabled = base.enabled;
		}
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00032A70 File Offset: 0x00030C70
	private void Sync()
	{
		this.SyncEnable();
		OcclusionFadeTypes occlusionFadeTypes = this.fadeType;
		if (occlusionFadeTypes != OcclusionFadeTypes.Fade)
		{
			if (occlusionFadeTypes != OcclusionFadeTypes.ShadowOnly)
			{
				return;
			}
			if (this.hiding)
			{
				foreach (Renderer renderer in this.renderers)
				{
					if (!(renderer == null))
					{
						renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
					}
				}
				return;
			}
			foreach (Renderer renderer2 in this.renderers)
			{
				if (!(renderer2 == null))
				{
					renderer2.shadowCastingMode = ShadowCastingMode.On;
				}
			}
			return;
		}
		else
		{
			if (this.tempMaterials == null)
			{
				this.tempMaterials = new List<Material>();
			}
			if (this.hiding)
			{
				int num = 0;
				foreach (Renderer renderer3 in this.renderers)
				{
					if (!(renderer3 == null))
					{
						this.tempMaterials.Clear();
						for (int j = 0; j < renderer3.materials.Length; j++)
						{
							Material mat = this.originMaterials[num];
							Material maskedMaterial = OcclusionFadeManager.Instance.GetMaskedMaterial(mat);
							this.tempMaterials.Add(maskedMaterial);
							num++;
						}
						renderer3.SetSharedMaterials(this.tempMaterials);
					}
				}
				return;
			}
			int num2 = 0;
			foreach (Renderer renderer4 in this.renderers)
			{
				if (!(renderer4 == null))
				{
					this.tempMaterials.Clear();
					for (int k = 0; k < renderer4.materials.Length; k++)
					{
						this.tempMaterials.Add(this.originMaterials[num2]);
						num2++;
					}
					renderer4.SetSharedMaterials(this.tempMaterials);
				}
			}
			return;
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00032C10 File Offset: 0x00030E10
	private void Hide()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00032C4C File Offset: 0x00030E4C
	private void Show()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00032C88 File Offset: 0x00030E88
	private Transform FindFirst(Transform root, string checkName)
	{
		for (int i = 0; i < root.childCount; i++)
		{
			Transform child = root.GetChild(i);
			if (child.name == checkName)
			{
				return child;
			}
			if (child.childCount > 0)
			{
				Transform transform = this.FindFirst(child, checkName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return null;
	}

	// Token: 0x04000A34 RID: 2612
	public OcclusionFadeTypes fadeType;

	// Token: 0x04000A35 RID: 2613
	public string topName = "Fade";

	// Token: 0x04000A36 RID: 2614
	public OcclusionFadeTrigger[] triggers;

	// Token: 0x04000A37 RID: 2615
	public Renderer[] renderers;

	// Token: 0x04000A38 RID: 2616
	public List<Material> originMaterials;

	// Token: 0x04000A39 RID: 2617
	private List<Material> tempMaterials;

	// Token: 0x04000A3A RID: 2618
	private Transform topTransform;

	// Token: 0x04000A3B RID: 2619
	private int enterCounter;

	// Token: 0x04000A3C RID: 2620
	private bool hiding;

	// Token: 0x04000A3D RID: 2621
	private bool triggerEnabled = true;
}
