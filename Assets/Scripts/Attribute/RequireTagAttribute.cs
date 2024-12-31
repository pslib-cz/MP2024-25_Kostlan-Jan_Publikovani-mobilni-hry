using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class RequireTagAttribute : System.Attribute 
	{
		public string Tag { get; }

		public RequireTagAttribute(string tag)
		{
			Tag = tag;
		}
	}
}

#if UNITY_EDITOR
[InitializeOnLoad]
public static class RequireTagValidator
{
	static RequireTagValidator()
	{
		ObjectFactory.componentWasAdded += ValidateTag;
	}

	private static void ValidateTag(Component component)
	{
		var componentType = component.GetType();

		var attribute = (Assets.Scripts.Attributes.RequireTagAttribute)
			Attribute.GetCustomAttribute(componentType, typeof(Assets.Scripts.Attributes.RequireTagAttribute));

		if (attribute != null)
		{
			var gameObject = component.gameObject;

			if (gameObject.tag != attribute.Tag)
			{
				throw new InvalidOperationException(
					$"GameObject '{gameObject.name}' musí mít tag '{attribute.Tag}' pro použití komponenty {componentType.Name}.");
			}
		}
	}
}
#endif