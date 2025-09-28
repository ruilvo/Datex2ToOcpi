using System;
using System.Linq;

using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;

namespace Datex2ToOcpi.Core.Strategy;

public class SpainStrategy : IStrategy
{
    public string City(EnergyInfrastructureSite eiSite)
    {
        var address = eiSite.LocationReference
                     .LocationReferenceExtension
                     .FacilityLocation
                     .Address;
        var addressLine = address.AddressLine
                                 .FirstOrDefault(al => al.Order == "2");

        return addressLine?.Text.Values[0].Value
               ?? throw new InvalidOperationException("Missing required address line text value.");
    }
}
