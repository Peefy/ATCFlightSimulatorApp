using System;
using System.IO;
using System.Collections.Generic;

namespace ATCSimulator.Models
{
    public static class ZBAAStandPositionFactory
    {
        private static Dictionary<string, ZBAAStandPosition> _positions
            = new Dictionary<string, ZBAAStandPosition>
            {
                { "806", ZBAAStandPosition.New("806", 40.08550853f, 116.5866655f, 353.0f, 36.0f) },
                { "807", ZBAAStandPosition.New("807", 40.08554117f, 116.5856192f, 353.0f, 36.0f) },
                { "808", ZBAAStandPosition.New("808", 40.08546398f, 116.5848012f, 353.0f, 36.0f) },
                { "809", ZBAAStandPosition.New("809", 40.08539544f, 116.5840232f, 353.0f, 36.0f) },
                { "810", ZBAAStandPosition.New("810", 40.08533946f, 116.5832487f, 353.0f, 36.0f) },
                { "811", ZBAAStandPosition.New("811", 40.08526971f, 116.5824604f, 353.0f, 36.0f) },
                { "812", ZBAAStandPosition.New("812", 40.08501598f, 116.5807972f, 353.0f, 36.0f) },
                { "813", ZBAAStandPosition.New("813", 40.08494163f, 116.5800191f, 353.0f, 36.0f) },
                { "814", ZBAAStandPosition.New("814", 40.08487330f,  116.5792373f, 353.0f, 36.0f) },
                { "815", ZBAAStandPosition.New("815", 40.08486166f, 116.5786044f, 353.0f, 36.0f) },
                { "816", ZBAAStandPosition.New("816", 40.08260633f, 116.577893f,  173.0f, 36.0f) },
                { "817", ZBAAStandPosition.New("817", 40.08251801f, 116.5772997f, 173.0f, 36.0f) },
                { "951", ZBAAStandPosition.New("951", 40.08893333f, 116.5990404f, 263.0f, 36.0f) },
                { "952", ZBAAStandPosition.New("952", 40.08962441f, 116.5989514f, 263.0f, 36.0f) },
                { "953", ZBAAStandPosition.New("953", 40.09027732f, 116.5988878f, 263.0f, 36.0f) },
                { "954", ZBAAStandPosition.New("954", 40.09102775f, 116.5988115f, 263.0f, 36.0f) },
                { "955", ZBAAStandPosition.New("955", 40.08989628f, 116.6083405f, 83.0f,  36.0f) },
                { "956", ZBAAStandPosition.New("956", 40.09052188f, 116.6082667f, 83.0f,  36.0f) },
                { "957", ZBAAStandPosition.New("957", 40.09117750f,  116.6082194f, 83.0f,  36.0f) },
                { "958", ZBAAStandPosition.New("958", 40.09188046f, 116.6081212f, 83.0f,  36.0f) },
                { "A113", ZBAAStandPosition.New("A113", 40.0817451f,  116.5799821f, 131.0f, 36.0f) },
                { "M01", ZBAAStandPosition.New("M01", 40.09573282f, 116.6079884f, 83.0f,  36.0f) },
                { "M02", ZBAAStandPosition.New("M02", 40.09644277f, 116.6078723f, 83.0f,  36.0f) }
            };

        public static void UpdateFromCsvFile(string filename)
        {
            var lines = File.ReadAllLines(filename);
            _positions.Clear();
            for (int i = 1;i < lines.Length ;++i)
            {
                var position = ZBAAStandPosition.New(lines[i]);
                _positions.Add(position.Name, position);
            }
        }

        public static ZBAAStandPosition Get(string name)
        {
            if (_positions.ContainsKey(name) == false)
            {
                throw new KeyNotFoundException(name + "not exist!");
            }
            return _positions[name];
        }
    }
        
}
