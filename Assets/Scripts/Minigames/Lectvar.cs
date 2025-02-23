using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Minigames
{
	public class Lectvar : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
	{
		public bool dragOnSurfaces = true;
		public LectvarController lectvarManager;
		public Sprite emptyPotionSprite;
		public Sprite filledPotionSprite;

		private RectTransform m_DraggingPlane;
		private Vector3 startPosition;
		private Image potionImage;
		private Sprite startPotionImage;
		private int originalSiblingIndex;
		[SerializeField] private  int potionIndex;

		private void Start()
		{
			startPosition = transform.position;
			potionImage = GetComponent<Image>();
			startPotionImage = potionImage.sprite;
			originalSiblingIndex = transform.GetSiblingIndex();
		}

		public void SetPotionAppearance(Sprite sprite, int index)
		{
			if (potionImage == null)
			{
				potionImage = GetComponent<Image>();
			}
			potionImage.sprite = sprite;
			potionIndex = index;
		}


		public void OnBeginDrag(PointerEventData eventData)
		{
			if (potionImage.sprite == emptyPotionSprite)
			{
				eventData.pointerDrag = null;
				return;
			}

			originalSiblingIndex = transform.GetSiblingIndex();
			transform.SetSiblingIndex(2);
			GetComponent<Image>().raycastTarget = false;
			var canvas = FindInParents<Canvas>(gameObject);
			if (canvas == null)
				return;

			if (dragOnSurfaces)
				m_DraggingPlane = transform as RectTransform;
			else
				m_DraggingPlane = canvas.transform as RectTransform;

			transform.Rotate(new Vector3(0, 0, 30));
			SetDraggedPosition(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			GetComponent<Image>().raycastTarget = true;
			transform.rotation = Quaternion.identity;
			transform.position = startPosition;
			transform.SetSiblingIndex(originalSiblingIndex);
		}

		public void OnDrag(PointerEventData data)
		{
			SetDraggedPosition(data);
		}

		private void SetDraggedPosition(PointerEventData data)
		{
			if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
				m_DraggingPlane = data.pointerEnter.transform as RectTransform;

			Vector3 globalMousePos;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
			{
				transform.position = globalMousePos;
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			var draggedObject = eventData.pointerDrag?.GetComponent<Lectvar>();

			if (draggedObject != null)
			{
				if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Potion") && eventData.pointerEnter != draggedObject.gameObject)
				{
					var targetPotion = eventData.pointerEnter.GetComponent<Lectvar>();

					// Pokud cílový lektvar je prázdný, vraťme objekt zpět
					if (targetPotion.potionImage.sprite == emptyPotionSprite)
					{
						ResetPosition();
						return;
					}

					// Zkontrolujeme správný pořadí
					if (lectvarManager.CheckCorrectPotion(draggedObject.GetPotionIndex(), targetPotion.GetPotionIndex()))
					{

						targetPotion.potionImage.sprite = filledPotionSprite;

						draggedObject.potionImage.sprite = emptyPotionSprite;

						if (lectvarManager.IsSequenceComplete())
						{
							lectvarManager.EndGame();
						}
					}
					else
					{
						lectvarManager.ResetGame();
					}
				}
				else
				{
					ResetPosition();
				}
			}
		}

		private void ResetPosition()
		{
			transform.position = startPosition;
			transform.SetSiblingIndex(originalSiblingIndex);
			transform.rotation = Quaternion.identity;
		}

		private int GetPotionIndex()
		{
			return potionIndex;
		}

		public void ResetPotions()
		{
			potionImage.sprite = startPotionImage;
			transform.position = startPosition;
			transform.SetSiblingIndex(originalSiblingIndex);
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
	}
}