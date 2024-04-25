//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.CloudSave.Internal.Http;



namespace Unity.Services.CloudSave.Internal.Models
{
    /// <summary>
    /// Request type for a Data Item to store in the Cloud Save service.
    /// </summary>
    [Preserve]
    [DataContract(Name = "SetItemBody")]
    internal class SetItemBody
    {
        /// <summary>
        /// Request type for a Data Item to store in the Cloud Save service.
        /// </summary>
        /// <param name="key">The key will be created if it does not exist, provided the item limit has not been reached for this entity.</param>
        /// <param name="value">Any JSON serializable structure</param>
        /// <param name="writeLock">Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.</param>
        [Preserve]
        public SetItemBody(string key, object value, string writeLock = default)
        {
            Key = key;
            Value = (IDeserializable) JsonObject.GetNewJsonObjectResponse(value);
            WriteLock = writeLock;
        }

        /// <summary>
        /// The key will be created if it does not exist, provided the item limit has not been reached for this entity.
        /// </summary>
        [Preserve]
        [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = true)]
        public string Key{ get; }
        
        /// <summary>
        /// Any JSON serializable structure
        /// </summary>
        [Preserve][JsonConverter(typeof(JsonObjectConverter))]
        [DataMember(Name = "value", IsRequired = true, EmitDefaultValue = true)]
        public IDeserializable Value{ get; }
        
        /// <summary>
        /// Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.
        /// </summary>
        [Preserve]
        [DataMember(Name = "writeLock", EmitDefaultValue = false)]
        public string WriteLock{ get; }
    
        /// <summary>
        /// Formats a SetItemBody into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            if (Key != null)
            {
                serializedModel += "key," + Key.ToString() + ",";
            }
            if (Value != null)
            {
                serializedModel += "value," + Value.ToString() + ",";
            }
            if (WriteLock != null)
            {
                serializedModel += "writeLock," + WriteLock;
            }
            return serializedModel;
        }

        /// <summary>
        /// Returns a SetItemBody as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            if (Key != null)
            {
                var keyStringValue = Key.ToString();
                dictionary.Add("key", keyStringValue);
            }
            
            if (WriteLock != null)
            {
                var writeLockStringValue = WriteLock.ToString();
                dictionary.Add("writeLock", writeLockStringValue);
            }
            
            return dictionary;
        }
    }
}
