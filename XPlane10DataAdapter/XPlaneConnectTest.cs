namespace XPlane10DataAdapter
{
    public class XPlaneConnectTest
    {
        public static void Run()
        {
            using(var xpc = new XPlaneConnect())
            {
                var positionData = xpc.GetPOSI(0);
                var controlData = xpc.GetCTRL(0);
            }
        }
    }
}
