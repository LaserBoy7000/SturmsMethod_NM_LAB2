using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2CHM.Calc
{
    public class Polynomial
    {
        //max power
        public int Power { get; private set; }
        //list of coeficients <[power] : coeficient>
        public double[] CoeficientsArray { get => Coeficients.AsReadOnly().ToArray(); } 
        List<double> Coeficients { get; }

        Polynomial(int power, List<double> coeficients)
        {
            Power = power;
            Coeficients = coeficients;
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            List<double> cnew = new();

            var lx = a.Power < b.Power ? a : b;
            var mx = a.Power > b.Power ? a : -b; //form b we should take with inversion
            int i = 0;
            for(; i <= lx.Power; i++)
                cnew.Add(a.Coeficients[i] - b.Coeficients[i]);
            for (; i <= mx.Power; i++)
                cnew.Add(mx.Coeficients[i]);
            while(cnew.Last() == 0 && cnew.Any())
                cnew.RemoveAt(cnew.Count - 1);

            if(!cnew.Any())
                return new Polynomial(0, new() {0});
            return new Polynomial(cnew.Count - 1, cnew);

        }

        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            List<double> cnew = new();

            var lx = a.Power < b.Power ? a : b;
            var mx = a.Power > b.Power ? a : b;
            int i = 0;
            for (; i <= lx.Power; i++)
                cnew.Add(a.Coeficients[i] + b.Coeficients[i]);
            for (; i <= mx.Power; i++)
                cnew.Add(mx.Coeficients[i]);
            while (cnew.Last() == 0 && cnew.Any())
                cnew.RemoveAt(cnew.Count - 1);

            if (!cnew.Any())
                return new Polynomial(0, new() { 0 });
            return new Polynomial(cnew.Count - 1, cnew);
        }

        public static Polynomial operator -(Polynomial a)
        {
            List<double> cnew = new();
            for (int i = 0; i <= a.Power; i++)
                cnew.Add(-a.CoeficientsArray[i]);
            return new Polynomial(a.Power, cnew);

        }

        public static Polynomial operator *(Polynomial a, double b)
        {
            List<double> cnew = new();
            for (int i = 0; i <= a.Power; i++)
                cnew.Add(a.CoeficientsArray[i] * b);
            return new Polynomial(a.Power, cnew);

        }

        public static Polynomial operator /(Polynomial a, double b)
        {
            List<double> cnew = new();
            for (int i = 0; i <= a.Power; i++)
                cnew.Add(a.CoeficientsArray[i] / b);
            return new Polynomial(a.Power, cnew);

        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            int sz = a.Power + b.Power;
            List<double> cnew = new(new double[sz + 1]);
            for(int i = 0; i <= a.Power; i++)
                for(int j = 0; j <= b.Power; j++)
                    cnew[i+j] += a.CoeficientsArray[i] * b.CoeficientsArray[j];
            while (cnew.Last() == 0 && cnew.Any())
                cnew.RemoveAt(cnew.Count - 1);
            if (!cnew.Any())
                return new Polynomial(0, new() { 0 });
            return new Polynomial(cnew.Count - 1, cnew);
        }

        public static (Polynomial, Polynomial) operator /(Polynomial a, Polynomial b)
        {
            if (a.Power < b.Power) throw new Exception("Unsupported division type: a < b");
            int lgh = a.Power - b.Power;
            List<double> res = new(new double[lgh + 1]);
            var one = new Polynomial(a.Power, new List<double>(a.Coeficients));
            for(; one.Power > 0 && one.Power >= b.Power; lgh--)
            {
                if (a.Coeficients.Last() == 0) continue;
                var k = one.Coeficients.Last() / b.Coeficients.Last();
                var mp = (b * k) >> lgh;
                var p = one.Power;
                one = one - mp;
                if (one.Power == p)
                    one.Cut();
                res[lgh] = k;
            }
            return (new Polynomial(res.Count - 1, res), one);
        }

        void Cut() => Coeficients.RemoveAt(Power--);

        public Polynomial Differentiate()
        {
            if(Power == 0) return new Polynomial(0, new() { 0 });
            List<double> cnew = new();
            for (int i = 1; i <= Power; i++)
                cnew.Add(Coeficients[i] * i);
            return new Polynomial(cnew.Count - 1, cnew);
        }

        public static Polynomial operator >>(Polynomial a, int b)
        {
            if(b <= 0) return new Polynomial(a.Power, new List<double>(a.Coeficients));
            List<double> cnew = new(new double[b + a.Power + 1]);
            int i;
            for(i = b+a.Power; i >= b; i--)
                cnew[i] = a.Coeficients[i - b];
            for (; i >= 0; i--)
                cnew[i] = 0;
            return new Polynomial(cnew.Count - 1, cnew);
        }

        public static Polynomial FromEquation(string equation)
        {
            if(!equation.Contains('=')) throw new Exception("Invalid input");

            string[] prt = equation.Split('=');
            Polynomial a = FromFunction(prt[0]);
            Polynomial b = FromFunction(prt[1]);
            return a - b;
        }

        public static Polynomial FromFunction(string function)
        {
            //parse matches
            var rx = new Regex(@"(?<coefa>(\- *)?[0-9]+((\.|\,)[0-9]+)? *)((?<xa>[a-z])+(\^(?<powa>[0-9]+))?)?|(?<coefb>(\- *)?([0-9]+((\.|\,)[0-9]+)? *)?)((?<xb>[a-z])+(\^(?<powb>[0-9]+))?)");
            var mch = rx.Matches(function);

            if (mch == null) throw new Exception("Invalid input");

            var suc = mch.Where(x => x.Success);

            if (!suc.Any()) throw new Exception("Invalid input");

            Dictionary<int, double> entr = new();

            foreach (Match m in suc)
            {
                //XOR combining
                bool ex = m.Groups["xa"].Value.Combine(m.Groups["xb"].Value) != "";
                string coef = m.Groups["coefa"].Value.Combine(m.Groups["coefb"].Value);
                string pow = m.Groups["powa"].Value.Combine(m.Groups["powb"].Value);
                var c = coef switch
                {
                    "-" => -1,
                    "" => 1,
                    _ => double.Parse(coef.Replace(" ","").Replace(',','.'), CultureInfo.InvariantCulture),
                };
                var p = 0;
                if (pow != "")
                    p = int.Parse(pow);
                else p = ex ? 1 : p;
                if(!entr.ContainsKey(p))
                    entr.Add(p, c);
                else entr[p] += c;
                
            }

            List<double> res = new();

            //linking coeficiets with powers
            int mpow = entr.Keys.Max();
            for (int i = 0; i <= mpow; i++)
                if (entr.ContainsKey(i))
                    res.Add(entr[i]);
                else
                    res.Add(0);

            return new Polynomial(mpow, res);
        }

        public double Calculate(double x)
        {
            double res = 0;
            for(int i = 0; i <= Power; i++)
                res += Math.Pow(x, i) * Coeficients[i];
            return res;
        }

        public override string ToString()
        {
            string res = "";
            for(int i = Power; i >= 0; i--)
            {
                if (Coeficients[i] == 0)
                    continue;
                
                if (Coeficients[i] < 0) res += " - ";
                else if(i < Power) res += " + ";

                double k = Math.Abs(Coeficients[i]);
                res += $"{k:0.00}";

                if (i > 0)
                {
                    res += "x";
                    if (i > 1)
                        res += $"^{i}";
                }
            }
            return res;
        }
    }

    public static class StringExtension
    {
        public static string Combine(this string one, string two)
        {
            if (string.IsNullOrEmpty(one))
                return two;
            else return one;
        }
    }
}
