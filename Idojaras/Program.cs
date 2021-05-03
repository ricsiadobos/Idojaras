using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Idojaras
{
    class Program
    {
        public class Adat
        {
            public int ev;
            public int honap;
            public int nap;
            public double levegoMaxHo;
            public double levegoMinHo;
            public double talajHo;
            public int paratartlom;
            //7-8 feladatokhoz segéd változók 
            public double csapadek;
            public double haviAtlag;
            public int honapok;


            public Adat(int ev, int honap, int nap, double levegoMaxHo, double levegoMinHo, double talajHo, int paratartlom, double csapadek)
            {
                this.ev = ev;
                this.honap = honap;
                this.nap = nap;
                this.levegoMaxHo = levegoMaxHo;
                this.levegoMinHo = levegoMinHo;
                this.talajHo = talajHo;
                this.paratartlom = paratartlom;
                this.csapadek = csapadek;
            }

            public Adat(double haviatlag, int honap)
            {
                this.haviAtlag = haviatlag;
                this.honapok = honap;
            }

            public Adat()
            {
              
            }

        }

        static void Main(string[] args)
        {
            #region 2. feladat

            FileStream file = new FileStream(@"idojaras.csv", FileMode.Open);
            StreamReader sr = new StreamReader(file);
           List<Adat> list = new List<Adat>();

            // év, hónap, nap, levegő, max hő, min hő, talaj hő, pára, csapadék (mm)
            string fejlec = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                string[] db = sor.Split(';');

                for (int i = 0; i < db.Length; i++)
                {
                    if (db[i].Contains(','))
                    {
                        db[i].Replace(',', '.');
                    }

                    else if (db[i].Length == 0)
                    {
                        db[i] = "0";
                    }
                }

                Adat adat = new Adat(Convert.ToInt32(db[0]), Convert.ToInt32(db[1]), Convert.ToInt32(db[2]), Convert.ToDouble(db[3]), 
                Convert.ToDouble(db[4]), Convert.ToDouble(db[5]), Convert.ToInt32(db[6]), Convert.ToDouble(db[7]));
                list.Add(adat);
            }
            #endregion

            feladat3(list);

            feladat4(list);

            feladat5(list);

            feladat6(list);

            feladat7(list);

            feladat8(list);
            
            Console.ReadLine();

        }


        private static void feladat8(List<Adat> list)
        {
            List<Adat> haviCsapadekLista = new List<Adat>();

            int honapok = 1;
                double havicsap=0;

            while (honapok < 13)
            {
                List<double> haviAtlag = new List<double>();

                foreach (Adat item in list)
                {
                    if (item.honap.Equals(honapok))
                    {
                        havicsap += (double)item.csapadek;
                    }
                }

                Adat haviCsapatek = new Adat(havicsap, honapok);
                haviCsapadekLista.Add(haviCsapatek);
                honapok++;
                havicsap = 0;
            }

            Adat MaxCsap = haviCsapadekLista.First(x=>x.haviAtlag==list.OrderByDescending(z=> z.haviAtlag).Max(y => y.haviAtlag));
            int legCsapadekosabbHonap = MaxCsap.honapok;
            double legMagasabbHo = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].honap==legCsapadekosabbHonap)
                {
                    if (legMagasabbHo<list[i].levegoMaxHo)
                    {
                        legMagasabbHo = list[i].levegoMaxHo;
                    }
                }
            }
            Console.WriteLine("8. feladat: \nA legcsapadékosabb hónapban mennyi volt a maximalis hőmérsékelete a levegőnek:\t" + Math.Round(legMagasabbHo) + "°C");

            Console.ReadLine();
        }

        private static void feladat7(List<Adat> list)
        {
            List<Adat> haviAtlagLista = new List<Adat>();

            int honapok = 1;
            while (honapok<13)
            {
            List<double> napiatlag = new List<double>();

                foreach (Adat item in list)
                {
                        if (item.honap.Equals(honapok))
                        {
                                napiatlag.Add((item.levegoMinHo + item.levegoMaxHo) / 2);
                        }
                }

                double haviAtlag = Convert.ToDouble(napiatlag.Average(x => x));
                Adat haviadatoltes = new Adat(haviAtlag, honapok);
                haviAtlagLista.Add(haviadatoltes);
                honapok++;
            }
            Console.WriteLine("7. feladat: \nMelyik hónapban mennyi volt az átlaghőmérséklet:");
            haviAtlagLista.ForEach(x => Console.Write("  " + x.honapok + "\t"));
            Console.WriteLine();
            haviAtlagLista.ForEach(x => Console.Write(Math.Round(x.haviAtlag, 2) + "\t"));
            Console.WriteLine();
            // A példában a számozás és az átlaghőmérséklet utolsó karaktere egy vonalban van!
        }

        private static void feladat6(List<Adat> list)
        {
            Console.WriteLine("\n6. feladat: \nDecemberben mely napokon volt hiányos az adatszolgáltatás a páratartamat illetően: ");
            List<Adat> hianyosDecember = list.Where(x => x.honap.Equals(12)).Where(x => x.paratartlom.Equals(0)).ToList();
            hianyosDecember.ForEach(x => Console.WriteLine("\t"+ x.ev + ". " + x.honap + ". " + x.nap + "."));  
        }

        private static void feladat5(List<Adat> list)
        {
            
            Adat marcLegcsapadekosabbNap = list.Where(x => x.honap.Equals(3)).OrderByDescending(x => x.csapadek).First();
            Console.Write("5. feladat: \nMárcius melyik napján esett a legtöbb csapadék és mennyi: ");
            Console.Write(marcLegcsapadekosabbNap.ev.ToString()+ ". ");
            Console.Write(marcLegcsapadekosabbNap.honap.ToString()+ ". ");
            Console.Write(marcLegcsapadekosabbNap.nap.ToString() + "., ");
            Console.Write(Math.Round(marcLegcsapadekosabbNap.csapadek, 2).ToString() + " mm");
             }

        private static void feladat4(List<Adat> list)
        {
            Console.Write("4. feladat: \nFebruárban mekkora volt az átlaghőmérséklet 10cm-el a talaj felszíne alatt: ");
            double febatlag = list.Where(x => x.honap.Equals(2)).Average(x => x.talajHo);
            Console.WriteLine(Math.Round(febatlag, 2)+ "°C");

        }

        private static void feladat3(List<Adat> list)
        {
            Console.Write("3. feladat: \nJanuár 1.-én mekkora volt a maximalis hőmérséklet: ");
            foreach (Adat item in list)
            {
                if (item.honap== 1)
                {
                    if (item.nap==1)
                    {
                        Console.WriteLine(item.levegoMaxHo + "°C");

                    }

                }

            } 
        }
    }
}
