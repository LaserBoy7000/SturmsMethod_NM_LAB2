using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CHM.Calc
{
    public class SturmSeries
    {
        public Polynomial[] Series { get => series.AsReadOnly().ToArray(); }

        public int Power { get; }

        readonly List<Polynomial> series;
        public SturmSeries(Polynomial seed)
        {
            Power = seed.Power;
            series = new() { seed };
            if (seed.Power == 0)
                return;
            series.Add(seed.Differentiate());

           for(int i = 2; series.Last().Power > 0; i++)
               series.Add(-(series[i - 2] / series[i - 1]).Item2);
        }
    }
}
