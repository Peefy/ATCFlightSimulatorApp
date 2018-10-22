using System.Collections.Generic;

namespace XPlane10DataAdapter
{
    public interface IXPlaneConnect
    {
        void PauseSim(bool pause);
        float[] GetDREF(string dref);
        void SendDREF(string dref, float value);
        float[] GetCTRL(int ac = 0);
        void SendCTRL(float[] values);
        float[] GetPOSI(int ac = 0);
        void SendPOSI(float[] values);
        List<float[]> ReadData();
        void SendData(List<float[]> data);
        void SelectData(int[] rows);
        void SendText(string msg);
        void SendView(ViewType view);
        void SendWYPT(WayPointOp op, float[] points);
        void SetCONN(int port);
    }
}
