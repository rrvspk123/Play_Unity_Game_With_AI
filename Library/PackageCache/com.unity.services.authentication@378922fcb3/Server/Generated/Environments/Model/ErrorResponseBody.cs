
//-----------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;
using Unity.Services.Authentication.Server.Shared;

namespace Unity.Services.Authentication.Server.Environments.Generated
{
    /// <summary>
    /// ErrorResponseBody
    /// </summary>
    [DataContract(Name = "ErrorResponseBody")]
    [Preserve]
    internal partial class ErrorResponseBody
    {
        /// <summary>
        /// The HTTP status code ([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
        /// </summary>
        /// <value>The HTTP status code ([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.</value>
        [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
        [Preserve]
        public int Status { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        /// <value>A human-readable explanation specific to this occurrence of the problem.</value>
        [DataMember(Name = "detail", IsRequired = true, EmitDefaultValue = true)]
        [Preserve]
        public string Detail { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type. It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization (e.g., using proactive content negotiation; see [RFC7231], Section 3.4).
        /// </summary>
        /// <value>A short, human-readable summary of the problem type. It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization (e.g., using proactive content negotiation; see [RFC7231], Section 3.4).</value>
        [DataMember(Name = "title", IsRequired = true, EmitDefaultValue = true)]
        [Preserve]
        public string Title { get; set; }

        /// <summary>
        /// An array of machine-readable service-specific errors.
        /// </summary>
        /// <value>An array of machine-readable service-specific errors.</value>
        [DataMember(Name = "details", EmitDefaultValue = false)]
        [Preserve]
        public List<KeyValuePair> Details { get; set; }

        /// <summary>
        /// A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be \&quot;about:blank\&quot;.
        /// </summary>
        /// <value>A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be \&quot;about:blank\&quot;.</value>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        [Preserve]
        public string Type { get; set; }

        /// <summary>
        /// A service-specific error code.
        /// </summary>
        /// <value>A service-specific error code.</value>
        [DataMember(Name = "code", IsRequired = true, EmitDefaultValue = true)]
        [Preserve]
        public int Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseBody" /> class.
        /// </summary>
        /// <param name="status">The HTTP status code ([RFC7231], Section 6) generated by the origin server for this occurrence of the problem. (required).</param>
        /// <param name="detail">A human-readable explanation specific to this occurrence of the problem. (required).</param>
        /// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization (e.g., using proactive content negotiation; see [RFC7231], Section 3.4). (required).</param>
        /// <param name="details">An array of machine-readable service-specific errors..</param>
        /// <param name="type">A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be \&quot;about:blank\&quot;. (required).</param>
        /// <param name="code">A service-specific error code. (required).</param>
        [Preserve]
        public ErrorResponseBody(int status = default(int), string detail = default(string), string title = default(string), List<KeyValuePair> details = default(List<KeyValuePair>), string type = default(string), int code = default(int))
        {
            this.Status = status;
            // to ensure "detail" is required (not null)
            if (detail == null)
            {
                throw new ArgumentNullException("detail is a required property for ErrorResponseBody and cannot be null");
            }
            this.Detail = detail;
            // to ensure "title" is required (not null)
            if (title == null)
            {
                throw new ArgumentNullException("title is a required property for ErrorResponseBody and cannot be null");
            }
            this.Title = title;
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for ErrorResponseBody and cannot be null");
            }
            this.Type = type;
            this.Code = code;
            this.Details = details;
        }
    }

}
