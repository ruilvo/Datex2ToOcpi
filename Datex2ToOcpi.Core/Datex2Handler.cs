using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Datex2ToOcpi.Core;

public static class Datex2Handler
{
    public static EnergyInfrastructureTable EnergyInfraTableFromStream(Stream stream)
    {
        try
        {
            XmlSerializer serializer = new(typeof(Models.Datex2.Common.PayloadPublication));
            var payload = serializer.Deserialize(stream) as Models.Datex2.Common.PayloadPublication
                          ?? throw new InvalidOperationException("Deserialization returned null PayloadPublication.");

            // The root element is <ns7:payload> but the payload's type is
            // ns6:EnergyInfrastructureTablePublication. This is Datex II
            // polymorphic type XML: the xsi:type points to the real type, which
            // is in the energyInfrastructure namespace.
            if (payload is EnergyInfrastructureTablePublication eiPublication)
            {
                if (eiPublication.EnergyInfrastructureTable.Count == 0)
                {
                    throw new InvalidOperationException("No EnergyInfrastructureTable found in the publication.");
                }
                // All the files I've seen have only one table.
                // We can handle multiple tables if needed.
                if (eiPublication.EnergyInfrastructureTable.Count > 1)
                {
                    
                    throw new InvalidOperationException("Multiple EnergyInfrastructureTables found; expected only one.");
                }
                return eiPublication.EnergyInfrastructureTable[0];
            }
            else
            {
                throw new InvalidOperationException($"Payload type is {payload.GetType().Name}, not EnergyInfrastructureTablePublication.");
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error processing Datex II data from stream.", ex);
        }
    }
}
