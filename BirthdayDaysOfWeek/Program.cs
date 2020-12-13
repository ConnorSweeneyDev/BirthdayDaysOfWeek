using System;
using System.Collections.Generic;
using System.Linq;

namespace BirthdayDaysOfWeek
{
    class Program
    {
        static void Main()
        {
            Console.SetWindowSize(111, 46);

            int repeat = 1;
            while (repeat == 1)
            {
                //creates a dictionary and adds 7 days to it (0-6)
                Dictionary<int, List<string>> days = new Dictionary<int, List<string>>();
                for (int d = 0; d < 7; d++)
                    days.Add(d, new List<string>());

                Console.Write("Please enter your date of birth.    [DD-MM-YYYY]: ");
                string DOB = Console.ReadLine();
                Console.Write("How many years do you want to calculate? [0-100]: ");
                string choice = Console.ReadLine();

                string day = DOB.Substring(0, 2);
                string month = DOB.Substring(3, 2);
                string FourDigitYear = DOB.Substring(6, 4);
                string TwoDigitYear = DOB.Substring(8, 2);
                string century = DOB.Substring(6, 2);
                string HorizontalLine = string.Concat(Enumerable.Repeat("-", 35));

                double DoubleChoice = System.Convert.ToDouble(choice);
                double DoubleDay = System.Convert.ToDouble(day);
                double DoubleMonth = System.Convert.ToDouble(month);
                double DoubleFourDigitYear = System.Convert.ToDouble(FourDigitYear);
                double DoubleCentury = System.Convert.ToDouble(century);
                double DoubleTwoDigitYear = System.Convert.ToDouble(TwoDigitYear);
                double LeapYearCode = 0;

                //all codes are from https://beginnersbook.com/2013/04/calculating-day-given-date/ under "the key value method"
                double[] MonthCodes = { 0, 1, 4, 4, 0, 2, 5, 0, 3, 6, 1, 4, 6 };
                double[] CenturyCodes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 2, 0, 6, 4, 2, 0, 6,
                                          4, 2, 0, 6, 4, 2, 0, 6, 4, 2, 0, 6, 4, 2, 0, 6, 4, 2, 0, 6, 4, 2, 0, 6, 4 };
                string[] DayNames = { "| Saturday  ", "| Sunday    ", "| Monday    ", "| Tuesday   ",
                                      "| Wednesday ", "| Thursday  ", "| Friday    " };

                double x = 0;

                //allows only how many years the user chose to calculate
                while (x <= (DoubleChoice - 1))
                {
                    double MonthCode = MonthCodes[(int)DoubleMonth];
                    double CenturyCode = CenturyCodes[(int)DoubleCentury];

                    //checks whether the year is a leap year - divisible by 4, not by 100, unless by 400
                    if ((DoubleFourDigitYear % 4 == 0 && DoubleFourDigitYear % 100 != 0) || (DoubleFourDigitYear % 400 == 0))
                    {
                        LeapYearCode = 1;
                    }
                    else
                    {
                        LeapYearCode = 0;
                    }

                    //the formula is from https://beginnersbook.com/2013/04/calculating-day-given-date/ under "the key value method"
                    double F1 = (DoubleTwoDigitYear / 4);
                    F1 = Math.Floor(F1);
                    double F2 = Math.Floor(F1 + DoubleDay + MonthCode + CenturyCode + DoubleTwoDigitYear);

                    //accounting for the leap year
                    if ((DoubleMonth == 1 || DoubleMonth == 2) && LeapYearCode == 1)
                        F2--;

                    //mod the current answer by 7 to get the final answer, then convert that to int to avoid errors
                    double WeekDay = F2 % 7;
                    int IntWeekDay = System.Convert.ToInt32(WeekDay);

                    //adds the year to the specified day's list
                    days[IntWeekDay].Add(System.Convert.ToString(DoubleFourDigitYear));

                    DoubleFourDigitYear++;
                    DoubleTwoDigitYear++;
                    x++;
                }

                int MaxLength = 0;

                //algorithm to keep track of the highest year count in one day so far
                foreach (KeyValuePair<int, List<string>> Day in days)
                {
                    if (Day.Value.Count > MaxLength)
                    {
                        MaxLength = Day.Value.Count;
                    }
                }

                Console.WriteLine("");
                Console.WriteLine(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                    + ("The days of the week your past and future birthdays are on are as follows:"));

                //centering the table
                Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength))) + HorizontalLine
                    + string.Concat(Enumerable.Repeat("-----", MaxLength)) + "-");

                //checking for a count of 0, as that would result in an unfinshed table
                if (MaxLength == 0)
                {
                    Console.WriteLine("-----");
                }
                else
                {
                    Console.WriteLine("");
                }

                foreach (KeyValuePair<int, List<string>> Day in days)
                {
                    //counting the number of digits in day value count, so the output can be formatted nicely
                    double DigitCount = Math.Floor(Math.Log10(Day.Value.Count) + 1);

                    //centering the table
                    //starting from key value 0 --> [day name] | count = [count of years in day] | Years = ...
                    if (DigitCount == 2)
                    {
                        Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                            + DayNames[Day.Key] + "| Count = " + Day.Value.Count + " | Years = ");
                    }
                    else if (DigitCount == 1)
                    {
                        Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                            + DayNames[Day.Key] + "| Count = " + Day.Value.Count + "  | Years = ");
                    }
                    else
                    {
                        Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                            + DayNames[Day.Key] + "| Count = " + Day.Value.Count + "  | Years = N/A");
                    }

                    //... [list of years in given day]
                    foreach (string i in Day.Value)
                    {
                        Console.Write(i + " ");
                    }

                    int spaces = 0;

                    //calculates how many spaces to add after the list of years using MaxLength for nice table formatting
                    if (MaxLength > Day.Value.Count)
                    {
                        spaces = MaxLength - Day.Value.Count;
                    }

                    //since "N/A" is shorter than "YYYY" a count of 0 gets its own statement that subtracts 1 from the total
                    if (Day.Value.Count == 0)
                    {
                        Console.Write("  ");
                        spaces -= 1;
                    }

                    //adds the total spaces calculated previously
                    for (int space = 0; space < spaces; space++)
                    {
                        Console.Write("     ");
                    }
                    Console.Write("|");
                    Console.WriteLine("");
                }

                //centering the table
                Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength))) + HorizontalLine
                    + string.Concat(Enumerable.Repeat("-----", MaxLength)) + "-");

                //checking for a count of 0, as that would result in an unfinshed table
                if (MaxLength == 0)
                {
                    Console.WriteLine("-----");
                }
                else
                {
                    Console.WriteLine("");
                }

                int y = 1;

                //checking whether the user will enter either "y" or "n" to restart or exit respectively
                while (y == 1)
                {
                    Console.Write(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                        + ("Do you want to try another date? (y/n): "));
                    string again = Console.ReadLine();

                    if (again == "n")
                    {
                        repeat--;
                        y--;
                    }
                    else if (again == "y")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("");
                        y--;
                    }
                    else
                    {
                        Console.WriteLine(string.Concat(Enumerable.Repeat("  ", (15 - MaxLength)))
                            + ("That is not a valid option."));
                        Console.WriteLine("");
                    }
                }
            }
        }
    }
}