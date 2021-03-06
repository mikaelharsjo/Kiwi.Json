using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class JsonArrayBuilder : JsonValueBuilder, IArrayBuilder
    {
        #region IArrayBuilder Members

        public object CreateNewArray(ITypeBuilderRegistry registry, object instanceState)
        {
            return new JsonArray();
        }

        public ITypeBuilder GetElementBuilder(ITypeBuilderRegistry registry)
        {
            return this;
        }

        public void AddElement(object array, object element)
        {
            ((JsonArray) array).Add((IJsonValue) element);
        }

        public object GetArray(object array)
        {
            return array;
        }

        #endregion
    }
}