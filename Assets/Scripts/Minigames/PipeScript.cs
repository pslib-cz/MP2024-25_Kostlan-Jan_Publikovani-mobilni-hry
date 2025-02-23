using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Enums;

/// <summary>
/// 
/// </summary>
public class PipeScript : MonoBehaviour, IPointerClickHandler
{
	[Header("Pipe type")]
	public PipeType pipeType;
	public bool isConnected;
	public bool[] openDirections = new bool[4];
	[SerializeField] public int gridX { get; set; }
	[SerializeField] public int gridY { get; set; }

	[Header("Components connections")]
	private RectTransform rectTransform;
	private GameManager gameManager;

	private void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	private void Start()
	{
		int rand = Random.Range(0, 4);
		rectTransform.localEulerAngles = new Vector3(0, 0, rand * 90);
		UpdateOpenDirections();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		rectTransform.Rotate(new Vector3(0, 0, 90));
		UpdateOpenDirections();

		gameManager.CheckAllConnections();
	}

	public void UpdateOpenDirections()
	{
		int rotation = (int)rectTransform.localEulerAngles.z;

		switch (pipeType)
		{
			case PipeType.Straight:
				openDirections = (rotation == 0 || rotation == 180) ?
					new bool[] { true, false, true, false } : new bool[] { false, true, false, true };
				break;

			case PipeType.Corner:
				rotation = (rotation + 360) % 360;
				switch (rotation)
				{
					case 0: openDirections = new bool[] { true, false, false, true }; break;
					case 90: openDirections = new bool[] { true, true, false, false }; break;
					case 180: openDirections = new bool[] { false, true, true, false }; break;
					case 270: openDirections = new bool[] { false, false, true, true }; break;
				}
				break;

			case PipeType.TShape:
				rotation = (rotation + 360) % 360;
				switch (rotation)
				{
					case 0: openDirections = new bool[] { true, false, true, true }; break;
					case 90: openDirections = new bool[] { true, true, false, true }; break;
					case 180: openDirections = new bool[] { true, true, true, false }; break;
					case 270: openDirections = new bool[] { false, true, true, true }; break;
				}
				break;

			case PipeType.Cross:
				openDirections = new bool[] { true, true, true, true };
				break;
				//case PipeType.Empty:
				//             openDirections = new bool[] { false, false, false, false };
				//             break;
		}
	}

	public bool HasOpenDirection(PipeDirection direction)
	{
		return openDirections[(int)direction];
	}

	public void SetConnected(bool connected)
	{

		isConnected = connected;
		GetComponent<Image>().color = connected ? Color.red : Color.white;
	}

	public void SetPipeOrientation(PipeDirection direction)
	{
		float rotationAngle = 0f;
		switch (direction)
		{
			case PipeDirection.Right: rotationAngle = 0f; break;
			case PipeDirection.Up: rotationAngle = 90f; break;
			case PipeDirection.Left: rotationAngle = 180f; break;
			case PipeDirection.Down: rotationAngle = 270f; break;
		}

		rectTransform.localEulerAngles = new Vector3(0, 0, rotationAngle);

		UpdateOpenDirections();
	}

	public void SetPipeType(PipeType newPipeType)
	{
		pipeType = newPipeType;
		UpdateOpenDirections();
	}

	public void SetColor(Color color)
	{
		GetComponent<Image>().color = color;
	}
}