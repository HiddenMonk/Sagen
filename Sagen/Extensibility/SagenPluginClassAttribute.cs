using System;

namespace Sagen.Extensibility
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class SagenPluginClassAttribute : Attribute
	{
		public SagenPluginClassAttribute(Type pluginClassType, string pluginName)
		{
			if (pluginClassType == null)
				throw new NullReferenceException("Plugin type is null.");
			if (String.IsNullOrWhiteSpace(pluginName))
				throw new ArgumentException($"Parameter '{nameof(pluginName)}' must contain non-whitespace characters.");
			if (!pluginClassType.IsSubclassOf(typeof(SagenLanguage)))
				throw new ArgumentException($"Plugin type {pluginClassType} must derive from {nameof(SagenLanguage)}.");

			PluginClassType = pluginClassType;
			PluginName = pluginName;
		}

		public Type PluginClassType { get; }
		public string PluginName { get; }
	}
}