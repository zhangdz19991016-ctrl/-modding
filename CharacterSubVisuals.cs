using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CharacterSubVisuals : MonoBehaviour
{
	// Token: 0x06000396 RID: 918 RVA: 0x0000FB9C File Offset: 0x0000DD9C
	private void InitLayers()
	{
		if (this.layerInited)
		{
			return;
		}
		this.layerInited = true;
		this.hiddenLayer = LayerMask.NameToLayer("SpecialCamera");
		this.showLayer = LayerMask.NameToLayer("Character");
		this.sodaLightShowLayer = LayerMask.NameToLayer("SodaLight");
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0000FBEC File Offset: 0x0000DDEC
	private void SetRenderers()
	{
		this.renderers.Clear();
		this.particles.Clear();
		this.lights.Clear();
		this.sodaPointLights.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			ParticleSystem component = renderer.GetComponent<ParticleSystem>();
			if (component)
			{
				this.particles.Add(component);
			}
			else
			{
				SodaPointLight component2 = renderer.GetComponent<SodaPointLight>();
				if (component2)
				{
					this.sodaPointLights.Add(component2);
				}
				else
				{
					this.renderers.Add(renderer);
				}
			}
		}
		foreach (Light item in base.GetComponentsInChildren<Light>(true))
		{
			this.lights.Add(item);
		}
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
	public void AddRenderer(Renderer renderer)
	{
		if (renderer == null || this.renderers.Contains(renderer))
		{
			return;
		}
		this.InitLayers();
		int layer = this.hidden ? this.hiddenLayer : this.showLayer;
		renderer.gameObject.layer = layer;
		this.renderers.Add(renderer);
		if (this.character)
		{
			this.character.RemoveVisual(this);
			this.character.AddSubVisuals(this);
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0000FD34 File Offset: 0x0000DF34
	public void SetRenderersHidden(bool _hidden)
	{
		this.hidden = _hidden;
		this.InitLayers();
		int layer = _hidden ? this.hiddenLayer : this.showLayer;
		int num = this.renderers.Count;
		for (int i = 0; i < num; i++)
		{
			if (this.renderers[i] == null)
			{
				this.renderers.RemoveAt(i);
				i--;
				num--;
			}
			else
			{
				this.renderers[i].gameObject.layer = layer;
			}
		}
		int num2 = this.particles.Count;
		for (int j = 0; j < num2; j++)
		{
			if (this.particles[j] == null)
			{
				this.particles.RemoveAt(j);
				j--;
				num2--;
			}
			else
			{
				this.particles[j].gameObject.layer = layer;
			}
		}
		int num3 = this.lights.Count;
		for (int k = 0; k < num3; k++)
		{
			Light light = this.lights[k];
			if (light == null)
			{
				this.lights.RemoveAt(k);
				k--;
				num3--;
			}
			else
			{
				light.gameObject.layer = layer;
				if (this.hidden)
				{
					light.cullingMask = 0;
				}
				else
				{
					light.cullingMask = -1;
				}
			}
		}
		int layer2 = _hidden ? this.hiddenLayer : this.sodaLightShowLayer;
		int num4 = this.sodaPointLights.Count;
		for (int l = 0; l < this.sodaPointLights.Count; l++)
		{
			if (this.sodaPointLights[l] == null)
			{
				this.sodaPointLights.RemoveAt(l);
				l--;
				num4--;
			}
			else
			{
				this.sodaPointLights[l].gameObject.layer = layer2;
			}
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000FF1C File Offset: 0x0000E11C
	private void OnTransformParentChanged()
	{
		CharacterMainControl componentInParent = base.GetComponentInParent<CharacterMainControl>(true);
		this.SetCharacter(componentInParent);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0000FF38 File Offset: 0x0000E138
	public void SetCharacter(CharacterMainControl newCharacter)
	{
		if (newCharacter != null)
		{
			newCharacter.AddSubVisuals(this);
			this.character = newCharacter;
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0000FF51 File Offset: 0x0000E151
	private void OnDestroy()
	{
		if (this.character != null)
		{
			this.character.RemoveVisual(this);
		}
	}

	// Token: 0x040002AD RID: 685
	private CharacterMainControl character;

	// Token: 0x040002AE RID: 686
	public List<Renderer> renderers;

	// Token: 0x040002AF RID: 687
	public List<ParticleSystem> particles;

	// Token: 0x040002B0 RID: 688
	public List<Light> lights;

	// Token: 0x040002B1 RID: 689
	public List<SodaPointLight> sodaPointLights;

	// Token: 0x040002B2 RID: 690
	private int hiddenLayer;

	// Token: 0x040002B3 RID: 691
	private int showLayer;

	// Token: 0x040002B4 RID: 692
	private int sodaLightShowLayer;

	// Token: 0x040002B5 RID: 693
	private bool hidden;

	// Token: 0x040002B6 RID: 694
	private bool layerInited;

	// Token: 0x040002B7 RID: 695
	public bool logWhenSetVisual;

	// Token: 0x040002B8 RID: 696
	public CharacterModel mainModel;

	// Token: 0x040002B9 RID: 697
	public bool debug;
}
