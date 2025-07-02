using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ricaun.AutoCAD.UI.Utils
{
    /// <summary>
    /// Represents a WPF pack URI for referencing resources embedded in assemblies.
    /// </summary>
    public class PackUri
    {
        /// <summary>
        /// The pack URI prefix used for application resources.
        /// </summary>
        public const string Pack = "pack://application:,,,/";
        /// <summary>
        /// The component segment used in pack URIs.
        /// </summary>
        public const string Component = "component/";
        /// <summary>
        /// The separator character between assembly name and component in a pack URI.
        /// </summary>
        public const char ComponentSeparator = ';';

        /// <summary>
        /// Attempts to parse a pack URI string into a <see cref="PackUri"/> instance.
        /// </summary>
        /// <param name="packUri">The pack URI string to parse.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="PackUri"/> if successful; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if parsing was successful; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string packUri, out PackUri result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(packUri))
                return false;

            if (!AssemblyResourceUtils.TryParsePackUri(packUri, out string assemblyName, out string resourceName))
                return false;

            result = new PackUri(assemblyName, resourceName);
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackUri"/> class from a pack URI string.
        /// </summary>
        /// <param name="packUri">The pack URI string to parse.</param>
        /// <exception cref="ArgumentException">Thrown if the pack URI is invalid.</exception>
        public PackUri(string packUri)
        {
            if (!AssemblyResourceUtils.TryParsePackUri(packUri, out string assemblyName, out string resourceName))
            {
                throw new ArgumentException($"Invalid pack URI: {packUri}", nameof(packUri));
            }
            AssemblyName = assemblyName;
            ResourceName = resourceName;
        }

        /// <summary>
        /// Gets the full pack URI string for this resource.
        /// </summary>
        public string PackPath => ToString();

        /// <summary>
        /// Gets the name of the assembly containing the resource.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// Gets the name of the resource within the assembly.
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackUri"/> class with the specified assembly and resource names.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyName"/> or <paramref name="resourceName"/> is null or whitespace.</exception>
        public PackUri(string assemblyName, string resourceName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));

            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            AssemblyName = assemblyName;
            ResourceName = resourceName;
        }

        /// <summary>
        /// Returns the pack URI string representation of this instance.
        /// </summary>
        /// <returns>The pack URI string.</returns>
        public override string ToString()
        {
            return Pack + AssemblyName.ToLowerInvariant() + ComponentSeparator + Component + ResourceName.ToLowerInvariant();
        }

        /// <summary>
        /// Determines whether the resource referenced by this pack URI exists in the specified assembly.
        /// </summary>
        /// <returns><c>true</c> if the resource exists; otherwise, <c>false</c>.</returns>
        public bool IsResourceExists()
        {
            var valid = AssemblyName.GetResourceName(ResourceName);
            return ToString().Equals(valid, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    /// <summary>
    /// Provides utility methods for working with assembly resources and pack URIs in WPF applications.
    /// </summary>
    public static class AssemblyResourceUtils
    {
        /// <summary>
        /// Regular expression used to parse pack URIs and extract assembly and resource names.
        /// </summary>
        private static readonly Regex PackUriRegex = new Regex(
            @"^(?:pack://application:,,,/)?/?(?<assembly>[^/;]+);component/+(?<resource>.+?)\s*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Tries to parse a pack URI and extract the assembly and resource names.
        /// </summary>
        /// <param name="packUri">The pack URI string to parse. Example: "pack://application:,,,/AssemblyName;component/ResourceName"</param>
        /// <param name="assemblyName">
        /// When this method returns, contains the name of the assembly if parsing succeeded; otherwise, <c>null</c>.
        /// </param>
        /// <param name="resourceName">
        /// When this method returns, contains the name of the resource if parsing succeeded; otherwise, <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the pack URI was successfully parsed and both assembly and resource names were extracted; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The method uses a regular expression to match the expected pack URI format. It is case-insensitive and tolerant of optional "pack://application:,,,/" prefix.
        /// </remarks>
        public static bool TryParsePackUri(string packUri, out string assemblyName, out string resourceName)
        {
            assemblyName = null;
            resourceName = null;

            if (string.IsNullOrWhiteSpace(packUri))
                return false;

            Match match = PackUriRegex.Match(packUri.ToLowerInvariant());
            if (!match.Success)
                return false;

            assemblyName = match.Groups["assembly"].Value;
            resourceName = match.Groups["resource"].Value;
            return true;
        }

        /// <summary>
        /// Internal cache of resource names for each assembly, keyed by assembly name.
        /// </summary>
        static Dictionary<string, HashSet<string>> AssemblyResources { get; } =
            new Dictionary<string, HashSet<string>>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets the full pack URI for a resource in the specified assembly if it exists.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly containing the resource.</param>
        /// <param name="resourceName">The name of the resource to look for.</param>
        /// <returns>
        /// The pack URI string for the resource if it exists in the assembly; otherwise, an empty string.
        /// </returns>
        /// <remarks>pack://application:,,,/AssemblyName;component/ResourceName</remarks>
        public static string GetResourceName(this string assemblyName, string resourceName)
        {
            var resourceNames = assemblyName.GetResourceNames();

            if (resourceNames.Length == 0)
                return string.Empty;

            resourceName = resourceName.Trim().TrimStart('/').Trim();

            if (AssemblyResources.TryGetValue(assemblyName, out var hashResources))
            {
                if (hashResources.Contains(resourceName))
                {
                    return new PackUri(assemblyName, resourceName).ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the resource names for the specified assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to search for resources.</param>
        /// <returns>
        /// An array of resource names found in the specified assembly; otherwise, an empty array if none are found.
        /// </returns>
        /// <remarks>
        /// This method first checks the internal cache for resource names. If not found, it searches loaded assemblies
        /// for a matching assembly name and retrieves its resource names. If the assembly or resources are not found,
        /// an empty array is returned.
        /// </remarks>
        public static string[] GetResourceNames(this string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
                return new string[0];

            if (AssemblyResources.TryGetValue(assemblyName, out var hashResources))
            {
                return hashResources.ToArray();
            }

            var assemblies = GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assemblyName.Equals(assembly.GetName().Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return assembly.GetResourceNames();
                }
            }

            return new string[0];
        }

        /// <summary>
        /// Gets all loaded assemblies in the current application domain or context.
        /// </summary>
        /// <returns>An enumerable of loaded <see cref="Assembly"/> instances.</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
#if NETFRAMEWORK
            return AppDomain.CurrentDomain.GetAssemblies();
#elif NET    
            return System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()).Assemblies;
#endif
        }

        /// <summary>
        /// Gets the resource names for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly"/> instance to retrieve resource names from.
        /// </param>
        /// <returns>
        /// An array of resource names found in the specified assembly; otherwise, an empty array if none are found or if the assembly is null.
        /// </returns>
        /// <remarks>
        /// This method checks the internal cache for resource names. If not found, it searches the assembly's manifest resources
        /// for entries ending with ".g.resources", which are typically generated by WPF for XAML resources. It then reads the resource keys
        /// and caches them for future lookups. If the assembly or resources are not found, an empty array is returned.
        /// </remarks>
        public static string[] GetResourceNames(this Assembly assembly)
        {
            if (assembly is null)
                return new string[0];

            var assemblyName = assembly.GetName().Name;
            if (AssemblyResources.TryGetValue(assembly.GetName().Name, out var hashResources))
            {
                return hashResources.ToArray();
            }

            AssemblyResources[assemblyName] = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                foreach (var resourceName in assembly.GetManifestResourceNames())
                {
                    if (!resourceName.EndsWith(".g.resources", StringComparison.InvariantCultureIgnoreCase)) continue;

                    using (var stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        using (var reader = new System.Resources.ResourceReader(stream))
                        {
                            var collection = reader.OfType<DictionaryEntry>()
                                .Select(entry => entry.Key.ToString())
                                .OrderBy(e => e);

                            AssemblyResources[assemblyName] = new HashSet<string>(collection, StringComparer.InvariantCultureIgnoreCase);
                            break;
                        }
                    }
                }
            }
            catch { }
            return assembly.GetResourceNames();
        }
    }
}
