using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Core.Validators
{
    public static class CustomValidators
    {
        public static bool IsValidGuid(Guid unValidatedGuid)
        {
            if (unValidatedGuid == Guid.Empty || unValidatedGuid == null)
            {
                return false;
            }
            return Guid.TryParse(unValidatedGuid.ToString(), out Guid validatedGuid);

        }
    }
}
