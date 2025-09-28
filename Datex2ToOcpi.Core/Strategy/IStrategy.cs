using System;
using System.Linq;
using System.Globalization;

using OCM.Model.OCPI;

using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;
using Datex2ToOcpi.Core.Models.Datex2.LocationReferencing;

namespace Datex2ToOcpi.Core.Strategy;

public interface IStrategy
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

    // There seems to be no standard way of identifying a location in Datex II.
    // This is a best-effort approach.
    public string Id(EnergyInfrastructureSite eiSite)
    {
        return eiSite.Id;
    }

    public string Address(EnergyInfrastructureSite eiSite)
    {
        return eiSite.LocationReference
                     .LocationReferenceExtension
                     .FacilityLocation
                     .Address
                     .AddressLine[0]
                     .Text
                     .Values[0]
                     .Value;
    }

    public string City(EnergyInfrastructureSite eiSite)
    {
        return eiSite.LocationReference
                     .LocationReferenceExtension
                     .FacilityLocation
                     .Address
                     .City
                     .Values[0]
                     .Value;
    }

    public string Country(EnergyInfrastructureSite eiSite)
    {
        // Datex II uses two letter country codes, OCPI uses three letter codes.
        return ISO3166.Country.List.First(
                    c => c.TwoLetterCode ==
                        eiSite.LocationReference
                            .LocationReferenceExtension
                            .FacilityLocation
                            .Address
                            .CountryCode
                ).ThreeLetterCode;
    }

    public string PostalCode(EnergyInfrastructureSite eiSite)
    {
        return eiSite.LocationReference
                     .LocationReferenceExtension
                     .FacilityLocation
                     .Address
                     .Postcode;
    }

    public GeoLocation Coordinates(EnergyInfrastructureSite eiSite)
    {
        var pointLocation = eiSite.LocationReference as PointLocation;
        var coordinates = (pointLocation?.PointByCoordinates?.PointCoordinates)
                        ?? throw new InvalidOperationException("LocationReference is not a PointLocation with PointByCoordinates.");
        return new GeoLocation()
        {
            Latitude = coordinates.Latitude.ToString("F7", CultureInfo.InvariantCulture),
            Longitude = coordinates.Longitude.ToString("F7", CultureInfo.InvariantCulture),
        };
    }
}
