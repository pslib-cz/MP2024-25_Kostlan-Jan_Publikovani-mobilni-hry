using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Minigames
{
	public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
	{
		public WireController wireManager;
		public Color wireColor;
		public Image connectedWireImage;
		public Sprite emptyWireSprite;
		public Sprite connectedWireSprite;
		public bool isConnected = false;

		public GameObject wireImageConnecting;
		private GameObject currentDraggedWire;

		private RectTransform draggingPlane;
		private Vector3 startPoint;
		private Image wireImage;
		private int originalSiblingIndex;

		private Wire connectedWire;
		[SerializeField] private Color dropColor;

		[SerializeField] private float scaleFactor;

	  private void Start()
	  {
		 // Dynamický výpočet scaleFactor na základě velikosti obrazovky nebo jiných parametrů
		 RectTransform canvasRect = draggingPlane != null ? draggingPlane : transform.parent as RectTransform;

		 startPoint = transform.position;
		 wireImage = GetComponent<Image>();
		 wireImage.color = wireColor;
		 scaleFactor = (float)((canvasRect.rect.width / Screen.width) * 8f);
		 originalSiblingIndex = transform.GetSiblingIndex();
	  }


	  private void Awake()
		{
			wireManager = FindFirstObjectByType<WireController>();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (isConnected) return;

			originalSiblingIndex = transform.GetSiblingIndex();
			transform.SetSiblingIndex(2);
			GetComponent<Image>().raycastTarget = false;

			var canvas = FindInParents<Canvas>(gameObject);
			if (canvas == null) return;

			draggingPlane = canvas.transform as RectTransform;

			currentDraggedWire = Instantiate(wireImageConnecting, transform.parent);
			currentDraggedWire.transform.position = startPoint;
			currentDraggedWire.tag = "WireImageConnecting";

			currentDraggedWire.transform.SetAsFirstSibling();

			Image wireImage = currentDraggedWire.GetComponent<Image>();
			if (wireImage != null)
			{
				wireImage.color = wireColor;
			}

			SetDraggedPosition(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (isConnected) return;

			GetComponent<Image>().raycastTarget = true;
			transform.position = startPoint;
			transform.SetSiblingIndex(originalSiblingIndex);

			if (currentDraggedWire != null)
			{
				Destroy(currentDraggedWire);
				currentDraggedWire = null;
			}
		}

		public void OnDrag(PointerEventData data)
		{
			if (isConnected) return;

			SetDraggedPosition(data);
		}

		private void SetDraggedPosition(PointerEventData data)
		{
			Vector3 globalMousePos;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out globalMousePos))
			{
				UpdateWire(globalMousePos);
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			var draggedWire = eventData.pointerDrag.GetComponent<Wire>();

			if (draggedWire != null && draggedWire != this && !isConnected && !draggedWire.isConnected)
			{
				Vector3 direction = draggedWire.transform.position - transform.position;

				if (wireColor == draggedWire.wireColor || true)
				{
					Debug.Log("Dráty propojeny.");
					draggedWire.ConnectWire(this);
					dropColor = draggedWire.wireColor;
					ConnectWire(draggedWire);

					if (draggedWire.wireColor == wireColor)
					{
						wireManager.CheckWinning(1);
					}
				}
			}

			GetComponent<Image>().raycastTarget = true;

			if (currentDraggedWire != null)
			{
				Destroy(currentDraggedWire);
				currentDraggedWire = null;
			}
		}

		public void ConnectWire(Wire targetWire)
		{
			isConnected = true;
			targetWire.isConnected = true;

			wireImage.sprite = connectedWireSprite;
			targetWire.wireImage.sprite = connectedWireSprite;

			connectedWire = targetWire;
		}

		public void DisconnectWire()
		{
			if (isConnected && connectedWire != null)
			{
				Debug.Log("Dráty odpojeny.");
				isConnected = false;
				connectedWire.isConnected = false;

				wireImage.sprite = emptyWireSprite;
				connectedWire.wireImage.sprite = emptyWireSprite;

				if (connectedWire.wireColor == wireColor)
				{
					wireManager.CheckWinning(-1);
				}

				GameObject wireToRemove = FindWireImageConnectingByColor(dropColor);
				if (wireToRemove != null)
				{
					Destroy(wireToRemove);
					Debug.Log("Odstraněno wireImageConnecting s barvou: " + wireColor);
				}

				connectedWire.isConnected = false;
				connectedWire.gameObject.GetComponent<Image>().raycastTarget = true;
				connectedWire = null;

			}
		}

		private GameObject FindWireImageConnectingByColor(Color color)
		{
			GameObject[] allWires = GameObject.FindGameObjectsWithTag("WireImageConnecting");
			foreach (GameObject wire in allWires)
			{
				Image wireImage = wire.GetComponent<Image>();
				if (wireImage != null && wireImage.color == color)
				{
					return wire;
				}
			}
			return null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (isConnected)
			{
				DisconnectWire();
			}
		}

		public void UpdateWire(Vector3 newPosition)
		{
			if (currentDraggedWire != null)
			{
				RectTransform wireRectTransform = currentDraggedWire.GetComponent<RectTransform>();

				wireRectTransform.pivot = new Vector2(0, 0.5f);

				Vector3 direction = newPosition - startPoint;
				currentDraggedWire.transform.position = startPoint;

				currentDraggedWire.transform.right = direction.normalized;

				float distance = Vector3.Distance(startPoint, newPosition);

				float scaledDistance = distance * scaleFactor;


				wireRectTransform.sizeDelta = new Vector2(scaledDistance, wireRectTransform.sizeDelta.y);
			}
		}

		static public T FindInParents<T>(GameObject go) where T : Component
		{
			if (go == null) return null;
			var comp = go.GetComponent<T>();

			if (comp != null)
				return comp;

			Transform t = go.transform.parent;
			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
			return comp;
		}

		public void ResetWire()
		{
			isConnected = false;
			wireImage.sprite = emptyWireSprite;
			transform.position = startPoint;
			transform.SetSiblingIndex(originalSiblingIndex);
		}
	}
}