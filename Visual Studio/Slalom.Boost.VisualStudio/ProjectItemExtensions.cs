using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;

namespace Slalom.Boost.VisualStudio
{
    // TOOD: Continue to move to using the file code model
    public static class ProjectItemExtensions
    {
        public static IEnumerable<CodeProperty> GetCodeProperties(this ProjectItem instance)
        {
            return GetCodeItems<CodeProperty>(instance.FileCodeModel?.CodeElements);
        }

        public static IEnumerable<CodeFunction> GetMethods(this ProjectItem instance)
        {
            return GetCodeItems<CodeFunction>(instance.FileCodeModel?.CodeElements);
        }

        public static string GetContent(this ProjectItem item)
        {
            return File.ReadAllText(item.GetPath());
        }

        public static string GetClassName(this ProjectItem instance)
        {
            if (instance.GetPath().EndsWith(".cs"))
            {
                return GetCodeItems<CodeClass>(instance?.FileCodeModel?.CodeElements).FirstOrDefault()?.Name;
            }
            return null;
        }

        public static string GetFullClassName(this ProjectItem instance)
        {
            if (instance.GetPath().EndsWith(".cs"))
            {
                return GetCodeItems<CodeClass>(instance?.FileCodeModel?.CodeElements).FirstOrDefault()?.FullName;
            }
            return null;
        }

        public static string GetEntityName(this ProjectItem instance)
        {
            if (instance.IsEntity())
            {
                return instance.GetClassName();
            }

            if (instance.IsReadModel())
            {
                var name = instance.GetClassName();
                if (name.EndsWith("ReadModel"))
                {
                    return name.Substring(0, name.Length - 9);
                }
                return name;
            }
            return null;
        }

        public static bool IsAddCommand(this ProjectItem instance)
        {
            return instance != null && instance.GetClassName().StartsWith("Add") && instance.IsCommand();
        }

        public static bool IsStringConcept(this ProjectItem instance)
        {
            return instance != null && instance.GetBaseClasses().Any(e => e.Name.StartsWith("ConceptAs", StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<CodeClass> GetBaseClasses(this ProjectItem instance)
        {
            return GetCodeItems<CodeClass>(instance.FileCodeModel?.CodeElements).FirstOrDefault()?.Bases.Cast<CodeClass>();
        }

        public static bool IsCommand(this ProjectItem instance)
        {
            return instance != null && instance.GetBaseClasses().Any(e => e.Name == "Command");
        }

        public static bool IsInputValidation(this ProjectItem instance)
        {
            return instance != null && instance.GetBaseClasses().Any(e => e.Name == "InputValidationRuleSet");
        }

        public static bool IsEntity(this ProjectItem instance)
        {
            return GetCodeItems<CodeClass>(instance?.FileCodeModel?.CodeElements).Any(e => e.Bases.Cast<CodeClass>().Any(x => x.Name == "Entity"));
        }

        public static bool IsReadModel(this ProjectItem instance)
        {
            return GetCodeItems<CodeClass>(instance?.FileCodeModel?.CodeElements).Any(e => e.ImplementedInterfaces.Cast<CodeInterface>().Any(x => x.Name == "IReadModelElement"));
        }

        public static string GetPath(this ProjectItem instance)
        {
            return instance.FileNames[0];
        }

        public static string GetRelativeFolder(this ProjectItem instance)
        {
            var path = instance.GetPath();
            var rootPath = instance.ContainingProject.GetRootPath();

            return Path.GetDirectoryName(path.Substring(rootPath.Length + 1));
        }

        public static string GetNamespace(this ProjectItem instance)
        {
            return GetCodeItems<CodeNamespace>(instance.FileCodeModel?.CodeElements).FirstOrDefault()?.Name;
        }

        public static IEnumerable<T> GetCodeItems<T>(this CodeElements elements) where T : class
        {
            if (elements != null)
            {
                foreach (CodeElement element in elements)
                {
                    var property = element as T;
                    if (property != null)
                    {
                        yield return property;
                    }

                    foreach (var item in GetCodeItems<T>(element.Children))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}