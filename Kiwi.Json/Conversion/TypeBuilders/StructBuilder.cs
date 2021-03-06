using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class StructBuilder<TStruct> : AbstractTypeBuilder, IObjectBuilder
    {
        private static readonly Dictionary<string, StructMember> Members = (from field in
                                                                                typeof (TStruct).GetFields(
                                                                                    BindingFlags.SetField |
                                                                                    BindingFlags.Public |
                                                                                    BindingFlags.Instance)
                                                                            select new StructMember
                                                                                       {
                                                                                           Name = field.Name,
                                                                                           Type = field.FieldType,
                                                                                           Member = field
                                                                                       })
            .ToDictionary(m => m.Name, m => m);

        #region IObjectBuilder Members

        public override object CreateNewObject(ITypeBuilderRegistry registry, object instanceState)
        {
            return new Dictionary<string, object>();
        }

        public override object GetMemberState(string memberName, object @object)
        {
            return null;
        }

        public override ITypeBuilder GetMemberBuilder(ITypeBuilderRegistry registry, string memberName)
        {
            StructMember member;
            if (Members.TryGetValue(memberName, out member))
            {
                return registry.GetTypeBuilder(member.Type);
            }
            return NothingBuilder.Instance;
        }

        public override void SetMember(string memberName, object @object, object value)
        {
            if (Members.ContainsKey(memberName))
            {
                ((Dictionary<string, object>) @object).Add(memberName, value);
            }
        }

        public override object GetObject(object @object)
        {
            var instanceMemberValues = (Dictionary<string, object>) @object;
            var membersToSet = (from kv in instanceMemberValues
                                select new
                                           {
                                               Members[kv.Key].Member,
                                               kv.Value
                                           }).ToArray();

            var members = (from v in membersToSet select v.Member).ToArray();
            var values = (from v in membersToSet select v.Value).ToArray();

            return
                FormatterServices.PopulateObjectMembers(FormatterServices.GetSafeUninitializedObject(typeof (TStruct)),
                                                        members, values);
        }

        #endregion

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        #region Nested type: StructMember

        private class StructMember
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public MemberInfo Member { get; set; }
        }

        #endregion
    }
}