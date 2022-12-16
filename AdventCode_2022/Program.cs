using System;
using System.Collections.Generic; // Allows to use lists and enumerations
using System.IO; // Allows you to access files and inputs
using System.Linq; // Allows to use lambda expressions
using System.Threading.Tasks; // Allows async operations, multi threading
using Microsoft.Extensions.DependencyInjection;

namespace AdventCode_2022
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Program p = new Program(); // Call outside methods as this is a static method

                // Day 1
                if (args.Contains("1"))
                {
                    p.GetCalories1and2(1);
                    p.GetCalories1and2(3);
                }

                //Day 2
                if (args.Contains("2"))
                {
                    p.GetRPS(1);
                    p.GetRPS(2);
                }

                //Day 3
                if (args.Contains("3"))
                {
                    p.GetSumOfPriorities();
                    p.GetSumOfBadges();
                }

                //Day 4
                if (args.Contains("4"))
                {
                    p.GetContainedAssignments();
                    p.GetRepeatedAssignments();
                }

                //Day 5
                if (args.Contains("5"))
                {
                    p.Day5();
                    p.Day5();
                }

                // if empty, call latest
                if(args.Count() <= 0)
                {
                    p.Day5();
                    p.Day5();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #region Day1

        public void GetCalories1and2(int top)
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\1.1_input_raw.txt";
            var file = File.ReadAllLines(fileLocation); // Read files from given file location

            List<int> caloriesGroup = new List<int>(); // Using lists as we don´t have to define the size of an array
            var pivotSum = 0;
            List<int> maxInGroup = new List<int>();
            var maxSum = 0;

            foreach (string line in file)
            {
                if (String.IsNullOrEmpty(line)) // If the line is empty, consider it the end of the element
                {
                    caloriesGroup.Add(pivotSum);
                    pivotSum = 0;
                }
                else
                {
                    pivotSum += Int32.Parse(line);
                }
                //Console.WriteLine(line);
            }
            maxInGroup = GetMaxValues(caloriesGroup, top);
            foreach (int element in maxInGroup)
            {
                Console.WriteLine(element);
            }
            maxSum = Sum(maxInGroup);

            Console.WriteLine($"Total calories from top {top}: {maxSum}");
        }

        public List<int> GetMaxValues(List<int> list, int numberOfMax)
        {
            try
            {
                var orderedList = list.OrderBy(l => l); // Order list
                var listOfChosen = orderedList.TakeLast(numberOfMax); // Take last n elements, top values

                return listOfChosen.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return list;
            }
        }

        public int Sum(List<int> list)
        {
            try
            {
                var sum = 0;
                foreach (int element in list)
                {
                    sum += element;
                }

                return sum;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        #endregion
        #region Day2
        public void GetRPS(int version)
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\2.1_input.txt";
            var file = File.ReadAllLines(fileLocation);

            var rpsFileModels = new List<RockPaperScissorsModel>();

            foreach (var element in file)
            {
                var items = element.Split(' ');
                var won = version == 1 ? GetScore(items) // if I win, will be true; if I lost, will be false; if it's a draw, it will be null
                    : GetAlt(items);

                var model = new RockPaperScissorsModel
                {
                    ElfTurn = items[0],
                    MyTurn = items[1],
                    WonByElf = won[0],
                    WonByMe = won[1]
                };

                rpsFileModels.Add(model);
            }

            var totalMe = rpsFileModels.Select(rps => rps.WonByMe).Sum(rps => rps);
            var totalElf = rpsFileModels.Select(rps => rps.WonByElf).Sum(rps => rps);

            Console.WriteLine($"Won by me: {totalMe}");
            Console.WriteLine($"Won by elf: {totalElf}");
        }

        public int[] GetScore(string[] hands)
        {
            var wonByMe = 0;
            var wonByElf = 0;
            var collectionRPS = new string[] { "Rock", "Paper", "Scissors" };
            var collectionElf = new string[] { "A", "B", "C" };
            var collectionMe = new string[] { "X", "Y", "Z" };
            var collectionValues = new int[] { 1, 2, 3 };

            var elfIndex = Array.IndexOf(collectionElf, hands[0]);
            var myIndex = Array.IndexOf(collectionMe, hands[1]);

            var standardElfChoice = collectionRPS[elfIndex];
            var standardMyChoice = collectionRPS[myIndex];

            if (standardMyChoice == standardElfChoice)
            {
                wonByMe = 3; // draw
                wonByElf = 3; // draw
                //Console.WriteLine("It's a tie!");
            }
            else if (standardMyChoice == "Rock" && standardElfChoice == "Scissors")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Rock beats scissors.");
            }
            else if (standardMyChoice == "Paper" && standardElfChoice == "Rock")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Paper beats rock.");
            }
            else if (standardMyChoice == "Scissors" && standardElfChoice == "Paper")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Scissors beats paper.");
            }
            else
            {
                wonByMe = 0; // I win
                wonByElf = 6; // I win
                //Console.WriteLine("You lose! {0} beats {1}.", standardElfChoice, standardMyChoice);
            }

            wonByMe += collectionValues[myIndex];
            wonByElf += collectionValues[elfIndex];

            //Console.WriteLine($"I scored {wonByMe}, and the elf scored {wonByElf}");

            var won = new int[] { wonByElf, wonByMe };

            return won;
        }

        public int[] GetAlt(string[] hands)
        {
            var wonByMe = 0;
            var wonByElf = 0;
            var collectionRPS = new string[] { "Rock", "Paper", "Scissors" };
            var collectionElf = new string[] { "A", "B", "C" };
            //var collectionMe = new string[] { "X", "Y", "Z" };
            var collectionValues = new int[] { 1, 2, 3 };

            var elfIndex = Array.IndexOf(collectionElf, hands[0]);
            //var myIndex = Array.IndexOf(collectionMe, hands[1]);

            var standardElfChoice = collectionRPS[elfIndex];

            int myIndex;

            switch (hands[1])
            {
                case "X": // I lose
                    myIndex = elfIndex == 0 ? 2 : (elfIndex - 1);
                    break;
                case "Y": // Draw
                    myIndex = elfIndex;
                    break;
                case "Z": // I win
                    myIndex = elfIndex == 2 ? 0 : (elfIndex + 1);
                    break;
                default:
                    throw new Exception("Not mapped!");
                    break;

            }

            var standardMyChoice = collectionRPS[myIndex];

            if (standardMyChoice == standardElfChoice)
            {
                wonByMe = 3; // draw
                wonByElf = 3; // draw
                //Console.WriteLine("It's a tie!");
            }
            else if (standardMyChoice == "Rock" && standardElfChoice == "Scissors")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Rock beats scissors.");
            }
            else if (standardMyChoice == "Paper" && standardElfChoice == "Rock")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Paper beats rock.");
            }
            else if (standardMyChoice == "Scissors" && standardElfChoice == "Paper")
            {
                wonByMe = 6; // I win
                wonByElf = 0; // I win
                //Console.WriteLine("You win! Scissors beats paper.");
            }
            else
            {
                wonByMe = 0; // I win
                wonByElf = 6; // I win
                //Console.WriteLine("You lose! {0} beats {1}.", standardElfChoice, standardMyChoice);
            }

            wonByMe += collectionValues[myIndex];
            wonByElf += collectionValues[elfIndex];

            //Console.WriteLine($"I scored {wonByMe}, and the elf scored {wonByElf}");

            var won = new int[] { wonByElf, wonByMe };

            return won;
        }

        #endregion
        #region Day3
        public void GetSumOfPriorities()
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\3.1_input.txt";
            var file = File.ReadAllLines(fileLocation);
            int sumOfPriorities = 0;

            foreach (string sack in file)
            {
                var sackSize = sack.Length;
                var halfSack = sackSize / 2;
                var firstHalf = sack.Substring(0, halfSack);
                var secondHalf = sack.Substring(halfSack, halfSack);

                var commonElement = firstHalf.Intersect(secondHalf);
                char priorityElement;

                if (commonElement.Count() > 1)
                {
                    sumOfPriorities += commonElement.Sum(ce => GetPriority(ce));
                    //Console.WriteLine($"Full: {sack}, first half: {firstHalf}, second half {secondHalf}");
                }
                else if (commonElement.Count() == 1)
                {
                    priorityElement = commonElement.FirstOrDefault();
                    var priority = GetPriority(priorityElement);

                    sumOfPriorities += priority;
                    //Console.WriteLine($"Full: {sack}, first half: {firstHalf}, second half {secondHalf}, common element: {priorityElement}");
                }
                else
                {
                    break;
                }
            }

            //Console.WriteLine($"First element is sack {sack}, and common element is {commonElement}");
            Console.WriteLine($"Sum of priorities is {sumOfPriorities}");
        }

        public void GetSumOfBadges()
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\3.1_input.txt";
            var file = File.ReadAllLines(fileLocation);
            int sumOfPriorities = 0;

            var index = 0;
            var groupedFile = file.GroupBy(x => index++ / 3).ToList();

            foreach (var sack in groupedFile)
            {
                var list = sack.ToList();

                var common = (list[0].Intersect(list[1].Intersect(list[2]))).FirstOrDefault();
                sumOfPriorities += GetPriority(common);
            }

            Console.WriteLine($"Sum of priorities is {sumOfPriorities}");
        }

        public int GetPriority(char sackElement)
        {
            var sum = 0;
            var isLowerCase = sackElement.ToString() == sackElement.ToString().ToLower(); // Check if it's lower case;
            var aPriority = 1;
            var APriority = 27;

            int value = sackElement - (isLowerCase ? 'a' : 'A');

            sum = value + (isLowerCase ? aPriority : APriority);
            //Console.WriteLine($"Common element is {sackElement} and the priority is {sum}");

            return sum;
        }

        #endregion
        #region Day4
        public void GetContainedAssignments()
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\4.1_input.txt";
            var file = File.ReadAllLines(fileLocation);

            var count = 0;

            foreach (var pair in file)
            {
                var sections = pair.Split(",");
                var section1 = sections[0];
                var section2 = sections[1];
                var sectionPair1 = section1.Split("-");
                var sectionPair2 = section2.Split("-");

                if (Int32.Parse(sectionPair1[0]) >= Int32.Parse(sectionPair2[0]) && Int32.Parse(sectionPair1[1]) <= Int32.Parse(sectionPair2[1]))
                {
                    count++;
                }
                else if (Int32.Parse(sectionPair2[0]) >= Int32.Parse(sectionPair1[0]) && Int32.Parse(sectionPair2[1]) <= Int32.Parse(sectionPair1[1]))
                {
                    count++;
                }
            }

            Console.WriteLine($"Overlaps: { count }");
        }

        public void GetRepeatedAssignments()
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\4.1_input.txt";
            var file = File.ReadAllLines(fileLocation);

            var count = 0;

            foreach (var pair in file)
            {
                var sections = pair.Split(",");
                var section1 = sections[0];
                var section2 = sections[1];
                var sectionPair1 = section1.Split("-");
                var sectionPair2 = section2.Split("-");


                if (Int32.Parse(sectionPair1[1]) >= Int32.Parse(sectionPair2[0]) && Int32.Parse(sectionPair1[1]) <= Int32.Parse(sectionPair2[1]))
                {
                    count++;
                    //Console.WriteLine($"{sectionPair1[0]}-{sectionPair1[1]},{sectionPair2[0]}-{sectionPair2[1]}");
                }
                else if (Int32.Parse(sectionPair2[1]) >= Int32.Parse(sectionPair1[0]) && Int32.Parse(sectionPair2[1]) <= Int32.Parse(sectionPair1[1]))
                {
                    count++;
                    //Console.WriteLine($"{sectionPair1[0]}-{sectionPair1[1]},{sectionPair2[0]}-{sectionPair2[1]}");
                }
            }

            Console.WriteLine($"Overlaps: { count }");
        }
        #endregion
        #region Day5
        public void Day5()
        {
            var fileLocation = "C:\\Users\\Roderick\\Documents\\Advent of Code\\2022\\5.1_input.txt";
            var file = File.ReadAllLines(fileLocation);

        }
        #endregion
    }
}
