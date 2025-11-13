using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B7 RID: 183
[RequireComponent(typeof(Rigidbody))]
public class Zone : MonoBehaviour
{
	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001AD18 File Offset: 0x00018F18
	public HashSet<Health> Healths
	{
		get
		{
			return this.healths;
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001AD20 File Offset: 0x00018F20
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.healths = new HashSet<Health>();
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		this.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Register(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001AD88 File Offset: 0x00018F88
	private void OnDestroy()
	{
		if (this.setActiveByDistance)
		{
			SetActiveByPlayerDistance.Unregister(base.gameObject, this.sceneBuildIndex);
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001ADA4 File Offset: 0x00018FA4
	private void OnTriggerEnter(Collider other)
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (!this.healths.Contains(component))
		{
			this.healths.Add(component);
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001AE10 File Offset: 0x00019010
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		Health component = other.GetComponent<Health>();
		if (component == null)
		{
			return;
		}
		if (this.onlyPlayerTeam && component.team != Teams.player)
		{
			return;
		}
		if (this.healths.Contains(component))
		{
			this.healths.Remove(component);
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001AE72 File Offset: 0x00019072
	private void OnDisable()
	{
		this.healths.Clear();
	}

	// Token: 0x04000586 RID: 1414
	public bool onlyPlayerTeam;

	// Token: 0x04000587 RID: 1415
	private HashSet<Health> healths;

	// Token: 0x04000588 RID: 1416
	public bool setActiveByDistance = true;

	// Token: 0x04000589 RID: 1417
	private Rigidbody rb;

	// Token: 0x0400058A RID: 1418
	private int sceneBuildIndex = -1;
}
