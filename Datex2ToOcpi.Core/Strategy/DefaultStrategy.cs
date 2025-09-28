using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;

namespace Datex2ToOcpi.Core.Strategy;

public class DefaultStrategy
{
    public string CountryCode(EnergyInfrastructureSite eiSite)
    {
        return eiSite.LocationReference
                     .LocationReferenceExtension
                     .FacilityLocation
                     .Address
                     .CountryCode;
    }

    public string PartyId(EnergyInfrastructureSite eiSite)
    {
        // party_id is 3 chars in OCPI, so we can't use .Operator.Id directly.
        // I'll get it from an infrastructure station...
        // The compiler doesn't warn about possible nulls, so I guess the size
        // being more than zero is enforced.
        return Helpers.PartyIdFromRefillPoint(
                    eiSite.EnergyInfrastructureStation[0].RefillPoint[0]
                );
    }

    public string Id(EnergyInfrastructureSite eiSite)
    {
        return eiSite.Id;
    }


}