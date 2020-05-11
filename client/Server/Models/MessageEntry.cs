﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace client.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class MessageEntry
    {
        /// <summary>
        /// Initializes a new instance of the MessageEntry class.
        /// </summary>
        public MessageEntry() { }

        /// <summary>
        /// Initializes a new instance of the MessageEntry class.
        /// </summary>
        public MessageEntry(int? id = default(int?), DateTime? time = default(DateTime?), string fromProperty = default(string), string to = default(string), string message = default(string), bool? downloaded = default(bool?))
        {
            Id = id;
            Time = time;
            FromProperty = fromProperty;
            To = to;
            Message = message;
            Downloaded = downloaded;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Time")]
        public DateTime? Time { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "From")]
        public string FromProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "To")]
        public string To { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Downloaded")]
        public bool? Downloaded { get; set; }

    }
}