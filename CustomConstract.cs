using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTools
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly string dynamicPropertyName;

        public CustomContractResolver(string dynamicPropertyName)
        {
            this.dynamicPropertyName = dynamicPropertyName;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName == "DynamicProperty" ? dynamicPropertyName : base.ResolvePropertyName(propertyName);
        }
    }
}
