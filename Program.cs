using System;
using System.Collections.Generic;
using System.Linq;

namespace NHLDraftSim
{
    class Program
    {
        static List<String> lotteryTeams = new List<string>();
        static List<float> lotteryOdds;
        static void Main(string[] args)
        {
            initLottery();
            String firstPick = drawFirstOverall();
            //draw 
        }
        static void initLottery()
        {
            lotteryOdds = new List<float>(){0f, 18.5f, 13.5f, 11.5f, 9.5f, 8.5f,
                                            7.5f, 6.5f, 6.0f, 5.0f, 3.5f,
                                            3.0f, 2.5f, 2.0f, 1.5f, 1.0f};
            lotteryTeams.Add("Detroit Red Wings");
            lotteryTeams.Add("Los Angeles Kings");
            lotteryTeams.Add("Ottawa Senators");
            lotteryTeams.Add("San Jose Sharks"); // goes to Ottawa
            lotteryTeams.Add("Anaheim Ducks");
            lotteryTeams.Add("Buffalo Sabres");
            lotteryTeams.Add("New Jersey Devils");
            lotteryTeams.Add("Montreal Canadiens");
            lotteryTeams.Add("Chicago Blackawks");
            lotteryTeams.Add("Arizona Coyotes"); // goes to NJ
            lotteryTeams.Add("Winnipeg Jets");
            lotteryTeams.Add("Minnesota Wild");
            lotteryTeams.Add("Florida Panthers");
            lotteryTeams.Add("New York Rangers");
            lotteryTeams.Add("Columbus Blue Jackets");
        }
        static String drawFirstOverall()
        {
            List<float> oddsThreshold = new List<float>(lotteryOdds.Count);
            foreach (var item in lotteryOdds.Select((Value,Index) => new { Value, Index }))
            {
                if (item.Index == 0)
                    oddsThreshold.Add(lotteryOdds[item.Index]);
                else
                    oddsThreshold.Add(lotteryOdds[item.Index] + oddsThreshold[item.Index - 1]);
            }
            Random random = new Random();
            var drawVal = random.NextDouble() * 100;
            int numTeams = lotteryOdds.Count;
            int a = 0; int b = numTeams;
            while (true)
            {
                int i = a + (b - a) / 2;
                if (drawVal > oddsThreshold[i] && drawVal <= oddsThreshold[i + 1])
                {
                    String posChange = i > 0 ? $"up {i}" : i < 0 ? $"down {i}" : "no change";
                    Console.WriteLine($"Congratulations to the {lotteryTeams[i]} on the first overall pick. ({posChange})");
                    return lotteryTeams[i];
                }
                else if (drawVal > oddsThreshold[i])
                    a = i + 1;
                else
                    b = i - 1;
            }
        }
    }
}
