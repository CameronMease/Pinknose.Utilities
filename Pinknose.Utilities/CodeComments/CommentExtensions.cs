using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Pinknose.Utilities.CodeComments
{
    /// <summary>
    /// Class that enables programmatic retrieval of XML comments for properties, methods, etc.
    /// </summary>
    public static class CommentExtensions
    {
        private static readonly Dictionary<string, string> comments = new();
        private static readonly Dictionary<string, XmlDocument> assemblyXml = new();

        /// <summary>
        /// Gets the XML code comment from the object's property with the name supplied in propertyName.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetPropertyCodeComment(this object obj, string propertyName) => GetMemberComment(obj.GetType().GetProperty(propertyName));

        private static string BuildKey(MemberInfo memberInfo)
        {
            char identifier;

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                identifier = 'P';
            }
            else if (memberInfo.MemberType == MemberTypes.Method)
            {
                identifier = 'M';
            }
            else
            {
                throw new NotImplementedException();
            }

            return $"{identifier}:{memberInfo.DeclaringType.Namespace}.{memberInfo.DeclaringType.Name}.{memberInfo.Name}";
        }

        /// <summary>
        /// Gets the XML code comment from the object's method with the name supplied in propertyName.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetMethodCodeComment(this object obj, string methodName) => GetMemberComment(obj.GetType().GetMethod(methodName));

        private static string GetMemberComment(MemberInfo memberInfo)
        {
            var key = BuildKey(memberInfo);
            var assemblyQualifiedName = memberInfo.DeclaringType.AssemblyQualifiedName;
            var codeBase = memberInfo.DeclaringType.Assembly.CodeBase;

            try
            {
                string comment;

                lock (comments)
                {
                    if (!comments.TryGetValue(key, out comment))
                    {
                        XmlDocument xml;

                        if (!assemblyXml.TryGetValue(assemblyQualifiedName, out xml))
                        {
                            var xmlFilePath = Path.ChangeExtension(codeBase, "xml");
                            xml = new XmlDocument();
                            xml.Load(xmlFilePath);
                            assemblyXml.Add(assemblyQualifiedName, xml);
                        }

                        var node = xml.DocumentElement.SelectSingleNode($"//members/member[@name='{key}']/summary");

                        if (node == null)
                        {
                            comment = string.Empty;
                        }
                        else
                        {
                            comment = FormatComment(node.InnerText);
                        }

                        comments.Add(key, comment);
                    }
                }

                return comment;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static readonly Regex DuplicateWhitespaceRegex = new Regex(@"\s{2,}", RegexOptions.Compiled);

        private static string FormatComment(string comment) => DuplicateWhitespaceRegex.Replace(comment, " ").Trim();

    }
}
