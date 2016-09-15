using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OgameBot.Objects.Types;

namespace OgameBot.Engine
{
    public class OGameStringProvider
    {
        [JsonProperty("Strings")]
        private readonly Dictionary<string, Dictionary<string, string>> _strings;

        [JsonIgnore]
        private readonly Dictionary<Type, object> _reverseMapCache;

        public OGameStringProvider()
        {
            _strings = new Dictionary<string, Dictionary<string, string>>();
            _reverseMapCache = new Dictionary<Type, object>();
        }

        public static OGameStringProvider Load(string file)
        {
            string content = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<OGameStringProvider>(content);
        }

        public void Save(string file)
        {
            string content = JsonConvert.SerializeObject(this);

            File.WriteAllText(file, content);
        }

        private string Get(string type, string key)
        {
            Dictionary<string, string> dict;
            if (!_strings.TryGetValue(type, out dict))
                return null;

            string value;
            dict.TryGetValue(key, out value);

            return value;
        }

        private void Set(string type, string key, string value)
        {
            Dictionary<string, string> dict;
            if (!_strings.TryGetValue(type, out dict))
                _strings.Add(type, dict = new Dictionary<string, string>());

            dict[key] = value;
        }

        public Dictionary<string, TEnum> GetReverseMap<TEnum>()
        {
            object resultObj;
            if (!_reverseMapCache.TryGetValue(typeof(TEnum), out resultObj))
            {
                Dictionary<string, string> dict;

                string type = typeof(TEnum).Name;
                if (!_strings.TryGetValue(type, out dict))
                    return null;

                Dictionary<string, TEnum> result = new Dictionary<string, TEnum>();

                foreach (KeyValuePair<string, string> pair in dict)
                {
                    TEnum val = (TEnum)Enum.Parse(typeof(TEnum), pair.Key);
                    result.Add(pair.Value, val);
                }

                _reverseMapCache[typeof(TEnum)] = resultObj = result;
            }

            return (Dictionary<string, TEnum>)resultObj;
        }

        public void SetLocalizedName(ResourceType type, string value)
        {
            Set(nameof(ResourceType), type.ToString(), value);
        }

        public void SetLocalizedName(ShipType type, string value)
        {
            Set(nameof(ShipType), type.ToString(), value);
        }

        public void SetLocalizedName(DefenceType type, string value)
        {
            Set(nameof(DefenceType), type.ToString(), value);
        }

        public void SetLocalizedName(BuildingType type, string value)
        {
            Set(nameof(BuildingType), type.ToString(), value);
        }

        public void SetLocalizedName(MissionType type, string value)
        {
            Set(nameof(MissionType), type.ToString(), value);
        }

        public void SetLocalizedName(ResearchType type, string value)
        {
            Set(nameof(ResearchType), type.ToString(), value);
        }

        public string GetLocalizedName(ResourceType type)
        {
            return Get(nameof(ResourceType), type.ToString());
        }

        public string GetLocalizedName(ShipType type)
        {
            return Get(nameof(ShipType), type.ToString());
        }

        public string GetLocalizedName(DefenceType type)
        {
            return Get(nameof(DefenceType), type.ToString());
        }

        public string GetLocalizedName(BuildingType type)
        {
            return Get(nameof(BuildingType), type.ToString());
        }

        public string GetLocalizedName(MissionType type)
        {
            return Get(nameof(MissionType), type.ToString());
        }

        public string GetLocalizedName(ResearchType type)
        {
            return Get(nameof(ResearchType), type.ToString());
        }
    }
}