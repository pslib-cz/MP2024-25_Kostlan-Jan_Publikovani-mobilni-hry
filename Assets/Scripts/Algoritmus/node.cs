using UnityEngine;

namespace Assets.Scripts.Algoritmus
{
	/// <summary>
	/// Node s konkréní pozicí, rodiče, hodnocení atd.
	/// </summary>
	public class Node
	{
		public Vector2Int Position { get; set; }
		public Node Parent { get; set; }
		public float GCost { get; set; }
		public float HCost { get; set; }
		public float FCost => GCost + HCost;

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			var other = (Node)obj;
			return Position.Equals(other.Position);
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}
	}
}
