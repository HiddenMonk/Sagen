using System;

namespace Sagen.Extensibility
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class SagenPluginClassAttribute : Attribute
	{
		public SagenPluginClassAttribute(Type pluginClassType)
		{
			if (pluginClassType == null)
				throw new NullReferenceException("Plugin type is null.");
			if (!pluginClassType.IsSubclassOf(typeof(SagenLanguage)))
				throw new ArgumentException($"Plugin type {pluginClassType} must derive from {nameof(SagenLanguage)}.");

			PluginClassType = pluginClassType;
		}

		public Type PluginClassType { get; }
	}
}