using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E0 RID: 224
public class OnTriggerEnterEvent : MonoBehaviour
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x000206FF File Offset: 0x0001E8FF
	private bool hideLayerMask
	{
		get
		{
			return this.onlyMainCharacter || this.filterByTeam;
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00020711 File Offset: 0x0001E911
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002071C File Offset: 0x0001E91C
	public void Init()
	{
		Collider component = base.GetComponent<Collider>();
		if (component)
		{
			component.isTrigger = true;
		}
		if (this.filterByTeam)
		{
			this.layerMask = 1 << LayerMask.NameToLayer("Character");
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00020761 File Offset: 0x0001E961
	private void OnCollisionEnter(Collision collision)
	{
		this.OnEvent(collision.gameObject, true);
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00020770 File Offset: 0x0001E970
	private void OnCollisionExit(Collision collision)
	{
		this.OnEvent(collision.gameObject, false);
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002077F File Offset: 0x0001E97F
	private void OnTriggerEnter(Collider other)
	{
		this.OnEvent(other.gameObject, true);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002078E File Offset: 0x0001E98E
	private void OnTriggerExit(Collider other)
	{
		this.OnEvent(other.gameObject, false);
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x000207A0 File Offset: 0x0001E9A0
	private void OnEvent(GameObject other, bool enter)
	{
		if (this.triggerOnce && this.triggered)
		{
			return;
		}
		if (this.onlyMainCharacter)
		{
			if (CharacterMainControl.Main == null || other != CharacterMainControl.Main.gameObject)
			{
				return;
			}
		}
		else
		{
			if ((1 << other.layer | this.layerMask) != this.layerMask)
			{
				return;
			}
			if (this.filterByTeam)
			{
				CharacterMainControl component = other.GetComponent<CharacterMainControl>();
				if (!component)
				{
					return;
				}
				Teams team = component.Team;
				if (!Team.IsEnemy(this.selfTeam, team))
				{
					return;
				}
			}
		}
		this.triggered = true;
		if (enter)
		{
			UnityEvent doOnTriggerEnter = this.DoOnTriggerEnter;
			if (doOnTriggerEnter == null)
			{
				return;
			}
			doOnTriggerEnter.Invoke();
			return;
		}
		else
		{
			UnityEvent doOnTriggerExit = this.DoOnTriggerExit;
			if (doOnTriggerExit == null)
			{
				return;
			}
			doOnTriggerExit.Invoke();
			return;
		}
	}

	// Token: 0x040006D2 RID: 1746
	public bool onlyMainCharacter;

	// Token: 0x040006D3 RID: 1747
	public bool filterByTeam;

	// Token: 0x040006D4 RID: 1748
	public Teams selfTeam;

	// Token: 0x040006D5 RID: 1749
	public LayerMask layerMask;

	// Token: 0x040006D6 RID: 1750
	public bool triggerOnce;

	// Token: 0x040006D7 RID: 1751
	public UnityEvent DoOnTriggerEnter = new UnityEvent();

	// Token: 0x040006D8 RID: 1752
	public UnityEvent DoOnTriggerExit = new UnityEvent();

	// Token: 0x040006D9 RID: 1753
	private bool triggered;

	// Token: 0x040006DA RID: 1754
	private bool mainCharacterIn;
}
