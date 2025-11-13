using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E9 RID: 489
public class Soda_Joysticks : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0003AB74 File Offset: 0x00038D74
	public bool Holding
	{
		get
		{
			return this.holding;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0003AB7C File Offset: 0x00038D7C
	public Vector2 InputValue
	{
		get
		{
			return this.inputValue;
		}
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0003AB84 File Offset: 0x00038D84
	private void Start()
	{
		this.joyImage.gameObject.SetActive(false);
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0003ABD7 File Offset: 0x00038DD7
	private void Update()
	{
		if (this.holding && !this.usable)
		{
			this.Revert();
		}
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0003ABEF File Offset: 0x00038DEF
	private void OnEnable()
	{
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
		this.triggeringCancle = false;
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0003AC18 File Offset: 0x00038E18
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		if (this.holding)
		{
			return;
		}
		this.holding = true;
		this.currentPointerID = eventData.pointerId;
		this.downPoint = eventData.position;
		this.verticalRes = Screen.height;
		this.joystickRangePixel = (float)this.verticalRes * this.joystickRangePercent;
		this.cancleRangePixel = (float)this.verticalRes * this.cancleRangePercent;
		if (!this.fixedPositon)
		{
			this.backGround.transform.position = this.downPoint;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.joyImage.gameObject.SetActive(true);
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, true);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 1f;
		}
		if (this.canCancle && this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0.12f;
		}
		this.triggeringCancle = false;
		UnityEvent onTouchEvent = this.OnTouchEvent;
		if (onTouchEvent == null)
		{
			return;
		}
		onTouchEvent.Invoke();
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0003AD64 File Offset: 0x00038F64
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		UnityEvent<bool> onUpEvent = this.OnUpEvent;
		if (onUpEvent != null)
		{
			onUpEvent.Invoke(!this.triggeringCancle);
		}
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding && this.currentPointerID == eventData.pointerId)
		{
			this.Revert();
		}
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0003ADC8 File Offset: 0x00038FC8
	private void Revert()
	{
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding)
		{
			UnityEvent<bool> onUpEvent = this.OnUpEvent;
			if (onUpEvent != null)
			{
				onUpEvent.Invoke(false);
			}
		}
		if (!this.usable)
		{
			return;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.inputValue = Vector2.zero;
		this.holding = false;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (this.joyImage.gameObject.activeSelf)
		{
			this.joyImage.gameObject.SetActive(false);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0003AEB3 File Offset: 0x000390B3
	public void CancleTouch()
	{
		this.Revert();
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0003AEBB File Offset: 0x000390BB
	public void OnDisable()
	{
		this.Revert();
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0003AEC4 File Offset: 0x000390C4
	public void OnDrag(PointerEventData eventData)
	{
		if (this.holding && eventData.pointerId == this.currentPointerID)
		{
			Vector2 vector = eventData.position;
			if (vector == this.downPoint)
			{
				this.inputValue = Vector2.zero;
				return;
			}
			float num = Vector2.Distance(vector, this.downPoint);
			float d = num;
			Vector2 normalized = (vector - this.downPoint).normalized;
			if (num > this.joystickRangePixel)
			{
				if (this.followFinger)
				{
					this.downPoint += (num - this.joystickRangePixel) * normalized;
				}
				if (!this.fixedPositon && this.followFinger)
				{
					this.backGround.transform.position = this.downPoint;
				}
				d = this.joystickRangePixel;
			}
			vector = this.downPoint + normalized * d;
			Vector2 vector2 = Vector2.zero;
			if (this.joystickRangePixel > 0f)
			{
				vector2 = normalized * d / this.joystickRangePixel;
			}
			this.joyImage.transform.position = this.backGround.transform.position + normalized * d;
			Vector3 vector3 = Vector3.zero;
			vector3.y = -vector2.x;
			vector3.x = vector2.y;
			vector3 *= this.rotValue;
			this.backGround.transform.rotation = Quaternion.Euler(vector3);
			float num2 = vector2.magnitude;
			num2 = Mathf.InverseLerp(this.deadZone, this.fullZone, num2);
			this.inputValue = num2 * normalized;
			UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
			if (updateValueEvent != null)
			{
				updateValueEvent.Invoke(this.inputValue, true);
			}
			if (this.canCancle && this.cancleRangeCanvasGroup)
			{
				if (num >= this.cancleRangePixel)
				{
					this.cancleRangeCanvasGroup.alpha = 1f;
					this.triggeringCancle = true;
					return;
				}
				this.cancleRangeCanvasGroup.alpha = 0.12f;
				this.triggeringCancle = false;
			}
		}
	}

	// Token: 0x04000C03 RID: 3075
	public bool usable = true;

	// Token: 0x04000C04 RID: 3076
	private int verticalRes;

	// Token: 0x04000C05 RID: 3077
	[Range(0f, 0.5f)]
	public float joystickRangePercent = 0.3f;

	// Token: 0x04000C06 RID: 3078
	[Range(0f, 0.5f)]
	public float cancleRangePercent = 0.4f;

	// Token: 0x04000C07 RID: 3079
	public bool fixedPositon = true;

	// Token: 0x04000C08 RID: 3080
	public bool followFinger;

	// Token: 0x04000C09 RID: 3081
	public bool canCancle;

	// Token: 0x04000C0A RID: 3082
	private float joystickRangePixel;

	// Token: 0x04000C0B RID: 3083
	private float cancleRangePixel;

	// Token: 0x04000C0C RID: 3084
	[SerializeField]
	private Transform backGround;

	// Token: 0x04000C0D RID: 3085
	[SerializeField]
	private Image joyImage;

	// Token: 0x04000C0E RID: 3086
	[SerializeField]
	private CanvasGroup cancleRangeCanvasGroup;

	// Token: 0x04000C0F RID: 3087
	private bool holding;

	// Token: 0x04000C10 RID: 3088
	private Vector2 downPoint;

	// Token: 0x04000C11 RID: 3089
	private int currentPointerID;

	// Token: 0x04000C12 RID: 3090
	private Vector2 inputValue;

	// Token: 0x04000C13 RID: 3091
	[SerializeField]
	private float rotValue = 10f;

	// Token: 0x04000C14 RID: 3092
	[Range(0f, 1f)]
	public float deadZone;

	// Token: 0x04000C15 RID: 3093
	[Range(0f, 1f)]
	public float fullZone = 1f;

	// Token: 0x04000C16 RID: 3094
	public bool hideWhenNotTouch;

	// Token: 0x04000C17 RID: 3095
	public CanvasGroup canvasGroup;

	// Token: 0x04000C18 RID: 3096
	private bool triggeringCancle;

	// Token: 0x04000C19 RID: 3097
	public UnityEvent<Vector2, bool> UpdateValueEvent;

	// Token: 0x04000C1A RID: 3098
	public UnityEvent OnTouchEvent;

	// Token: 0x04000C1B RID: 3099
	public UnityEvent<bool> OnUpEvent;
}
