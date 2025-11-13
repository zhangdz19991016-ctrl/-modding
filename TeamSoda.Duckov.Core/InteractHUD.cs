using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class InteractHUD : MonoBehaviour
{
	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001BFA4 File Offset: 0x0001A1A4
	private PrefabPool<InteractSelectionHUD> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<InteractSelectionHUD>(this.selectionPrefab, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0001BFDD File Offset: 0x0001A1DD
	private void Awake()
	{
		this.interactableGroup = new List<InteractableBase>();
		this.selectionsHUD = new List<InteractSelectionHUD>();
		this.selectionPrefab.gameObject.SetActive(false);
		this.master.gameObject.SetActive(false);
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0001C018 File Offset: 0x0001A218
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
		}
		if (this.camera == null)
		{
			this.camera = Camera.main;
			if (this.camera == null)
			{
				return;
			}
		}
		bool flag = false;
		bool flag2 = false;
		this.interactableMaster = this.characterMainControl.interactAction.MasterInteractableAround;
		bool flag3 = InputManager.InputActived && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running);
		Shader.SetGlobalFloat(this.interactableHash, flag3 ? 1f : 0f);
		this.interactable = (this.interactableMaster != null && flag3);
		if (this.interactable)
		{
			if (this.interactableMaster != this.interactableMasterTemp)
			{
				this.interactableMasterTemp = this.interactableMaster;
				flag = true;
				flag2 = true;
			}
			if (this.interactableIndexTemp != this.characterMainControl.interactAction.InteractIndexInGroup)
			{
				this.interactableIndexTemp = this.characterMainControl.interactAction.InteractIndexInGroup;
				flag2 = true;
			}
		}
		else
		{
			this.interactableMasterTemp = null;
		}
		if (this.interactable != this.master.gameObject.activeInHierarchy)
		{
			this.master.gameObject.SetActive(this.interactable);
		}
		if (flag)
		{
			this.RefreshContent();
			this.SyncPos();
		}
		if (flag2)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0001C19F File Offset: 0x0001A39F
	private void LateUpdate()
	{
		if (this.characterMainControl == null)
		{
			return;
		}
		if (this.camera == null)
		{
			return;
		}
		this.SyncPos();
		this.UpdateInteractLine();
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0001C1CC File Offset: 0x0001A3CC
	private void SyncPos()
	{
		if (!this.syncPosToTarget)
		{
			return;
		}
		if (!this.interactableMaster)
		{
			return;
		}
		Vector3 position = this.interactableMaster.transform.TransformPoint(this.interactableMaster.interactMarkerOffset);
		Vector3 v = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(position);
		Vector2 v2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, v, null, out v2);
		base.transform.localPosition = v2;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0001C254 File Offset: 0x0001A454
	private void RefreshContent()
	{
		if (this.interactableMaster == null)
		{
			return;
		}
		this.selectionsHUD.Clear();
		this.interactableGroup.Clear();
		foreach (InteractableBase interactableBase in this.interactableMaster.GetInteractableList())
		{
			if (interactableBase != null)
			{
				this.interactableGroup.Add(interactableBase);
			}
		}
		this.Selections.ReleaseAll();
		foreach (InteractableBase interactableBase2 in this.interactableGroup)
		{
			InteractSelectionHUD interactSelectionHUD = this.Selections.Get(null);
			interactSelectionHUD.transform.SetAsLastSibling();
			interactSelectionHUD.SetInteractable(interactableBase2, this.interactableGroup.Count > 1);
			this.selectionsHUD.Add(interactSelectionHUD);
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001C36C File Offset: 0x0001A56C
	private void RefreshSelection()
	{
		InteractableBase interactTarget = this.characterMainControl.interactAction.InteractTarget;
		foreach (InteractSelectionHUD interactSelectionHUD in this.selectionsHUD)
		{
			if (interactSelectionHUD.InteractTarget == interactTarget)
			{
				interactSelectionHUD.SetSelection(true);
			}
			else
			{
				interactSelectionHUD.SetSelection(false);
			}
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0001C3F4 File Offset: 0x0001A5F4
	private void UpdateInteractLine()
	{
	}

	// Token: 0x040005E1 RID: 1505
	private CharacterMainControl characterMainControl;

	// Token: 0x040005E2 RID: 1506
	public RectTransform master;

	// Token: 0x040005E3 RID: 1507
	private InteractableBase interactableMaster;

	// Token: 0x040005E4 RID: 1508
	private InteractableBase interactableMasterTemp;

	// Token: 0x040005E5 RID: 1509
	private List<InteractableBase> interactableGroup;

	// Token: 0x040005E6 RID: 1510
	private List<InteractSelectionHUD> selectionsHUD;

	// Token: 0x040005E7 RID: 1511
	private int interactableIndexTemp;

	// Token: 0x040005E8 RID: 1512
	private bool interactable;

	// Token: 0x040005E9 RID: 1513
	private Camera camera;

	// Token: 0x040005EA RID: 1514
	public bool syncPosToTarget;

	// Token: 0x040005EB RID: 1515
	public InteractSelectionHUD selectionPrefab;

	// Token: 0x040005EC RID: 1516
	private int interactableHash = Shader.PropertyToID("Interactable");

	// Token: 0x040005ED RID: 1517
	private PrefabPool<InteractSelectionHUD> _selectionsCache;
}
