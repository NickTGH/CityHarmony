using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private static LTDescr delay;
	public string header;
	public string description;
	public void OnPointerEnter(PointerEventData eventData)
	{
		delay = LeanTween.delayedCall(0.5f, ()=> 
			{
				TooltipSystem.Show(description, header);
			});
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		LeanTween.cancel(delay.uniqueId);
		TooltipSystem.Hide();
	}

	private void OnMouseEnter()
	{
		delay = LeanTween.delayedCall(0.5f, () =>
		{
			TooltipSystem.Show(description, header);
		});
	}

	private void OnMouseExit()
	{
		LeanTween.cancel(delay.uniqueId);
		TooltipSystem.Hide();
	}
}
