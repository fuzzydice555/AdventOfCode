﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode {
    public class Day3 {
        public static void SolvePart1() {
            int part1 = 0;
            var lines = File.ReadAllLines("day3input.txt");
            using (StreamWriter outputFile = new StreamWriter("day3output.txt")) {

                for (int i = 0; i < lines.Length; i++) {
                    var line = lines[i];
                    outputFile.Write(line + "   ");
                    string lineBefore = null;
                    string lineAfter = null;
                    if (i >= 1) lineBefore = lines[i - 1];
                    if (lines.Length > i + 1) lineAfter = lines[i + 1];

                    string actNumber = "";
                    bool isPart = false;
                    for (int j = 0; j < line.Length; j++) {
                        char c = line[j];
                        if (char.IsNumber(c)) {
                            actNumber += char.GetNumericValue(c).ToString();
                            if (!isPart && (CheckPartChar(lineBefore, j - 1)
                                || CheckPartChar(lineBefore, j)
                                || CheckPartChar(lineBefore, j + 1)
                                || CheckPartChar(line, j - 1)
                                || CheckPartChar(line, j + 1)
                                || CheckPartChar(lineAfter, j - 1)
                                || CheckPartChar(lineAfter, j)
                                || CheckPartChar(lineAfter, j + 1))) {
                                isPart = true;
                            }
                        }
                        else {
                            SetNum(ref part1, outputFile, ref actNumber, ref isPart);
                        }
                    }
                    SetNum(ref part1, outputFile, ref actNumber, ref isPart);
                    outputFile.WriteLine();
                }
            }
            Console.WriteLine($"line: {part1}");
        }
        public static void SolvePart2() {
            int part2 = 0;
            var lines = File.ReadAllLines("day3input.txt");
            Dictionary<Point, List<int>> numbersForGears = new Dictionary<Point, List<int>>();
            Dictionary<int, int> numbersByID = new Dictionary<int, int>();
            using (StreamWriter outputFile = new StreamWriter("day3output.txt")) {
                for (int i = 0; i < lines.Length; i++) {
                    var line = lines[i];
                    outputFile.Write(line + "   ");
                    string lineBefore = null;
                    string lineAfter = null;
                    if (i >= 1) lineBefore = lines[i - 1];
                    if (lines.Length > i + 1) lineAfter = lines[i + 1];
                    string actNumber = "";
                    int actID = 0;
                    List<Point> gearCharsForThisNum = new List<Point>();
                    for (int j = 0; j < line.Length; j++) {
                        char c = line[j];
                        if (char.IsNumber(c)) {
                            if (actNumber == "") actID = numbersByID.Count;
                            actNumber += char.GetNumericValue(c).ToString();
                            AssignGearChar(lineBefore, i - 1, j - 1, actID, numbersForGears);
                            AssignGearChar(lineBefore, i - 1, j, actID, numbersForGears);
                            AssignGearChar(lineBefore, i - 1, j + 1, actID, numbersForGears);
                            AssignGearChar(line, i, j - 1, actID, numbersForGears);
                            AssignGearChar(line, i, j + 1, actID, numbersForGears);
                            AssignGearChar(lineAfter, i + 1, j - 1, actID, numbersForGears);
                            AssignGearChar(lineAfter, i + 1, j, actID, numbersForGears);
                            AssignGearChar(lineAfter, i + 1, j + 1, actID, numbersForGears);
                        }
                        else {
                            SetNumPart2(outputFile, ref actNumber, actID, numbersByID);
                        }
                    }
                    SetNumPart2(outputFile, ref actNumber, actID, numbersByID);
                    outputFile.WriteLine();
                }
            }
            foreach (var gear in numbersForGears.Where(x => x.Value.Count == 2)) {
                int ratio = numbersByID[gear.Value[0]] * numbersByID[gear.Value[1]];
                part2 += ratio;
            }
            Console.WriteLine($"Part:{part2}");
        }

        private static void SetNum(ref int part1, StreamWriter outputFile, ref string actNumber, ref bool isPart) {
            if (actNumber.Length > 0 && isPart) {
                outputFile.Write(actNumber + " ");
                part1 += int.Parse(actNumber);
            }
            actNumber = "";
            isPart = false;
        }

        private static void SetNumPart2(StreamWriter outputFile, ref string actNumber, int numId, Dictionary<int, int> numbersByID) {
            if (actNumber.Length > 0) {
                outputFile.Write(actNumber + " ");
                numbersByID[numId] = int.Parse(actNumber);
            }
            actNumber = "";
        }

        private static void AssignGearChar(string line, int i, int j, int numID, Dictionary<Point, List<int>> numbersForGears) {
            if (string.IsNullOrEmpty(line) || j < 0 || j >= line.Length) return;
            var c = line[j];
            if (c == '*') {
                var p = new Point(i, j);
                List<int> numIdsForThisGear;
                if (!numbersForGears.TryGetValue(p, out numIdsForThisGear)) {
                    numIdsForThisGear = new List<int>();
                    numbersForGears.Add(p, numIdsForThisGear);
                }
                if (!numIdsForThisGear.Contains(numID)) numIdsForThisGear.Add(numID);
            }
        }

        private static bool CheckPartChar(string line, int index) {
            if (string.IsNullOrEmpty(line) || index < 0 || index >= line.Length) return false;
            var c = line[index];
            if (!char.IsNumber(c) && c != '.') return true;
            return false;
        }
    }
}
