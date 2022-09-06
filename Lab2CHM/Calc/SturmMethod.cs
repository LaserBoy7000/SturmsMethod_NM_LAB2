using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CHM.Calc
{
    public class SturmMethod
    {
        public SturmSeries Series { get; }

        public Dictionary<double, double> LastResult { get; private set; }
        
        public SturmMethod(SturmSeries series, double gap)
        {
            Series = series;
            Parse(gap);
        }

        public Dictionary<double, double> Parse(double gap)
        {
            Dictionary<double, double> result = new ();
            var f = Series.Series[0];
            //Cauchy bounds
            double a = f.CoeficientsArray[f.Power];
            double max = Math.Abs(f.CoeficientsArray[0] / a);
            for (int i = f.Power-1; i > 0; i--)
            {
                var r = Math.Abs(f.CoeficientsArray[i] / a);
                max = r > max ? r : max;
            }
            max += 2;
            Localize(-max, max, gap, result);
            LastResult = new Dictionary<double, double>(result);
            return result;  
        }

        void Localize(double a, double b, double gap, Dictionary<double, double> zones)
        {
            int check = RootsNumber(a, b);
            if (b - a < gap)
            {
                if (check == 1)
                {
                    zones.Add(a, b);
                    return;
                }
                if (check == 0)
                    return;
            }
            double mid = (a + b) / 2;
            if (RootsNumber(a, mid) > 0)
                Localize(a, mid, gap, zones);
            if (RootsNumber(mid, b) > 0)
                Localize(mid, b, gap, zones);
            return;
        }

        public int AllRootsCount() => RootsNumber(Double.NegativeInfinity, Double.PositiveInfinity);

        public int RootsNumber(double a, double b)
        {
            if (a == b) throw new Exception("Invalid bounds");
            int oa, ob;
            oa = Omega(a);
            ob = Omega(b);
            return a < b ? oa - ob : ob - oa;
        }

        int Omega(double x)
        {
            double[] seq = new double[Series.Power + 1];
            for(int i = 0; i <= Series.Power; i++)
                seq[i] = Series.Series[i].Calculate(x);
            return SignChanges(seq);
        }

        int SignChanges(double[] vals)
        {
            if (!vals.Any()) return 0;

            int res = 0;
            double prev = vals[0];
            foreach(var a in vals)
            {
                if (Math.Sign(a) != Math.Sign(prev) && a != 0)
                    res++;
                prev = a;
            }

            return res;
        }
        public override string ToString()
        {
            string res = "";
            int i = 1;
            foreach(var x in LastResult)
            {
                res += $"| {x.Key:0.000} < x{i} <= {x.Value:0.000} |";
                if (i % 2 == 0)
                    res += '\n';
                i++;
            }
            return res;
        }
    }
}
