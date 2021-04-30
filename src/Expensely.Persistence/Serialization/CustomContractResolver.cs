using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Json.Serialization;
using Raven.Client.Json.Serialization.NewtonsoftJson;

namespace Expensely.Persistence.Serialization
{
    /// <summary>
    /// Represents the custom contract resolver to be used in the serialization process.
    /// </summary>
    public sealed class CustomContractResolver : DefaultRavenContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomContractResolver"/> class.
        /// </summary>
        /// <param name="conventions">The serialization conventions.</param>
        public CustomContractResolver(ISerializationConventions conventions)
            : base(conventions)
        {
        }

        /// <inheritdoc />
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

            if (jsonProperty.Writable)
            {
                return jsonProperty;
            }

            var propertyInfo = member as PropertyInfo;

            if (propertyInfo == null)
            {
                return jsonProperty;
            }

            bool hasPrivateSetter = propertyInfo.GetSetMethod(true) != null;

            jsonProperty.Writable = hasPrivateSetter;

            if (jsonProperty.Writable)
            {
                return jsonProperty;
            }

            FieldInfo privateField = member
                .DeclaringType!
                .GetRuntimeFields()
                .FirstOrDefault(x =>
                    x.Name.Equals(
                        $"_{char.ToLowerInvariant(jsonProperty.PropertyName![0])}{jsonProperty.PropertyName[1..]}",
                        StringComparison.Ordinal));

            if (privateField == null)
            {
                return jsonProperty;
            }

            string originalPropertyName = jsonProperty.PropertyName;

            jsonProperty = base.CreateProperty(privateField, memberSerialization);

            jsonProperty.Readable = true;

            jsonProperty.Writable = true;

            jsonProperty.PropertyName = originalPropertyName;

            jsonProperty.UnderlyingName = originalPropertyName;

            return jsonProperty;
        }

        /// <inheritdoc />
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = new List<MemberInfo>();

            members.AddRange(
                objectType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute(typeof(JsonIgnoreAttribute)) == null));

            return members;
        }
    }
}
