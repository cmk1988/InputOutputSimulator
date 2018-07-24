using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InputOutputSimulator
{
    public class BusinessLogic
    {
        readonly int siebenUhrX;
        readonly int siebenUhrY;
        readonly int zeilenAbstand;
        readonly string schule;
        readonly string arbeit;
        readonly string ueberstundenabbau;
        readonly string urlaub;
        readonly string kommen;
        readonly string gehen;
        readonly string pausenbeginn;
        readonly string pausenende;
        readonly string raucherpausebeginn;
        readonly string raucherpauseende;
        readonly string projekt;

        public BusinessLogic(
            int siebenUhrX,
            int siebenUhrY,
            int zeilenAbstand,
            string schule,
            string arbeit,
            string ueberstundenabbau,
            string urlaub,
            string kommen,
            string gehen,
            string pausenbeginn,
            string pausenende,
            string raucherpausebeginn,
            string raucherpauseende,
            string projekt
            )
        {
            this.siebenUhrX = siebenUhrX;
            this.siebenUhrY = siebenUhrY;
            this.zeilenAbstand = zeilenAbstand;
            this.schule = schule;
            this.arbeit = arbeit;
            this.ueberstundenabbau = ueberstundenabbau;
            this.urlaub = urlaub;
            this.kommen = kommen;
            this.gehen = gehen;
            this.pausenbeginn = pausenbeginn;
            this.pausenende = pausenende;
            this.raucherpausebeginn = raucherpausebeginn;
            this.raucherpauseende = raucherpauseende;
            this.projekt = projekt;
        }

        public void UndLos(string str)
        {
            var str1 = str.Split('\n');
            var list = new List<DTO>();
            foreach (var s in str1)
            {
                var str2 = s.Split('\t');
                var dto = new DTO
                {
                    H = Int32.Parse(str2[0].Substring(0, 2)),
                    M = Int32.Parse(str2[0].Substring(3, 2)),
                    What = str2[1],
                    Act = str2[2]
                };
                dto.RelativeZeilenVonSiebenUhr = (dto.H - 7) * 4 + (dto.M + 7) / 15;
                list.Add(dto);
            }
            var pausenMinutenBeiArbeit = 0;
            var pausenMinutenBeiUrlaub = 0;
            var b = false;
            DTO beginn = null;
            foreach (var a in list)
            {
                if (b)
                {
                    if(a.What == arbeit)
                        pausenMinutenBeiArbeit += (a.M - beginn.M) <= 0 ? (a.H - beginn.H) - (a.M - beginn.M) : (a.M - beginn.M);
                    else if (a.What == urlaub)
                        pausenMinutenBeiUrlaub += (a.M - beginn.M) <= 0 ? (a.H - beginn.H) - (a.M - beginn.M) : (a.M - beginn.M);
                    b = false;
                }
                else if(a.What == arbeit || a.What == urlaub)
                {
                    if(a.Act == pausenbeginn || a.Act == raucherpausebeginn)
                    {
                        beginn = a;
                        b = true;
                    }
                }
            }
            var pauseInSchrittenBeiArbeit = (pausenMinutenBeiArbeit + 7) / 15;
            var pauseInSchrittenBeiUrlaub = (pausenMinutenBeiUrlaub + 7) / 15;
            if (list.Exists(x => x.What == schule))
            {
                var k = list.FirstOrDefault(x => x.What == schule && x.Act == kommen);
                var g = list.FirstOrDefault(x => x.What == schule && x.Act == gehen);
                Class1.ClickFromTo(siebenUhrX, k.RelativeZeilenVonSiebenUhr, g.RelativeZeilenVonSiebenUhr, siebenUhrX, "{S}");
            }
            if (list.Exists(x => x.What == arbeit))
            {
                var k = list.FirstOrDefault(x => x.What == arbeit && x.Act == kommen);
                var g = list.FirstOrDefault(x => x.What == arbeit && x.Act == gehen);
                Class1.ClickFromTo(siebenUhrX, k.RelativeZeilenVonSiebenUhr, siebenUhrX, g.RelativeZeilenVonSiebenUhr, projekt);
                if (pauseInSchrittenBeiArbeit > 0)
                {
                    var t = k.RelativeZeilenVonSiebenUhr - g.RelativeZeilenVonSiebenUhr;
                    var start = k.RelativeZeilenVonSiebenUhr + (t - pauseInSchrittenBeiArbeit) / 2;
                    var ende = start + k.RelativeZeilenVonSiebenUhr;
                    Thread.Sleep(100);
                    Class1.ClickFromTo(siebenUhrX, start, siebenUhrX, ende, "{P}");
                }
            }
            if (list.Exists(x => x.What == ueberstundenabbau))
            {
                var k = list.FirstOrDefault(x => x.What == ueberstundenabbau && x.Act == kommen);
                var g = list.FirstOrDefault(x => x.What == ueberstundenabbau && x.Act == gehen);
                Class1.ClickFromTo(siebenUhrX, k.RelativeZeilenVonSiebenUhr, siebenUhrX, g.RelativeZeilenVonSiebenUhr, "{Ü}");
            }
            if (list.Exists(x => x.What == urlaub))
            {
                var k = list.FirstOrDefault(x => x.What == urlaub && x.Act == kommen);
                var g = list.FirstOrDefault(x => x.What == urlaub && x.Act == gehen);
                Class1.ClickFromTo(siebenUhrX, k.RelativeZeilenVonSiebenUhr, siebenUhrX, g.RelativeZeilenVonSiebenUhr, "{U}");
                if (pauseInSchrittenBeiArbeit > 0)
                {
                    var t = k.RelativeZeilenVonSiebenUhr - g.RelativeZeilenVonSiebenUhr;
                    var start = k.RelativeZeilenVonSiebenUhr + (t - pauseInSchrittenBeiArbeit) / 2;
                    var ende = start + k.RelativeZeilenVonSiebenUhr;
                    Thread.Sleep(100);
                    Class1.ClickFromTo(siebenUhrX, start, siebenUhrX, ende, "{P}");
                }
            }
        }
    }
}
