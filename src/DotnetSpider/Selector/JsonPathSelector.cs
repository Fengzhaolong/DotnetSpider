using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotnetSpider.Selector
{
    /// <summary>
    /// JsonPath selector.
    /// Used to extract content from JSON.
    /// </summary>
    public class JsonPathSelector : ISelector
    {
        private readonly string _jsonPath;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="jsonPath">JsonPath</param>
        public JsonPathSelector(string jsonPath)
        {
            _jsonPath = jsonPath;
        }

        /// <summary>
        /// 从JSON文本中查询单个结果
        /// 如果符合条件的结果有多个, 仅返回第一个
        /// </summary>
        /// <param name="text">需要查询的Json文本</param>
        /// <returns>查询结果</returns>
        public ISelectable Select(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var token = ((JObject) JsonConvert.DeserializeObject(text)).SelectToken(_jsonPath);
            if (token == null)
            {
                return null;
            }

            return new JsonSelectable(token);
        }

        /// <summary>
        /// 从JSON文本中查询所有结果
        /// </summary>
        /// <param name="text">需要查询的Json文本</param>
        /// <returns>查询结果</returns>
        public IEnumerable<ISelectable> SelectList(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return ((JObject) JsonConvert.DeserializeObject(text)).SelectTokens(_jsonPath)
                .Select(x => new JsonSelectable(x));
        }
    }
}