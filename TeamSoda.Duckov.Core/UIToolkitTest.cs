using System;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x0200020B RID: 523
public class UIToolkitTest : MonoBehaviour
{
	// Token: 0x06000F59 RID: 3929 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
	private void Awake()
	{
		VisualElement visualElement = this.doc.rootVisualElement.Q("Button", null);
		CallbackEventHandler callbackEventHandler = this.doc.rootVisualElement.Q("Button2", null);
		visualElement.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButtonClicked), TrickleDown.NoTrickleDown);
		callbackEventHandler.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButton2Clicked), TrickleDown.NoTrickleDown);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0003CF1F File Offset: 0x0003B11F
	private void OnButton2Clicked(ClickEvent evt)
	{
		Debug.Log("Button 2 Clicked");
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0003CF2B File Offset: 0x0003B12B
	private void OnButtonClicked(ClickEvent evt)
	{
		Debug.Log("Button Clicked");
	}

	// Token: 0x04000C91 RID: 3217
	[SerializeField]
	private UIDocument doc;
}
