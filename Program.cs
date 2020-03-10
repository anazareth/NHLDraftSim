using System;
using System.Collections.Generic;
using System.Linq;

namespace NHLDraftSim
{
    class Program
    {
        static List<String> lotteryTeamsOrig, lotteryTeams = new List<string>();
        static List<float> firstRoundOdds, secondRoundOdds, thirdRoundOdds;
        static void Main(string[] args)
        {
            initLottery();
            List<String> draftOrder = new List<string>();
            String firstPick = drawFirstOverall();
            lotteryTeams.Remove(firstPick);
            draftOrder.Add(firstPick);
            String secondPick = drawSecondOverall(firstPick);
            lotteryTeams.Remove(secondPick);
            draftOrder.Add(secondPick);
            String thirdPick = drawThirdOverall(secondPick);
            lotteryTeams.Remove(thirdPick);
            draftOrder.Add(thirdPick);

            draftOrder.AddRange(lotteryTeams);

            Console.WriteLine("2020 NHL Draft Order:");
            foreach (var item in draftOrder)
            {
                int origSeed = lotteryTeamsOrig.IndexOf(item) + 1;
                int seed = draftOrder.IndexOf(item) + 1;
                String posChange = origSeed > seed ? $"up {origSeed - seed}" : origSeed < seed ? $"down {seed - origSeed}" : "--";
                Console.WriteLine($"{item} ({posChange})");
            }
        }
        static void initLottery()
        {
            firstRoundOdds = new List<float>(){0f, 18.5f, 13.5f, 11.5f, 9.5f, 8.5f,
                                            7.5f, 6.5f, 6.0f, 5.0f, 3.5f,
                                            3.0f, 2.5f, 2.0f, 1.5f, 1.0f};
            secondRoundOdds = new List<float>(){0f, 16.5f, 13.0f, 11.3f, 9.6f, 8.7f,
                                            7.8f, 6.8f, 6.3f, 5.3f, 3.8f,
                                            3.3f, 2.7f, 2.2f, 1.7f, 1.1f};
            thirdRoundOdds = new List<float>(){0f, 14.4f, 12.3f, 11.1f, 9.7f, 8.9f,
                                            8.0f, 7.1f, 6.7f, 5.7f, 4.1f,
                                            3.6f, 3.0f, 2.4f, 1.8f, 1.2f};
            lotteryTeams.Add("Detroit Red Wings");
            lotteryTeams.Add("Los Angeles Kings");
            lotteryTeams.Add("Ottawa Senators");
            lotteryTeams.Add("San Jose Sharks --> Ottawa Senators"); // goes to Ottawa
            lotteryTeams.Add("Anaheim Ducks");
            lotteryTeams.Add("Buffalo Sabres");
            lotteryTeams.Add("New Jersey Devils");
            lotteryTeams.Add("Montreal Canadiens");
            lotteryTeams.Add("Chicago Blackawks");
            lotteryTeams.Add("Arizona Coyotes --> New Jersey Devils"); // goes to NJ
            lotteryTeams.Add("Winnipeg Jets");
            lotteryTeams.Add("Minnesota Wild");
            lotteryTeams.Add("Florida Panthers");
            lotteryTeams.Add("New York Rangers");
            lotteryTeams.Add("Columbus Blue Jackets");

            lotteryTeamsOrig = new List<string>(lotteryTeams);
        }
        static String drawFirstOverall()
        {
            List<float> oddsThreshold = new List<float>(firstRoundOdds.Count);
            foreach (var item in firstRoundOdds.Select((Value,Index) => new { Value, Index }))
            {
                if (item.Index == 0)
                    oddsThreshold.Add(firstRoundOdds[item.Index]);
                else
                    oddsThreshold.Add(firstRoundOdds[item.Index] + oddsThreshold[item.Index - 1]);
            }
            Random random = new Random();
            var drawVal = random.NextDouble() * 100;
            
            return binarySearch(drawVal, oddsThreshold, 1);
        }

        static String drawSecondOverall(String firstPick)
        {
            secondRoundOdds.Remove(lotteryTeams.IndexOf(firstPick));
            var newSum = secondRoundOdds.Sum();
            var newtest = secondRoundOdds.Select(x => x / newSum);
            List<float> oddsThreshold = new List<float>(secondRoundOdds.Count);
            foreach (var item in secondRoundOdds.Select((Value, Index) => new { Value, Index }))
            {
                if (item.Index == 0)
                    oddsThreshold.Add(secondRoundOdds[item.Index]);
                else
                    oddsThreshold.Add(secondRoundOdds[item.Index] + oddsThreshold[item.Index - 1]);
            }
            Random random = new Random();
            var drawVal = random.NextDouble() * 100;
            return binarySearch(drawVal, oddsThreshold, 2);
        }

        static String drawThirdOverall(String secondPick)
        {
            secondRoundOdds.Remove(lotteryTeams.IndexOf(secondPick));
            var newSum = thirdRoundOdds.Sum();
            var newtest = thirdRoundOdds.Select(x => x / newSum);
            List<float> oddsThreshold = new List<float>(thirdRoundOdds.Count);
            foreach (var item in thirdRoundOdds.Select((Value, Index) => new { Value, Index }))
            {
                if (item.Index == 0)
                    oddsThreshold.Add(thirdRoundOdds[item.Index]);
                else
                    oddsThreshold.Add(thirdRoundOdds[item.Index] + oddsThreshold[item.Index - 1]);
            }
            Random random = new Random();
            var drawVal = random.NextDouble() * 100;

            return binarySearch(drawVal, oddsThreshold, 3);
        }

        static String binarySearch(double target, List<float> oddsThreshold, int round)
        {
            int numTeams = firstRoundOdds.Count;
            int a = 0; int b = numTeams;
            while (true) // binary search implementation
            {
                int i = a + (b - a) / 2;
                if (target > oddsThreshold[i] && target <= oddsThreshold[i + 1])
                {
                    int origSeed = lotteryTeamsOrig.IndexOf(lotteryTeams[i]) + 1;
                    String posChange = origSeed > round ? $"up {origSeed - round}" : origSeed < round ? $"down {round - origSeed}" : "--";
                    return lotteryTeams[i];
                }
                else if (target > oddsThreshold[i])
                    a = i + 1;
                else
                    b = i - 1;
            }
        }
    }
}
