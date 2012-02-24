using Kiwi.Json.Untyped;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class JsonObjectBuilder : JsonValueBuilder, IObjectBuilder
    {
        private readonly JsonObject _object = new JsonObject();

        #region IObjectBuilder<IJsonValue> Members

        public ITypeBuilder GetMemberBuilder(string memberName)
        {
            return this;
        }

        public void SetMember(string memberName, object value)
        {
            //_object[memberName] = (IJsonValue)value;
            _object.Add(memberName, (IJsonValue) value);
        }

        public object GetObject()
        {
            return _object;
        }

        #endregion
    }
}