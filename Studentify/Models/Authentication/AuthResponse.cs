using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Models.Authentication
{
    /// <summary>
    /// Class that represents response returned by AuthenticateController
    /// actions to inform the frontend about a result of an operation.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Describe result (Error or Success)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Description of the operation's result
        /// </summary>
        public string Message { get; set; }
    }
}
