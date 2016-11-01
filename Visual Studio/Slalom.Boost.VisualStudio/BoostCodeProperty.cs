using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Slalom.Boost.VisualStudio
{
    public class BoostCodeTypeRef : CodeTypeRef
    {
        public BoostCodeTypeRef(Type type)
        {
            this.AsString = type.ToFriendlyName();
            this.AsFullName = type.FullName;
        }

        public BoostCodeTypeRef(string name)
        {
            this.AsString = name;
            this.AsFullName = name;
        }

        public CodeTypeRef CreateArrayType(int Rank = 1)
        {
            throw new NotImplementedException();
        }

        public DTE DTE { get; }

        public object Parent { get; }

        public vsCMTypeRef TypeKind { get; }

        public CodeType CodeType { get; set; }

        public CodeTypeRef ElementType { get; set; }

        public string AsString { get; }

        public string AsFullName { get; }

        public int Rank { get; set; }
    }

    public class BoostCodeProperty : CodeProperty
    {
        public BoostCodeProperty(string name)
        {
            this.Name = name;
            this.Type = new BoostCodeTypeRef(typeof(String));
        }

        public BoostCodeProperty(string name, Type type)
        {
            this.Name = name;
            this.Type = new BoostCodeTypeRef(type);
        }

        public BoostCodeProperty(string name, string type)
        {
            this.Name = name;
            this.Type = new BoostCodeTypeRef(type);
        }

        public TextPoint GetStartPoint(vsCMPart Part = vsCMPart.vsCMPartWholeWithAttributes)
        {
            throw new NotImplementedException();
        }

        public TextPoint GetEndPoint(vsCMPart Part = vsCMPart.vsCMPartWholeWithAttributes)
        {
            throw new NotImplementedException();
        }

        public CodeAttribute AddAttribute(string Name, string Value, object Position)
        {
            throw new NotImplementedException();
        }

        public DTE DTE { get; }

        public CodeElements Collection { get; }

        public string Name { get; set; }

        public string FullName { get; }

        public ProjectItem ProjectItem { get; }

        public vsCMElement Kind { get; }

        public bool IsCodeType { get; }

        public vsCMInfoLocation InfoLocation { get; }

        public CodeElements Children { get; }

        public string Language { get; }

        public TextPoint StartPoint { get; }

        public TextPoint EndPoint { get; }

        public object ExtenderNames { get; }

        public object get_Extender(string ExtenderName)
        {
            throw new NotImplementedException();
        }

        public string ExtenderCATID { get; }

        public CodeClass Parent { get; }

        public string get_Prototype(int Flags = 0)
        {
            throw new NotImplementedException();
        }

        public CodeTypeRef Type { get; set; }

        public CodeFunction Getter { get; set; }

        public CodeFunction Setter { get; set; }

        public vsCMAccess Access { get; set; }

        public CodeElements Attributes { get; }

        public string DocComment { get; set; }

        public string Comment { get; set; }
    }
}
