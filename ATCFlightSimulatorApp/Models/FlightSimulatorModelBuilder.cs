using System;

using Newtonsoft.Json;

using FlightSimulator.Models;

using ATCFlightSimulatorApp.Utils;

namespace ATCFlightSimulatorApp.Models
{
    public class FlightSimulatorModelBuilder
    {

        SimPlanePacket packet;

        public FlightSimulatorModelBuilder()
        {
            packet = new SimPlanePacket
            {
                ID = 10,
                PlanID = 10,
                Altitude = 0,
                AltitudeType = 1,
                CallSign = "EC135",
                DesAltitude = 0,
                DesAltitudeType = 1,
                DesHeading = 0,
                EmergencyType = 1,
                EngineFire = 0x11,
                EngineSmoke = 0x22,
                GAS = 1,
                GearStatus = 0x33,
                Heading = 0,
                IAS = 0,
                ICAOAddress = 1234,
                IsRVSM = true,
                LandingRunway = "北京机场",
                Latitude = 40.05692751,
                Longtitude = 116.59992303,
            };
        }

        public FlightSimulatorModelBuilder SetPositions(double x, double y, double z, double roll, double yaw, double pitch)
        {
            packet.Altitude = z;
            packet.Heading = yaw;
            packet.DesHeading = yaw;
            packet.RollAngle = roll;
            return this;
        }

        public FlightSimulatorModelBuilder SetLonLan(double x, double y, double z, double roll, double yaw, double pitch)
        {
            return this;
        }

        public byte[] Build()
        {
            return StructHelper.StructToBytes(packet);
        }

        public string BuildJsonString()
        {
            return JsonConvert.SerializeObject(packet, Formatting.Indented);
        }

    }

}
