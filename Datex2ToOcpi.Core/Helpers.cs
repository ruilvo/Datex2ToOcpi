using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;

namespace Datex2ToOcpi.Core.Helpers;

public static class Helpers
{
    /// <summary>
    /// The refill point uses the standard format: PT*KLC*E*ABF*00005*02
    /// </summary>
    /// <param name="refillPoint"></param>
    /// <returns></returns>
    public static string PartyIdFromRefillPoint(RefillPoint refillPoint)
    {
        return refillPoint.ExternalIdentifier.Substring(3, 3);
    }
}
