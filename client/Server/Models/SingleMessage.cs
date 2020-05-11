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

    public partial class SingleMessage
    {
        /// <summary>
        /// Initializes a new instance of the SingleMessage class.
        /// </summary>
        public SingleMessage() { }

        /// <summary>
        /// Initializes a new instance of the SingleMessage class.
        /// </summary>
        public SingleMessage(DateTime? time = default(DateTime?), string fromProperty = default(string), string to = default(string), string message = default(string))
        {
            Time = time;
            FromProperty = fromProperty;
            To = to;
            Message = message;
        }

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

    }
}
