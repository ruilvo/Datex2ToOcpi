using System;
using System.IO;
using System.Xml.Serialization;
using Datex2ToOcpi.Core.Models.Datex2.Common;
using Datex2ToOcpi.Core.Models.Datex2.EnergyInfrastructure;

namespace Datex2ToOcpi.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path.");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            try
            {
                XmlSerializer serializer = new(typeof(PayloadPublication));

                using (FileStream fs = new(filePath, FileMode.Open))
                {
                    PayloadPublication payload = (PayloadPublication)serializer.Deserialize(fs);

                    Console.WriteLine("XML file loaded successfully.");

                    // The root element is <ns7:payload> but the payload’s type
                    // is ns6:EnergyInfrastructureTablePublication. This is
                    // Datex II polymorphic type XML: the xsi:type
                    // points to the real type, which is in the
                    // energyInfrastructure namespace.
                    // This is true for the Mobi.e file, but is it for all of them?
                    if (payload is EnergyInfrastructureTablePublication eiPublication)
                    {
                        foreach (var site in eiPublication.EnergyInfrastructureTable)
                        {
                            foreach (var eiSite in site.EnergyInfrastructureSite)
                            {
                                Console.WriteLine($"Site ID: {eiSite.Id}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Payload type is {payload.GetType().Name}, not EnergyInfrastructureTablePublication.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML: {ex.Message}");
            }
        }
    }
}
