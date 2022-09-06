using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2CHM.Calc
{
    public class Program
    {
        public string Main(string equ, string gap)
        {
            if (!double.TryParse(gap.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double g))
                throw new Exception("Invalid input");
            var p = Polynomial.FromEquation(equ);
            var ser = new SturmSeries(p);
            return new SturmMethod(ser, g).ToString();
        }

        public string Echo(string equ)
        {
            var p = Polynomial.FromEquation(equ);
            return p.ToString();
        }
    }
}
