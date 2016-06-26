namespace Uspelite.Web.Infrastructure
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class JsonHelper
    {
        public object NestedDeserializeDynamicDictionary(string json)
        {
            return ToDictionaryRecursive(JToken.Parse(json));
        }

        private object ToDictionaryRecursive(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToDictionaryRecursive(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToDictionaryRecursive).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
    }
}
