// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public class ValidationException : Exception
    {
        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class Validator
    {
        private static Dictionary<string, object> CheckGripSignature(string claimRaw, DateTime? checkTime = null)
        {
            Dictionary<string, object> claim = null;
            try
            {
                claim = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimRaw);
            }
            catch (JsonSerializationException ex)
            {
                throw new ValidationException("Claim has invalid JSON format.", ex);
            }

            if (!claim.ContainsKey("exp"))
                throw new ValidationException("Claim does not contain exp.");

            int exp;
            try
            {
                exp = Convert.ToInt32(claim["exp"]);
            }
            catch (Exception ex)
            {
                throw new ValidationException("Could not parse exp field.", ex);
            }

            if (checkTime == null)
                checkTime = DateTime.UtcNow;

            if (checkTime >= new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(exp))
                throw new ValidationException("Signature expired.");

            return claim;
        }

        public static Dictionary<string, object> CheckGripSignature(string gripSignatureHeader, byte[] key, DateTime? checkTime = null)
        {
            string claimRaw = null;
            try
            {
                claimRaw = JWT.JsonWebToken.Decode(gripSignatureHeader, key);
            }
            catch (Exception ex)
            {
                throw new ValidationException("Could not decode signature with key.", ex);
            }

            return CheckGripSignature(claimRaw, checkTime);
        }

        public static Dictionary<string, object> CheckGripSignature(string gripSignatureHeader, PublishControl.Configuration config, DateTime? checkTime = null)
        {
            string claimRaw = null;
            foreach (var entry in config.Entries)
            {
                if (entry.Key == null)
                    continue;

                try
                {
                    claimRaw = JWT.JsonWebToken.Decode(gripSignatureHeader, entry.Key);
                    break;
                }
                catch (Exception)
                {
                    // pass
                }
            }

            if (claimRaw == null)
                throw new ValidationException("Could not decode signature with any key.");

            return CheckGripSignature(claimRaw, checkTime);
        }
    }

    public static class ValidatorExtensions
    {
        public static bool CheckGripSignature(this HttpRequestMessage request, PublishControl.Configuration config, DateTime? checkTime = null)
        {
            foreach (var headerVal in request.Headers.GetValues("Grip-Sig"))
            {
                try
                {
                    Validator.CheckGripSignature(headerVal, config, checkTime);
                    return true;
                }
                catch (ValidationException)
                {
                    // skip to the next
                }
            }

            return false;
        }
    }
}
