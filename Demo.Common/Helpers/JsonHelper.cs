namespace Common.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonHelper
    {
        public static string ObjToJson<TModel>(this TModel model)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, jsonSettings);
        }

        public static TModel JsonToObj<TModel>(this string json)
        {
            return JsonConvert.DeserializeObject<TModel>(json);
        }
    }
}
