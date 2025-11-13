using System;
using TMPro;
using UnityEngine;

namespace FX
{
	// Token: 0x02000211 RID: 529
	public class PopTextEntity : MonoBehaviour
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0003EA35 File Offset: 0x0003CC35
		private RectTransform spriteRendererRectTransform
		{
			get
			{
				if (this._spriteRendererRectTransform_cache == null)
				{
					this._spriteRendererRectTransform_cache = this.spriteRenderer.GetComponent<RectTransform>();
				}
				return this._spriteRendererRectTransform_cache;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0003EA5C File Offset: 0x0003CC5C
		private TextMeshPro tmp
		{
			get
			{
				return this._tmp;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0003EA64 File Offset: 0x0003CC64
		public TextMeshPro Tmp
		{
			get
			{
				return this.tmp;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0003EA6C File Offset: 0x0003CC6C
		public Color EndColor
		{
			get
			{
				return this.endColor;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0003EA74 File Offset: 0x0003CC74
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0003EA7C File Offset: 0x0003CC7C
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.endColor = this.color;
				this.endColor.a = 0f;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0003EAA1 File Offset: 0x0003CCA1
		public float timeSinceSpawn
		{
			get
			{
				return Time.time - this.spawnTime;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0003EAAF File Offset: 0x0003CCAF
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x0003EABC File Offset: 0x0003CCBC
		private string text
		{
			get
			{
				return this.tmp.text;
			}
			set
			{
				this.tmp.text = value;
			}
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0003EACC File Offset: 0x0003CCCC
		public void SetupContent(string text, Sprite sprite = null)
		{
			this.text = text;
			if (sprite == null)
			{
				this.spriteRenderer.gameObject.SetActive(false);
				return;
			}
			this.spriteRenderer.gameObject.SetActive(true);
			this.spriteRenderer.sprite = sprite;
			this.spriteRenderer.transform.localScale = Vector3.one * (0.5f / (sprite.rect.width / sprite.pixelsPerUnit));
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0003EB4D File Offset: 0x0003CD4D
		internal void SetColor(Color newColor)
		{
			this.Tmp.color = newColor;
			this.spriteRenderer.color = newColor;
		}

		// Token: 0x04000CCB RID: 3275
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000CCC RID: 3276
		private RectTransform _spriteRendererRectTransform_cache;

		// Token: 0x04000CCD RID: 3277
		[SerializeField]
		private TextMeshPro _tmp;

		// Token: 0x04000CCE RID: 3278
		public Vector3 velocity;

		// Token: 0x04000CCF RID: 3279
		public float size;

		// Token: 0x04000CD0 RID: 3280
		private Color color;

		// Token: 0x04000CD1 RID: 3281
		private Color endColor;

		// Token: 0x04000CD2 RID: 3282
		public float spawnTime;
	}
}
