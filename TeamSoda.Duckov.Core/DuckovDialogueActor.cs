using System;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class DuckovDialogueActor : MonoBehaviour, IDialogueActor
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000369C5 File Offset: 0x00034BC5
	private static List<DuckovDialogueActor> ActiveActors
	{
		get
		{
			if (DuckovDialogueActor._activeActors == null)
			{
				DuckovDialogueActor._activeActors = new List<DuckovDialogueActor>();
			}
			return DuckovDialogueActor._activeActors;
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x000369DD File Offset: 0x00034BDD
	public static void Register(DuckovDialogueActor actor)
	{
		if (DuckovDialogueActor.ActiveActors.Contains(actor))
		{
			Debug.Log("Actor " + actor.nameKey + " 在重复注册", actor);
			return;
		}
		DuckovDialogueActor.ActiveActors.Add(actor);
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00036A13 File Offset: 0x00034C13
	public static void Unregister(DuckovDialogueActor actor)
	{
		DuckovDialogueActor.ActiveActors.Remove(actor);
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00036A24 File Offset: 0x00034C24
	public static DuckovDialogueActor Get(string id)
	{
		return DuckovDialogueActor.ActiveActors.Find((DuckovDialogueActor e) => e.ID == id);
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00036A54 File Offset: 0x00034C54
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00036A5C File Offset: 0x00034C5C
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00036A64 File Offset: 0x00034C64
	public string NameKey
	{
		get
		{
			return this.nameKey;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00036A6C File Offset: 0x00034C6C
	public Texture2D portrait
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00036A6F File Offset: 0x00034C6F
	public Sprite portraitSprite
	{
		get
		{
			return this._portraitSprite;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00036A78 File Offset: 0x00034C78
	public Color dialogueColor
	{
		get
		{
			return default(Color);
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00036A90 File Offset: 0x00034C90
	public Vector3 dialoguePosition
	{
		get
		{
			return default(Vector3);
		}
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x00036AA6 File Offset: 0x00034CA6
	private void OnEnable()
	{
		DuckovDialogueActor.Register(this);
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00036AAE File Offset: 0x00034CAE
	private void OnDisable()
	{
		DuckovDialogueActor.Unregister(this);
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x00036ABE File Offset: 0x00034CBE
	string IDialogueActor.get_name()
	{
		return base.name;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00036AC6 File Offset: 0x00034CC6
	Transform IDialogueActor.get_transform()
	{
		return base.transform;
	}

	// Token: 0x04000B3F RID: 2879
	private static List<DuckovDialogueActor> _activeActors;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private string id;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private Sprite _portraitSprite;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	[LocalizationKey("Default")]
	private string nameKey;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private Vector3 offset;
}
