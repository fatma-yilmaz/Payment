using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.Core.Validators
{
    public static class CustomValidators
    {
        public static Func<string, bool> IsGuid => value =>
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return Guid.TryParse(value, out _);
        };

        public static bool BeAValidGuid(Guid unValidatedGuid)
        {
            if (unValidatedGuid == Guid.Empty || unValidatedGuid == null)
            {
                return false;
            }
            return Guid.TryParse(unValidatedGuid.ToString(), out Guid validatedGuid);

        }
    }
}
