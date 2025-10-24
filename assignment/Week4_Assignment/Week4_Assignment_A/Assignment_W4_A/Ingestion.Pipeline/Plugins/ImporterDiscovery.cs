using Assignment_W4_A.Ingestion.Pipeline.Importers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_W4_A.Ingestion.Pipeline.Plugins
{
    /// <summary>
    /// Discover concrete implementations of <see cref="FileImporter{T}"/> via reflection.
    /// Objects are created via parameterless constructor when available.
    /// </summary>
    public static class ImporterDiscovery
    {
        /// <summary>Discover importer instances from loaded assemblies.</summary>
        public static IEnumerable<object> DiscoverImporters()
        {
            Type baseType = typeof(FileImporter<>);
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsAbstract || t.IsInterface)
                    {
                        continue;
                    }

                    Type? bt = t.BaseType;
                    if (bt == null)
                    {
                        continue;
                    }

                    if (bt.IsGenericType && bt.GetGenericTypeDefinition() == baseType)
                    {
                        object? instance = null;
                        try
                        {
                            instance = Activator.CreateInstance(t);
                        }
                        catch
                        {
                            instance = null;
                        }

                        if (instance != null)
                        {
                            yield return instance;
                        }
                    }
                }
            }
        }
    }

}
