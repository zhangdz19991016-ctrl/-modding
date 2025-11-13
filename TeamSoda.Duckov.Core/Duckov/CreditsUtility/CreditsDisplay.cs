using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x020002FE RID: 766
	public class CreditsDisplay : MonoBehaviour
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x0005B9E4 File Offset: 0x00059BE4
		private void ParseAndDisplay()
		{
			this.Reset();
			CreditsLexer creditsLexer = new CreditsLexer(this.content.text);
			this.BeginVerticalLayout(Array.Empty<string>());
			foreach (Token token in creditsLexer)
			{
				if (this.status.records.Count > 0)
				{
					Token token2 = this.status.records[this.status.records.Count - 1];
				}
				this.status.records.Add(token);
				switch (token.type)
				{
				case TokenType.Invalid:
					Debug.LogError("Invalid Token: " + token.text);
					break;
				case TokenType.End:
					goto IL_F4;
				case TokenType.String:
					this.DoText(token.text);
					break;
				case TokenType.Instructor:
					this.DoInstructor(token.text);
					break;
				case TokenType.EmptyLine:
					this.EndItem();
					break;
				}
			}
			IL_F4:
			this.EndLayout(Array.Empty<string>());
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0005BB00 File Offset: 0x00059D00
		private void EndItem()
		{
			if (this.status.activeItem)
			{
				this.status.activeItem = null;
				this.EndLayout(Array.Empty<string>());
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0005BB2C File Offset: 0x00059D2C
		private void BeginItem()
		{
			this.status.activeItem = this.BeginVerticalLayout(Array.Empty<string>());
			this.status.activeItem.SetLayoutSpacing(this.internalItemSpacing);
			this.status.activeItem.SetPreferredWidth(this.itemWidth);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0005BB7B File Offset: 0x00059D7B
		private void DoEmpty(params string[] elements)
		{
			UnityEngine.Object.Instantiate<EmptyEntry>(this.emptyPrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0005BB94 File Offset: 0x00059D94
		private void DoInstructor(string text)
		{
			string[] array = text.Split(' ', StringSplitOptions.None);
			if (array.Length < 1)
			{
				return;
			}
			string text2 = array[0];
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 3008443898U)
			{
				if (num <= 1811125385U)
				{
					if (num != 1031692888U)
					{
						if (num != 1811125385U)
						{
							return;
						}
						if (!(text2 == "Horizontal"))
						{
							return;
						}
						this.BeginHorizontalLayout(array);
						return;
					}
					else
					{
						if (!(text2 == "color"))
						{
							return;
						}
						this.DoColor(array);
						return;
					}
				}
				else if (num != 2163944795U)
				{
					if (num != 3008443898U)
					{
						return;
					}
					if (!(text2 == "image"))
					{
						return;
					}
					this.DoImage(array);
					return;
				}
				else
				{
					if (!(text2 == "Vertical"))
					{
						return;
					}
					this.BeginVerticalLayout(array);
					return;
				}
			}
			else if (num <= 3482547786U)
			{
				if (num != 3250860581U)
				{
					if (num != 3482547786U)
					{
						return;
					}
					if (!(text2 == "End"))
					{
						return;
					}
					this.EndLayout(Array.Empty<string>());
					return;
				}
				else
				{
					if (!(text2 == "Space"))
					{
						return;
					}
					this.DoEmpty(array);
					return;
				}
			}
			else if (num != 3876335077U)
			{
				if (num != 3909890315U)
				{
					if (num != 4127999362U)
					{
						return;
					}
					if (!(text2 == "s"))
					{
						return;
					}
					this.status.s = true;
					return;
				}
				else
				{
					if (!(text2 == "l"))
					{
						return;
					}
					this.status.l = true;
					return;
				}
			}
			else
			{
				if (!(text2 == "b"))
				{
					return;
				}
				this.status.b = true;
				return;
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0005BD04 File Offset: 0x00059F04
		private void DoImage(string[] elements)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			UnityEngine.Object.Instantiate<ImageEntry>(this.imagePrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0005BD38 File Offset: 0x00059F38
		private void DoColor(string[] elements)
		{
			if (elements.Length < 2)
			{
				return;
			}
			Color color;
			ColorUtility.TryParseHtmlString(elements[1], out color);
			this.status.color = color;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0005BD64 File Offset: 0x00059F64
		private void DoText(string text)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			TextEntry textEntry = UnityEngine.Object.Instantiate<TextEntry>(this.textPrefab, this.CurrentTransform);
			int size = 30;
			if (this.status.s)
			{
				size = 20;
			}
			if (this.status.l)
			{
				size = 40;
			}
			bool b = this.status.b;
			textEntry.Setup(text, this.status.color, size, b);
			this.status.Flush();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0005BDE8 File Offset: 0x00059FE8
		private Transform GetCurrentTransform()
		{
			if (this.status == null)
			{
				return this.rootContentTransform;
			}
			if (this.status.transforms.Count == 0)
			{
				return this.rootContentTransform;
			}
			return this.status.transforms.Peek();
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x0005BE22 File Offset: 0x0005A022
		private Transform CurrentTransform
		{
			get
			{
				return this.GetCurrentTransform();
			}
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0005BE2A File Offset: 0x0005A02A
		public void PushTransform(Transform trans)
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return;
			}
			this.status.transforms.Push(trans);
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0005BE54 File Offset: 0x0005A054
		public Transform PopTransform()
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return null;
			}
			if (this.status.transforms.Count == 0)
			{
				Debug.LogError("Nothing to pop. Makesure to match push and pop.", this);
				return null;
			}
			return this.status.transforms.Pop();
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0005BEA5 File Offset: 0x0005A0A5
		private void Awake()
		{
			if (this.setupOnAwake)
			{
				this.ParseAndDisplay();
			}
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0005BEB8 File Offset: 0x0005A0B8
		private void Reset()
		{
			while (base.transform.childCount > 0)
			{
				Transform child = base.transform.GetChild(0);
				child.SetParent(null);
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
			this.status = new CreditsDisplay.GenerationStatus();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0005BF14 File Offset: 0x0005A114
		private VerticalEntry BeginVerticalLayout(params string[] args)
		{
			VerticalEntry verticalEntry = UnityEngine.Object.Instantiate<VerticalEntry>(this.verticalPrefab, this.CurrentTransform);
			verticalEntry.Setup(args);
			verticalEntry.SetLayoutSpacing(this.mainSpacing);
			this.PushTransform(verticalEntry.transform);
			return verticalEntry;
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0005BF53 File Offset: 0x0005A153
		private void EndLayout(params string[] args)
		{
			if (this.status.activeItem != null)
			{
				this.EndItem();
			}
			this.PopTransform();
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0005BF78 File Offset: 0x0005A178
		private HorizontalEntry BeginHorizontalLayout(params string[] args)
		{
			HorizontalEntry horizontalEntry = UnityEngine.Object.Instantiate<HorizontalEntry>(this.horizontalPrefab, this.CurrentTransform);
			horizontalEntry.Setup(args);
			this.PushTransform(horizontalEntry.transform);
			return horizontalEntry;
		}

		// Token: 0x04001233 RID: 4659
		[SerializeField]
		private bool setupOnAwake;

		// Token: 0x04001234 RID: 4660
		[SerializeField]
		private TextAsset content;

		// Token: 0x04001235 RID: 4661
		[SerializeField]
		private Transform rootContentTransform;

		// Token: 0x04001236 RID: 4662
		[SerializeField]
		private float internalItemSpacing = 8f;

		// Token: 0x04001237 RID: 4663
		[SerializeField]
		private float mainSpacing = 16f;

		// Token: 0x04001238 RID: 4664
		[SerializeField]
		private float itemWidth = 350f;

		// Token: 0x04001239 RID: 4665
		[Header("Prefabs")]
		[SerializeField]
		private HorizontalEntry horizontalPrefab;

		// Token: 0x0400123A RID: 4666
		[SerializeField]
		private VerticalEntry verticalPrefab;

		// Token: 0x0400123B RID: 4667
		[SerializeField]
		private EmptyEntry emptyPrefab;

		// Token: 0x0400123C RID: 4668
		[SerializeField]
		private TextEntry textPrefab;

		// Token: 0x0400123D RID: 4669
		[SerializeField]
		private ImageEntry imagePrefab;

		// Token: 0x0400123E RID: 4670
		private CreditsDisplay.GenerationStatus status;

		// Token: 0x02000597 RID: 1431
		private class GenerationStatus
		{
			// Token: 0x060028EB RID: 10475 RVA: 0x0009755E File Offset: 0x0009575E
			public void Flush()
			{
				this.s = false;
				this.l = false;
				this.b = false;
				this.color = Color.white;
			}

			// Token: 0x0400201A RID: 8218
			public List<Token> records = new List<Token>();

			// Token: 0x0400201B RID: 8219
			public Stack<Transform> transforms = new Stack<Transform>();

			// Token: 0x0400201C RID: 8220
			public bool s;

			// Token: 0x0400201D RID: 8221
			public bool l;

			// Token: 0x0400201E RID: 8222
			public bool b;

			// Token: 0x0400201F RID: 8223
			public Color color = Color.white;

			// Token: 0x04002020 RID: 8224
			public VerticalEntry activeItem;
		}
	}
}
