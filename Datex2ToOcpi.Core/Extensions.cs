using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using OCM.Model.OCPI;

using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;
using Datex2ToOcpi.Core.Strategy;

namespace Datex2ToOcpi.Core;

public static class ExtensionMethods
{
    /// <summary>
    /// Combine the current EnergyInfrastructureSite with another one.
    ///
    /// Some API's have the static data and the dynamic data in separate endpoints.
    /// Even though OCM only cares about the static data, the dynamic data is needed,
    /// as it contains the current availability status, which includes whether the site
    /// is operational or not.
    /// </summary>
    /// <param name="eiSite"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void CombineWith(this EnergyInfrastructureSite eiSite)
    {
        throw new NotImplementedException("Not yet implemented.");
    }

    /// <summary>
    /// Converts an <see cref="EnergyInfrastructureSite"/> instance to an OCPI <see cref="Location"/>.
    /// </summary>
    /// <param name="eiSite">The <see cref="EnergyInfrastructureSite"/> to convert.
    /// Cannot be <see langword="null"/>.
    /// </param>
    /// <returns>A new <see cref="Location"/> instance representing the OCPI location
    /// mapped from the specified <see cref="EnergyInfrastructureSite"/>.
    /// </returns>
    public static Location ToOcpiLocation(this EnergyInfrastructureSite eiSite, IStrategy? strategy = null)
    {
        strategy ??= new DefaultStrategy();

        Location location = new()
        {
            Country_code = strategy.CountryCode(eiSite),
            Party_id = strategy.PartyId(eiSite),
            Id = strategy.Id(eiSite),
            Publish = true, // Not available in DATEX II
            Address = strategy.Address(eiSite),
            City = strategy.City(eiSite),
            Country = strategy.Country(eiSite),
            Postal_code = strategy.Country(eiSite),

            Coordinates = new GeoLocation()
            {
                Latitude = eiSite.LocationReference.Point
            }
        };

        return location;
    }
}
