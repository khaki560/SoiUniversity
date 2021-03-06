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

    public partial class UserEntry
    {
        /// <summary>
        /// Initializes a new instance of the UserEntry class.
        /// </summary>
        public UserEntry() { }

        /// <summary>
        /// Initializes a new instance of the UserEntry class.
        /// </summary>
        public UserEntry(string name = default(string), string pass = default(string), string pubKey = default(string), string privateKey = default(string))
        {
            Name = name;
            Pass = pass;
            PubKey = pubKey;
            PrivateKey = privateKey;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Pass")]
        public string Pass { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PubKey")]
        public string PubKey { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PrivateKey")]
        public string PrivateKey { get; set; }

    }
}
