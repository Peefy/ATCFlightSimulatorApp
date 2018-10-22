using System;

using FlightSimulator.Models;

using ATCFlightSimulatorApp.Utils;

namespace ATCFlightSimulatorApp.Models
{
    public class FlightSimulatorBuilder
    {

        SimPlanePacket packet;

        public FlightSimulatorBuilder()
        {
            packet = new SimPlanePacket();
        }

        public FlightSimulatorBuilder SetPositions(double x, double y, double z, double roll, double yaw, double pitch)
        {
            return this;
        }

        public FlightSimulatorBuilder SetPositions(double x, double y, double z, double roll, double yaw, double pitch)
        {
            return this;
        }

        public byte[] Build()
        {
            return StructHelper.StructToBytes(packet);
        }

    }

}
