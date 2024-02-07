using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
	public TextMeshProUGUI headerField;

	public TextMeshProUGUI descriptionField;

	public LayoutElement layoutElement;

	public int characterWrapLimit;

	public RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}
	public void SetText(string description, string header = "")
	{
		if (string.IsNullOrEmpty(header))
		{
			headerField.gameObject.SetActive(false);
		}
		else
		{
			headerField.gameObject.SetActive(true);
			headerField.text = header;
		}

		descriptionField.text = description;

		layoutElement.enabled = Mathf.Max(headerField.preferredWidth, descriptionField.preferredWidth) >= layoutElement.preferredWidth;
	}
	private void Update()
	{
		if (Application.isEditor)
		{

			layoutElement.enabled = Mathf.Max(headerField.preferredWidth, descriptionField.preferredWidth) >= layoutElement.preferredWidth;
		}

		Vector2 position = Input.mousePosition;

		float pivotX = position.x / Screen.width;
		float pivotY = position.y / Screen.height;

		float finalPivotX = 0f;
		float finalPivotY = 0f;

		if (pivotX < 0.5)
		{
			finalPivotX = -0.1f;
		}
		else
		{
			finalPivotX = 1.01f;
		}

		if (pivotY < 0.5)
		{
			finalPivotY = 0;
		}
		else
		{
			finalPivotY = 1;
		}

		rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);
		transform.position = position;
	}
}
