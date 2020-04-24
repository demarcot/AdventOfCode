using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode
{
    class Program
    {
        public static void Main(string[] args)
        {
            ExecuteDay14();
        }

        #region Day1
        public static void ExecuteDay1()
        {
            List<int> massList = new List<int>();

            //----------------------------
            //Read list from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_1_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                massList.Add(Int32.Parse(line));
            }
            file.Close();
            //----------------------------

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Calculate result
            float totalFuelReq = 0;
            float supplementalFuelReq = 0; // For part 2 - Calculate fuel needed for the added fuel
            foreach (int m in massList)
            {
                int curFuelReq = GetFuelReq(m);
                totalFuelReq += curFuelReq;

                // Calculate supplemantal fuel requirement
                while (curFuelReq > 8)
                {
                    int curSupplementalFuelReq = GetFuelReq(curFuelReq);
                    if (curSupplementalFuelReq > 0)
                    {
                        supplementalFuelReq += curSupplementalFuelReq;
                    }
                    curFuelReq = curSupplementalFuelReq;
                }
            }

            watch.Stop();

            // Display result
            Console.WriteLine(totalFuelReq + supplementalFuelReq + $" - Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        public static int GetFuelReq(int m)
        { 
            return (int)MathF.Floor(m / 3.0f) - 2;
        }
        #endregion

        #region Day2
        public static void ExecuteDay2()
        {
            int result = -1;
            List<int> originalProgram = new List<int>();

            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_2_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(Int32.Parse(s));
                }
            }
            file.Close();

            List<int> program = new List<int>(originalProgram);
            // 1202 Protocol 
            // - replace program[1] with 12
            // - replace program[2] with 2
            program[1] = 12;
            program[2] = 2;

            // Process program
            result = ExecuteProgram(program);
            Console.WriteLine("Part 1 Result: " + result);

            // Part 2
            // Is there a more efficient way to do this? I feel like that'd require math though :(
            ResetMemory(originalProgram, program);
            int desiredResult = 19690720;
            for(int noun=0; noun < 100; noun++)
            {
                for(int verb=0; verb < 100; verb++)
                {
                    ResetMemory(originalProgram, program);
                    program[1] = noun;
                    program[2] = verb;
                    if (desiredResult == ExecuteProgram(program))
                    {
                        int t = (100 * noun) + verb;
                        Console.WriteLine("Part 2 Result: " + t);
                        return;
                    }
                }
            }
        }

        public static void ResetMemory(List<int> original, List<int> target)
        {
            for (int i = 0; i < original.Count; i++)
            {
                target[i] = original[i];
            }
        }

        public static void ResetMemory(List<string> original, List<string> target)
        {
            for (int i = 0; i < original.Count; i++)
            {
                target[i] = original[i];
            }
        }

        public static int ExecuteProgram(List<int> program)
        {
            for (int pc = 0; program[pc] != 99 && pc < program.Count; pc += 4)
            {
                int cmd = program[pc];
                if (1 == cmd)
                {
                    program[program[pc + 3]] = program[program[pc + 1]] + program[program[pc + 2]];
                }
                else if (2 == cmd)
                {
                    program[program[pc + 3]] = program[program[pc + 1]] * program[program[pc + 2]];
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }
            return program[0];
        }
        #endregion

        #region Day3
        public class Point
        {
            public int x, y;

            public Point() { }

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point(Point o)
            {
                x = o.x;
                y = o.y;
            }

            public static bool operator ==(Point a, Point b)
            {
                return a.x == b.x && a.y == b.y;
            }

            public static bool operator !=(Point a, Point b)
            {
                return !(a == b);
            }

            public override bool Equals(Object b)
            {
                if(!this.GetType().Equals(b.GetType()))
                {
                    Console.WriteLine("Type mismatch.");
                    return false;
                }
                return this == (Point)b;
            }

            public override string ToString()
            {
                return "(" + x + ", " + y + ")";
            }

            public static float Slope(Point a, Point b)
            {
                return (0.0f + b.y - a.y) / (0.0f + b.x - a.x);
            }

            public static Point RationalSlope(Point a, Point b)
            {
                return new Point
                {
                    x = b.y - a.y, // Numerator
                    y = b.x - a.x // Denominator
                };
            }

            // This might not be right after I was guessing for a while on a previous problem
            public static bool CompareRationalSlopes(Point a, Point b)
            {
                if (a.y == 0 && b.y == 0)
                {
                    if ((a.x > 0 && b.x > 0)
                        || (a.x < 0 && b.x < 0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (a.y == b.y)
                {
                    if(a.x == b.x)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }

                if (a.x == 0 && b.x == 0)
                {
                    if ((a.y > 0 && b.y > 0)
                        || (a.y < 0 && b.y < 0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (a.x == b.x)
                {
                    if (a.y == b.y)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return (a.x * b.y) == (b.x * a.y);
            }

            public static float GetAngle(Point a, Point b)
            {
                return MathF.Atan2(b.x - a.x, b.y - a.y);
            }
        }

        // Part 1
        // Answer: 342, too high
        // Answer: 303, correct
        // Part 2
        public static void ExecuteDay3()
        {
            // The list of wires each containing a list of it's directions
            List<List<string>> wiresDirLists = new List<List<string>>();

            // Read file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_3_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] dirs = line.Split(',');
                wiresDirLists.Add(new List<string>(dirs));
            }
            file.Close();

            //Expected: 6
            //wiresDirLists.Add(new List<string>() { "R8", "U5", "L5", "D3" });
            //wiresDirLists.Add(new List<string>() { "U7", "R6", "D4", "L4" });

            // Expected: 159
            //wiresDirLists.Add(new List<string>() { "R75", "D30", "R83", "U83", "L12", "D49", "R71", "U7", "L72" });
            //wiresDirLists.Add(new List<string>() { "U62", "R66", "U55", "R34", "D71", "R55", "D58", "R83" });

            // Expected: 135
            //wiresDirLists.Add(new List<string>() { "R98", "U47", "R26", "D63", "R33", "U87", "L62", "D20", "R33", "U53", "R51" });
            //wiresDirLists.Add(new List<string>() { "U98", "R91", "D20", "R16", "D67", "R40", "U7", "R15", "U6", "R7" });

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Store line segment endpoints
            Point prevPoint = new Point(0, 0);
            List<List<Point>> wiresPointsLists = new List<List<Point>>();
            foreach (var dirList in wiresDirLists)
            {
                List<Point> pointList = new List<Point>();
                pointList.Add(new Point(0,0));

                foreach (string dir in dirList)
                {

                    // Fill pointList
                    prevPoint = FillPoints(pointList, prevPoint, dir);
                }

                wiresPointsLists.Add(pointList);
                prevPoint = new Point(0, 0);
            }

            // Find points of collision
            Point origin = new Point(0, 0);
            List<List<Point>> collisionsLists = new List<List<Point>>();
            for(int i = 1; i < wiresPointsLists.Count; i++)
            {
                List<Point> collisions = new List<Point>();
                for (int j = 0; j < wiresPointsLists[0].Count - 1; j++)
                {
                    for (int k = 0; k < wiresPointsLists[i].Count - 1; k++)
                    {
                        Point p = FindCollision(wiresPointsLists[0][j], wiresPointsLists[0][j + 1],
                            wiresPointsLists[i][k], wiresPointsLists[i][k + 1]);
                        if (origin != p)
                        {
                            collisions.Add(p);
                        }
                    }
                }
                collisionsLists.Add(collisions);
            }
            
            /*
            foreach (var l in collisionsLists)
            {
                string s = "";
                foreach(var p in l)
                {
                    s += p.x + ", " + p.y + " --- ";
                }
                Console.WriteLine("Collisions: " + s);
            }
            */

            // Calculate Manhattan Distance on each collision to find closest
            int distance = int.MaxValue;
            foreach(var l in collisionsLists)
            {
                foreach(var p in l)
                {
                    int t = Math.Abs(p.x) + Math.Abs(p.y);
                    if (t < distance)
                    {
                        distance = t;
                    }
                }
            }
            watch.Stop();
            Console.WriteLine($" - Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Distance: " + distance);
        }

        public static Point FillPoints(List<Point> points, Point start, string dir)
        {
            string d = dir.Substring(0, 1);
            int c = Int32.Parse(dir.Substring(1));
            Point lastPoint = new Point();

            switch (d)
                {
                    case "R":
                        lastPoint = new Point(start.x + c, start.y);
                        points.Add(lastPoint);
                        break;
                    case "L":
                        lastPoint = new Point(start.x - c, start.y);
                        points.Add(lastPoint);
                        break;
                    case "U":
                        lastPoint = new Point(start.x, start.y + c);
                        points.Add(lastPoint);
                        break;
                    case "D":
                        lastPoint = new Point(start.x, start.y - c);
                        points.Add(lastPoint);
                        break;
                    default:
                        Console.WriteLine("Invalid Direction: " + d);
                        break;
                }
            return lastPoint;
        }

        public static Point FindCollision(Point a1, Point a2, Point b1, Point b2)
        {
            Point collision = new Point();
            if (a1.x == a2.x && b1.y == b2.y) // Line 1 is vertical
            {
                if (IsIntersecting(a1, a2, b1, b2) 
                    && (b1.x >= a1.x && b2.x <= a1.x
                    || b1.x <= a1.x && b2.x >= a1.x))
                {
                    collision = new Point(a1.x, b1.y);
                }
            } else if(b1.x == b2.x && a1.y == a2.y) // Line 2 is vertical
            {
                if(IsIntersecting(b1, b2, a1, a2)
                    && (a1.x >= b1.x && a2.x <= b1.x
                    || a1.x <= b1.x && a2.x >= b1.x))
                {
                    collision = new Point(b1.x, a1.y);
                }
            }
            return collision;
        }

        public static bool IsIntersecting(Point vert1, Point vert2, Point horiz1, Point horiz2)
        {
            if (horiz1.x < vert1.x && horiz2.x < vert1.x
                || horiz1.x > vert1.x && horiz2.x > vert1.x)
            {
                return false;
            } else if (vert1.y < horiz1.y && vert2.y < horiz1.y
                || vert1.y > horiz1.y && vert2.y > horiz1.y)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Day4
        // Answers:
        // 43596, too high
        // 1763, correct
        // Part 2:
        // 1380, too high
        // 1248, too high
        // 1196, correct
        public static void ExecuteDay4()
        {
            int min = 152085, max = 670283;
            List<int> possiblePasswordList = new List<int>();

            string curStr = "";
            for(int i = min+1; i<max; i++)
            {
                curStr = i.ToString();
                if(MatchesRules(curStr))
                {
                    possiblePasswordList.Add(i);
                }
            }

            Console.WriteLine("Possibles: " + possiblePasswordList.Count);
        }

        public static bool MatchesRules(string str)
        {
            bool isDoubleMet = false;
            char doubleChar = '.';
            bool inRun = false;
            char prevChar = str[0];
            for(int i = 1; i < str.Length; i++)
            {
                if(!isDoubleMet && str[i] == prevChar && !inRun)
                {
                    isDoubleMet = true;
                    doubleChar = str[i];
                    inRun = true;
                } else if(isDoubleMet && inRun && str[i] == doubleChar)
                {
                    isDoubleMet = false;
                } else if(inRun && str[i] != doubleChar)
                {
                    inRun = false;
                }

                if(prevChar > str[i]) // Not int comparison, but should be the same even with ASCII vals
                {
                    return false;
                }
                prevChar = str[i];
            }

            return isDoubleMet;
        }
        #endregion

        #region Day5
        // Answer:
        // 6069343, correct
        // Part 2: 
        // 11537607, too high
        // 3188550, correct
        public static void ExecuteDay5()
        {
            int result = -1;
            //{ "1002", "6", "3", "6", "0004", "6", "33" }
            //{"3","21","1008","21","8","20","1005","20","22","107","8","21","20","1006","20","31","1106","0","36","98","0","0","1002","21","125","20","4","20","1105","1","46","104","999","1105","1","46","1101","1000","1","20","4","20","1105","1","46","98","99"}
            List<string> originalProgram = new List<string>();

            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_5_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(s);
                }
            }
            file.Close();

            List<string> program = new List<string>(originalProgram);

            //int input = 1; // Part 1 input
            int input = 5; // Part 2 input

            // Process program
            result = ExecuteProgram2(program, input, new List<int>());
        }

        public static int ExecuteProgram2(List<string> program, int input, List<int> output)
        {
            
            int pc = 0;
            while (program[pc] != "99" && pc < program.Count)
            {
                string cmd = program[pc];
                int opcode = -1;
                bool[] isImmediate = new bool[3] { false, false, false};

                if(cmd.Length>2)
                {
                    string s = cmd.Substring(cmd.Length-2);
                    string s2 = cmd.Substring(0, cmd.Length-2);
                    opcode = Int32.Parse(s);
                    if(99 == opcode)
                    {
                        return -1;
                    }
                    for (int i = 0; s2.Length-i > 0; i++)
                    {
                        isImmediate[isImmediate.Length - 1 - i] = s2[s2.Length - 1 - i] == '1';
                    }

                } else
                {
                    opcode = Int32.Parse(cmd);
                }
                
                if (1 == opcode) // 1, add two params, store in third param
                {
                    program[Int32.Parse(program[pc + 3])] 
                        = (Int32.Parse(program[isImmediate[2] ? pc+1 : Int32.Parse(program[pc + 1])]) 
                        + Int32.Parse(program[isImmediate[1] ? pc+2 : Int32.Parse(program[pc + 2])])).ToString();

                    pc += 4;
                }
                else if (2 == opcode) // 2, add two params, store in third param
                {
                    program[Int32.Parse(program[pc + 3])] 
                        = (Int32.Parse(program[isImmediate[2] ? pc+1 : Int32.Parse(program[pc + 1])]) 
                        * Int32.Parse(program[isImmediate[1] ? pc+2 : Int32.Parse(program[pc + 2])])).ToString();

                    pc += 4;
                }
                else if (3 == opcode) // 3, store 'input' in position given by param
                {
                    program[Int32.Parse(program[pc + 1])] = input.ToString();

                    pc += 2; 
                }
                else if (4 == opcode) // 4, output value stored in position given by param
                {
                    //Console.WriteLine(Int32.Parse(program[isImmediate[2] ? pc+1 : Int32.Parse(program[pc + 1])]));
                    output.Add(Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])]));
                    pc += 2; 
                }
                else if (5 == opcode) // 5, if first param is nonzero, set the instruction pointer to the val from second param
                {
                    if (program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1]) ] != "0")
                    {
                        pc = Int32.Parse(program[isImmediate[1] ? pc+2 : Int32.Parse(program[pc+2])]);
                    } else
                    {
                        pc += 3;
                    }
                }
                else if (6 == opcode) // 6, if first param is zero, set the instruction pointer to the val from second param
                {
                    if (program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])] == "0")
                    {
                        pc = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (7 == opcode) // 7, if first param is < second param, set third param to 1, else 0
                {
                    int a = Int32.Parse(program[isImmediate[2] ? pc+1 : Int32.Parse(program[pc+1])]);
                    int b = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    if(a < b)
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "1";
                    } else
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "0";
                    }
                    pc += 4;
                }
                else if (8 == opcode) // 8, if first param == second param, set third param to 1, else 0
                {
                    int a = Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])]);
                    int b = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    if (a == b)
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "1";
                    }
                    else
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "0";
                    }
                    pc += 4;
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }
            return Int32.Parse(program[0]);
        }
        #endregion

        #region Day6
        public class Orbit
        {
            public string target;
            public string satellite;

            public Orbit(string target, string satellite)
            {
                this.target = target;
                this.satellite = satellite;
            }
        }
        public static void ExecuteDay6()
        {
            List<Orbit> orbitList = new List<Orbit>();
            int totalOrbitCount = 0;
            //----------------------------
            //Read list from file
            //System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_6_1_input.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/message.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] orbit = line.Split(')');
                orbitList.Add(new Orbit(orbit[0], orbit[1]));
            }
            file.Close();
            //----------------------------


            foreach(Orbit o in orbitList)
            {
                totalOrbitCount += GetOrbitTotal(orbitList, o);
            }
            Console.WriteLine("Orbit count: " + totalOrbitCount);
        }

        public static int GetOrbitTotal(List<Orbit> orbitList, Orbit o)
        { 
            if(o.target == "COM")
            {
                return 1;
            }
            else
            {
                return 1 + GetOrbitTotal(orbitList, GetOrbit(orbitList, o.target));
            }
        }

        public static Orbit GetOrbit(List<Orbit> orbitList, string satellite)
        {
            foreach(Orbit o in orbitList)
            {
                if(o.satellite == satellite)
                {
                    return o;
                }
            }
            return null;
        }
        #endregion

        #region Day7
        // Answers:
        // 225, too low
        // 425314, too high
        // 272368, correct
        public static void ExecuteDay7()
        {
            List<string> originalProgram = new List<string>();

            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_7_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(s);
                }
            }
            file.Close();

            List<string> program = new List<string>(originalProgram);

            int[] initPhaseInputs = new int[] { 0, 1, 2, 3, 4};
            //int[] initPhaseInputs = new int[] { 5, 6, 7, 8, 9 };
            List<int[]> perms = new List<int[]>();
            permute(initPhaseInputs, 0, initPhaseInputs.Length-1, perms);

            int max = 0;
            foreach(int[] phaseInputs in perms)
            {
                List<int> outputs = new List<int>() { 0 };
                foreach (int phaseInput in phaseInputs)
                {
                    List<int> inputs = new List<int>();
                    inputs.Add(phaseInput);
                    inputs.Add(outputs[outputs.Count - 1]);
                    ExecuteProgram3(program, inputs, outputs);
                    ResetMemory(originalProgram, program);
                    inputs.Clear();
                }

                if(outputs[outputs.Count-1] > max)
                {
                    max = outputs[outputs.Count - 1];
                }
            }
            Console.WriteLine("Max: " + max);
        }

        private static void permute(int[] arr, int l, int r, List<int[]> perms)
        {
            if (l == r)
            {
                perms.Add((int[])arr.Clone());
            }else
            {
                for (int i = l; i <= r; i++)
                {
                    arr = swap(arr, l, i);
                    permute(arr, l + 1, r, perms);
                    arr = swap(arr, l, i);
                }
            }
        }

        public static int[] swap(int[] arr, int i, int j)
        {
            int temp;
            temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
            return arr;
        }

        public static int ExecuteProgram3(List<string> program, List<int> input, List<int> output)
        {

            int pc = 0;
            while (program[pc] != "99" && pc < program.Count)
            {
                string cmd = program[pc];
                int opcode = -1;
                bool[] isImmediate = new bool[3] { false, false, false };

                if (cmd.Length > 2)
                {
                    string s = cmd.Substring(cmd.Length - 2);
                    string s2 = cmd.Substring(0, cmd.Length - 2);
                    opcode = Int32.Parse(s);
                    if (99 == opcode)
                    {
                        return -1;
                    }
                    for (int i = 0; s2.Length - i > 0; i++)
                    {
                        isImmediate[isImmediate.Length - 1 - i] = s2[s2.Length - 1 - i] == '1';
                    }

                }
                else
                {
                    opcode = Int32.Parse(cmd);
                }

                if (1 == opcode) // 1, add two params, store in third param
                {
                    program[Int32.Parse(program[pc + 3])]
                        = (Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])])
                        + Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])])).ToString();

                    pc += 4;
                }
                else if (2 == opcode) // 2, add two params, store in third param
                {
                    program[Int32.Parse(program[pc + 3])]
                        = (Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])])
                        * Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])])).ToString();

                    pc += 4;
                }
                else if (3 == opcode) // 3, store 'input' in position given by param
                {
                    program[Int32.Parse(program[pc + 1])] = input[0].ToString();
                    input.RemoveAt(0);

                    pc += 2;
                }
                else if (4 == opcode) // 4, output value stored in position given by param
                {
                    //Console.WriteLine(Int32.Parse(program[isImmediate[2] ? pc+1 : Int32.Parse(program[pc + 1])]));
                    output.Add(Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])]));
                    pc += 2;
                }
                else if (5 == opcode) // 5, if first param is nonzero, set the instruction pointer to the val from second param
                {
                    if (program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])] != "0")
                    {
                        pc = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (6 == opcode) // 6, if first param is zero, set the instruction pointer to the val from second param
                {
                    if (program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])] == "0")
                    {
                        pc = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (7 == opcode) // 7, if first param is < second param, set third param to 1, else 0
                {
                    int a = Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])]);
                    int b = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    if (a < b)
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "1";
                    }
                    else
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "0";
                    }
                    pc += 4;
                }
                else if (8 == opcode) // 8, if first param == second param, set third param to 1, else 0
                {
                    int a = Int32.Parse(program[isImmediate[2] ? pc + 1 : Int32.Parse(program[pc + 1])]);
                    int b = Int32.Parse(program[isImmediate[1] ? pc + 2 : Int32.Parse(program[pc + 2])]);
                    if (a == b)
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "1";
                    }
                    else
                    {
                        program[isImmediate[0] ? pc + 3 : Int32.Parse(program[pc + 3])] = "0";
                    }
                    pc += 4;
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }
            return Int32.Parse(program[0]);
        }
        #endregion

        #region Day8
        // Answers:
        // 84, wrong
        // 1935, correct
        public static void ExecuteDay8()
        {
            //List<string> originalImage = new List<string>();
            int imageWidth = 25, imageHeight = 6;
            string originalImage = "";

            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_8_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                originalImage += line;
            }
            file.Close();

            // Build list of layers
            List<List<int>> layerList = new List<List<int>>();
            for(int layerIndex = 0; layerIndex < originalImage.Length / (imageWidth * imageHeight); layerIndex++)
            {
                List<int> curLayer = new List<int>();
                for (int i = imageWidth * imageHeight * layerIndex; i < (imageWidth * imageHeight * layerIndex) + (imageWidth * imageHeight); i++)
                {
                    curLayer.Add(originalImage[i] - '0');
                }
                layerList.Add(curLayer);
            }

            // Find layer with least zeros
            int minZeros = Int32.MaxValue;
            int minZerosIndex = -1;
            for (int j = 0; j < layerList.Count; j++)
            {
                List<int> l = layerList[j];
                int curZeros = 0;
                foreach (int i in l)
                {
                    if(i==0)
                    {
                        curZeros++;
                    }
                }

                if(curZeros < minZeros)
                {
                    minZerosIndex = j;
                    minZeros = curZeros;
                }
            }

            // Count num of ones and num of twos
            int ones = 0;
            int twos = 0;
            foreach (int i in layerList[minZerosIndex])
            {
                if(i == 1)
                {
                    ones++;
                } else if(i == 2)
                {
                    twos++;
                }
            }

            //Return product of those
            Console.WriteLine(ones*twos);

            // Part 2
            // Loop through each layer to determine the final image
            // 2 - transparent
            // 0 - black
            // 1 - white

            List<int> finalImage = new List<int>();
            for(int i = 0; i < imageWidth*imageHeight; i++)
            {
                finalImage.Add(2);
            }

            foreach (List<int> l in layerList)
            { 
                for(int i = 0; i < imageWidth*imageHeight; i++)
                {
                    if(finalImage[i] == 2 && l[i] != 2)
                    {
                        finalImage[i] = l[i];
                    }
                }
            }

            for(int r = 0; r < imageHeight; r++)
            {
                string row = "";
                for(int c = r * imageWidth; c < (r * imageWidth) + imageWidth; c++)
                {
                    row += finalImage[c];
                }
                Console.WriteLine(row);
            }
        }
        #endregion

        #region Day9
        // Answers:
        // 72, incorrect
        // 2399197539, correct
        // Part 2:
        // 35106, correct
        public enum ParamMode
        { 
            POSITION,
            IMMEDIATE,
            RELATIVE
        }

        public static void ExecuteDay9()
        {
            // { "104", "1125899906842624", "99" }
            // {"1102","34915192","34915192","7","4","7","99","0"}
            // {"109","1","204","-1","1001","100","1","100","1008","100","16","101","1006","101","0","99"}
            List<string> originalProgram = new List<string>();
            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_9_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(s);
                }
            }
            file.Close();

            List<string> program = new List<string>(originalProgram);
            // Initializing 10x the amount of memory the program takes up
            for(int i = 0; i < originalProgram.Count * 10; i++)
            {
                program.Add("0");
            }

            //List<BigInteger> inputs = new List<BigInteger>() { 1 }; // Input for Part 1
            List<BigInteger> inputs = new List<BigInteger>() { 2 };
            List<BigInteger> outputs = new List<BigInteger>();

            ExecuteProgram4(program, inputs, outputs);
            foreach(BigInteger i in outputs)
            {
                Console.WriteLine(i);
            }
        }

        public static int ExecuteProgram4(List<string> program, List<BigInteger> input, List<BigInteger> output)
        {
            int relBase = 0;
            int pc = 0;
            while (program[pc] != "99" && pc < program.Count)
            {
                string cmd = program[pc];
                int opcode = -1;
                ParamMode[] paramModes = new ParamMode[3] { ParamMode.POSITION, ParamMode.POSITION, ParamMode.POSITION };

                if (cmd.Length > 2)
                {
                    string s = cmd.Substring(cmd.Length - 2);
                    string s2 = cmd.Substring(0, cmd.Length - 2);
                    opcode = Int32.Parse(s);
                    if (99 == opcode)
                    {
                        return -1;
                    }
                    for (int i = 0; s2.Length - i > 0; i++)
                    {
                        if (s2[s2.Length - 1 - i] == '1')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.IMMEDIATE;
                        } else if (s2[s2.Length - 1 - i] == '2')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.RELATIVE;
                        }
                         
                    }

                }
                else
                {
                    opcode = Int32.Parse(cmd);
                }

                if (1 == opcode) // 1, add two params, store in third param
                {
                    program[GetResolvedLocation(pc+3, relBase, paramModes[0], program)] 
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        + BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();
                    
                    pc += 4;
                }
                else if (2 == opcode) // 2, add two params, store in third param
                {
                    program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)]
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        * BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();

                    pc += 4;
                }
                else if (3 == opcode) // 3, store 'input' in position given by param
                {
                    program[GetResolvedLocation(pc+1, relBase, paramModes[2], program)] = input[0].ToString();
                    //input.RemoveAt(0); // Input was not eaten for this problem

                    pc += 2;
                }
                else if (4 == opcode) // 4, output value stored in position given by param
                {
                    output.Add(BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]));
                    
                    pc += 2;
                }
                else if (5 == opcode) // 5, if first param is nonzero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] != "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (6 == opcode) // 6, if first param is zero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (7 == opcode) // 7, if first param is < second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a < b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }
                    
                    pc += 4;
                }
                else if (8 == opcode) // 8, if first param == second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a == b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }
                    
                    pc += 4;
                }
                else if (9 == opcode) // 9, adjust relBase by the first param
                {
                    relBase += Int32.Parse(program[GetResolvedLocation(pc+1, relBase, paramModes[2], program)]);

                    pc += 2;
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }
            return Int32.Parse(program[0]);
        }

        public static int GetResolvedLocation(int loc, int relBase, ParamMode paramMode, List<string> program)
        {
            switch(paramMode)
            {
                case ParamMode.IMMEDIATE:
                    return loc;
                case ParamMode.POSITION:
                    return Int32.Parse(program[loc]);
                case ParamMode.RELATIVE:
                    return relBase + Int32.Parse(program[loc]);
                default:
                    return -1;
            }
        }
        #endregion

        #region Day10
        // Answers:
        // 302, too low
        // 303, correct
        public static void ExecuteDay10()
        {
            // The list of wires each containing a list of it's directions
            List<Point> asteroidLocList = new List<Point>();

            // Read file
            //System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/test.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_10_1_input.txt");
            int row = 0;
            string line;
            while ((line = file.ReadLine()) != null)
            {
                for(int col = 0; col < line.Length; col++)
                {
                    if('#' == line[col])
                    {
                        asteroidLocList.Add(new Point(col, row));
                    }
                }
                row++;
            }
            file.Close();

            int maxVisibleAsteroids = 0;
            Point maxVisiblePoint = null;
            foreach(Point p in asteroidLocList)
            {
                List<float> foundAngles = new List<float>();
                int curVisibleAsteroids = 0;
                foreach(Point o in asteroidLocList)
                {
                    if (p != o) 
                    {
                        float angle = Point.GetAngle(p, o);
                        if(!foundAngles.Contains(angle))
                        {
                            foundAngles.Add(angle);
                            curVisibleAsteroids++;
                        }
                    }
                }

                if(curVisibleAsteroids > maxVisibleAsteroids)
                {
                    maxVisibleAsteroids = curVisibleAsteroids;
                    maxVisiblePoint = p;
                }
            }

            Console.WriteLine(maxVisibleAsteroids);
            Console.WriteLine(maxVisiblePoint);
        }
        #endregion

        #region Day11
        // Answers:
        // 303, too low (was running day 10)
        // 9600, too high
        // 2469, correct
        // Part 2:
        //
        public static void ExecuteDay11()
        {
            

            // Read in Intcode program from file
            List<string> originalProgram = new List<string>();
            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_11_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(s);
                }
            }
            file.Close();

            List<string> program = new List<string>(originalProgram);
            for (int i = 0; i < originalProgram.Count * 10; i++)
            {
                program.Add("0");
            }
            // Execute program
            ExecuteProgram5(program);
            // Compare current position to whiteTiles to determine if input should be 0 or 1
            // Add traversed tiles to paintedTiles to determine any unique painted tile
            // Use output to paint tile
            // Use second output to increment curDir on 1, decrement on 0

        }

        public static int ExecuteProgram5(List<string> program)
        {
            // Day 11 vars
            List<Point> whiteTiles = new List<Point>() { new Point(0, 0) };
            List<Point> paintedTiles = new List<Point>();
            Point curLoc = new Point(0, 0);

            List<Point> dirList = new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1),
                new Point(-1, 0)
            };
            int curDir = 0;

            bool isTurnOutput = false;
            //

            int relBase = 0;
            int pc = 0;
            while (program[pc] != "99" && pc < program.Count)
            {
                string cmd = program[pc];
                int opcode = -1;
                ParamMode[] paramModes = new ParamMode[3] { ParamMode.POSITION, ParamMode.POSITION, ParamMode.POSITION };

                if (cmd.Length > 2)
                {
                    string s = cmd.Substring(cmd.Length - 2);
                    string s2 = cmd.Substring(0, cmd.Length - 2);
                    opcode = Int32.Parse(s);
                    if (99 == opcode)
                    {
                        return -1;
                    }
                    for (int i = 0; s2.Length - i > 0; i++)
                    {
                        if (s2[s2.Length - 1 - i] == '1')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.IMMEDIATE;
                        }
                        else if (s2[s2.Length - 1 - i] == '2')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.RELATIVE;
                        }

                    }

                }
                else
                {
                    opcode = Int32.Parse(cmd);
                }

                if (1 == opcode) // 1, add two params, store in third param
                {
                    program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)]
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        + BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();

                    pc += 4;
                }
                else if (2 == opcode) // 2, add two params, store in third param
                {
                    program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)]
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        * BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();

                    pc += 4;
                }
                else if (3 == opcode) // 3, store 'input' in position given by param
                {
                    if(whiteTiles.Contains(curLoc))
                    {
                        program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] = "1";
                    } else
                    {
                        program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] = "0";
                    }

                    pc += 2;
                }
                else if (4 == opcode) // 4, output value stored in position given by param
                {
                    if(isTurnOutput)
                    {
                        if(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "0")
                        {
                            curDir = TurnLeft(curDir, dirList.Count);
                        } else if(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "1")
                        {
                            curDir = TurnRight(curDir, dirList.Count);
                        } else
                        {
                            Console.WriteLine("Turn???");
                        }

                        curLoc.x += dirList[curDir].x;
                        curLoc.y += dirList[curDir].y;
                        isTurnOutput = false;
                    } else
                    {
                        if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "0")
                        {
                            if(whiteTiles.Contains(curLoc))
                            {
                                whiteTiles.RemoveAt(whiteTiles.IndexOf(curLoc));
                            }
                        }
                        else if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "1")
                        {
                            whiteTiles.Add(new Point(curLoc));
                        }
                        else
                        {
                            Console.WriteLine("Paint???");
                        }

                        if(!paintedTiles.Contains(curLoc))
                        {
                            paintedTiles.Add(new Point(curLoc));
                        }
                        
                        isTurnOutput = true;
                    }

                    pc += 2;
                }
                else if (5 == opcode) // 5, if first param is nonzero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] != "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (6 == opcode) // 6, if first param is zero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (7 == opcode) // 7, if first param is < second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a < b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }

                    pc += 4;
                }
                else if (8 == opcode) // 8, if first param == second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a == b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }

                    pc += 4;
                }
                else if (9 == opcode) // 9, adjust relBase by the first param
                {
                    relBase += Int32.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);

                    pc += 2;
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }

            //Console.WriteLine("Painted tiles: " + paintedTiles.Count);
            // Part 2 - Figure out how to plot all the white tiles...
            foreach(Point p in whiteTiles)
            {
                Console.WriteLine(p);
            }
            return Int32.Parse(program[0]);
        }

        /*
         List<Point> dirList = new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1),
                new Point(-1, 0)
            };
        */
        public static int TurnRight(int cur, int length)
        {
            cur++;
            if(cur >= length)
            {
                cur = 0;
            }
            return cur;
        }

        public static int TurnLeft(int cur, int length)
        {
            cur--;
            if (cur < 0)
            {
                cur = length-1;
            }
            return cur;
        }
        #endregion

        #region Day12
        public class Vector3
        {
            public int x, y, z;

            public Vector3()
            {
                x = 0;
                y = 0;
                z = 0;
            }

            public Vector3(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Vector3(Vector3 v)
            {
                this.x = v.x;
                this.y = v.y;
                this.z = v.z;
            }

            public Vector3 add(Vector3 o)
            {
                return new Vector3(x + o.x, y + o.y, z + o.z);
            }

            public Vector3 sub(Vector3 o)
            {
                return new Vector3(x - o.x, y - o.y, z - o.z);
            }

            // Overrides
            public static bool operator ==(Vector3 s, Vector3 o)
            {
                if(null == s || null == o)
                {
                    return false;
                }

                return s.x == o.x && s.y == o.y && s.z == o.z;
            }

            public static bool operator !=(Vector3 s, Vector3 o)
            {
                return !(s == o);
            }

            public override bool Equals(Object o)
            {
                return this == (Vector3)o;
            }
        }

        // Answers:
        // 5350, correct
        // Part 2:
        //
        public static void ExecuteDay12()
        {
            int timeSteps = 1000;
            List<Vector3> locs = new List<Vector3>();
            List<Vector3> vels = new List<Vector3>();

            // Read file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_12_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] vec = line.Split(',');
                locs.Add(new Vector3(Int32.Parse(vec[0]), Int32.Parse(vec[1]), Int32.Parse(vec[2])));
                vels.Add(new Vector3(0, 0, 0));
            }
            file.Close();

            // Iterate for every time step
            for (int i = 0; i < timeSteps; i++)
            {
                // Simulate gravity for time step
                for(int l = 0; l < locs.Count; l++)
                {
                    for(int o = 0; o < locs.Count; o++)
                    {
                        if(l != o)
                        {
                            if(locs[l].x > locs[o].x)
                            {
                                vels[l].x--;
                            } else if(locs[l].x < locs[o].x)
                            {
                                vels[l].x++;
                            }

                            if (locs[l].y > locs[o].y)
                            {
                                vels[l].y--;
                            }
                            else if (locs[l].y < locs[o].y)
                            {
                                vels[l].y++;
                            }

                            if (locs[l].z > locs[o].z)
                            {
                                vels[l].z--;
                            }
                            else if (locs[l].z < locs[o].z)
                            {
                                vels[l].z++;
                            }
                        }
                    }
                }
                // Simulate velocity action for time step
                for(int v = 0; v < vels.Count; v++)
                {
                    locs[v] = locs[v].add(vels[v]);
                }
            }

            // Return total energy in system after 1000 time steps 
            // total energy = potential * kinetic
            int totalSystemEnergy = 0;
            for(int i = 0; i < locs.Count; i++)
            {
                totalSystemEnergy +=
                    ((Math.Abs(locs[i].x) + Math.Abs(locs[i].y) + Math.Abs(locs[i].z)) * (Math.Abs(vels[i].x) + Math.Abs(vels[i].y) + Math.Abs(vels[i].z)));
            }
            Console.WriteLine("Total system energy: " + totalSystemEnergy);
        }
        #endregion

        #region Day13
        // Answers:
        // 341, correct
        // Part 2:
        //
        public static void ExecuteDay13()
        {
            // Read in Intcode program from file
            List<string> originalProgram = new List<string>();
            //Read input from file
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_13_1_input.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] opcodes = line.Split(',');
                foreach (string s in opcodes)
                {
                    originalProgram.Add(s);
                }
            }
            file.Close();

            List<string> program = new List<string>(originalProgram);
            for (int i = 0; i < originalProgram.Count * 10; i++)
            {
                program.Add("0");
            }
            // Execute program
            List<int> outputs = new List<int>();
            int totalBlocks = 0;
            ExecuteProgram6(program, outputs);

            for(int i = 0; i < outputs.Count; i+=3)
            {
                if(outputs[i+2] == 2)
                {
                    totalBlocks++;
                }
            }
            Console.WriteLine("Total block tiles: " + totalBlocks);
        }

        public static int ExecuteProgram6(List<string> program, List<int> outputs)
        {
            int relBase = 0;
            int pc = 0;
            while (program[pc] != "99" && pc < program.Count)
            {
                string cmd = program[pc];
                int opcode = -1;
                ParamMode[] paramModes = new ParamMode[3] { ParamMode.POSITION, ParamMode.POSITION, ParamMode.POSITION };

                if (cmd.Length > 2)
                {
                    string s = cmd.Substring(cmd.Length - 2);
                    string s2 = cmd.Substring(0, cmd.Length - 2);
                    opcode = Int32.Parse(s);
                    if (99 == opcode)
                    {
                        return -1;
                    }
                    for (int i = 0; s2.Length - i > 0; i++)
                    {
                        if (s2[s2.Length - 1 - i] == '1')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.IMMEDIATE;
                        }
                        else if (s2[s2.Length - 1 - i] == '2')
                        {
                            paramModes[paramModes.Length - 1 - i] = ParamMode.RELATIVE;
                        }

                    }

                }
                else
                {
                    opcode = Int32.Parse(cmd);
                }

                if (1 == opcode) // 1, add two params, store in third param
                {
                    program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)]
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        + BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();

                    pc += 4;
                }
                else if (2 == opcode) // 2, add two params, store in third param
                {
                    program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)]
                        = (BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)])
                        * BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)])).ToString();

                    pc += 4;
                }
                else if (3 == opcode) // 3, store 'input' in position given by param
                {

                    pc += 2;
                }
                else if (4 == opcode) // 4, output value stored in position given by param
                {
                    outputs.Add(Int32.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]));

                    pc += 2;
                }
                else if (5 == opcode) // 5, if first param is nonzero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] != "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (6 == opcode) // 6, if first param is zero, set the instruction pointer to the val from second param
                {
                    if (program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)] == "0")
                    {
                        pc = Int32.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    }
                    else
                    {
                        pc += 3;
                    }
                }
                else if (7 == opcode) // 7, if first param is < second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a < b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }

                    pc += 4;
                }
                else if (8 == opcode) // 8, if first param == second param, set third param to 1, else 0
                {
                    BigInteger a = BigInteger.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);
                    BigInteger b = BigInteger.Parse(program[GetResolvedLocation(pc + 2, relBase, paramModes[1], program)]);
                    if (a == b)
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "1";
                    }
                    else
                    {
                        program[GetResolvedLocation(pc + 3, relBase, paramModes[0], program)] = "0";
                    }

                    pc += 4;
                }
                else if (9 == opcode) // 9, adjust relBase by the first param
                {
                    relBase += Int32.Parse(program[GetResolvedLocation(pc + 1, relBase, paramModes[2], program)]);

                    pc += 2;
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmd);
                    //break; // For some reason, processing continues even after unrecognized commands.
                }
            }

            return Int32.Parse(program[0]);
        }
        #endregion

        #region Day14
        public class Ingredient
        {
            public string _id;
            public int _qty;

            public Ingredient(string id, int qty)
            {
                this._id = id;
                this._qty = qty;
            }
        }

        public class Reaction
        {
            public List<Ingredient> reactants;
            public Ingredient solution;
            
            public Reaction(List<Ingredient> reactants, Ingredient solution)
            {
                this.reactants = reactants;
                this.solution = solution;
            }

            public override string ToString()
            {
                string s = "";
                foreach(var r in reactants)
                {
                    s += r._qty + " " + r._id + " ";
                }

                s += " => " + solution._id + " " + solution._qty;
                return s;
            }
        }
        // Answers:
        // 37328, too low
        // 438587, too low
        // 4482441, too high
        public static void ExecuteDay14()
        {
            // ORE is the raw materials
            // FUEL is the target, how much ORE is needed to produce one?
            // List is in the following format:
            // QTY ING, QTY ING2 => QTY ING3
            // There can be 1 to n input ingredients

            // Solution Attempt 1:
            // - Not sure why this was incorrect, yet
            // Find FUEL process
            // Recurse backwards to find required ORE amounts

            // Solution Attempt 2:
            // Recurse through and build a list of the requirements that lead to ORE, then just multiply by the amount needed for everything
       

            // Read file
            List<Reaction> reactions = new List<Reaction>();
            Dictionary<string, Reaction> reactionDict = new Dictionary<string, Reaction>();
            Dictionary<string, int> store = new Dictionary<string, int>();
            //Read input from file
            //System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/day_14_1_input.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/test.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] splitReaction = line.Split(" => ");

                var solutionStrArr = splitReaction[1].Split(' ');
                Ingredient solution = new Ingredient(solutionStrArr[1], Int32.Parse(solutionStrArr[0]));

                List<Ingredient> reactants = new List<Ingredient>();
                string[] reactantsStrArr = splitReaction[0].Split(",");
                foreach(var r in reactantsStrArr)
                {
                    var t = r.TrimStart().Split(' ');
                    reactants.Add(new Ingredient(t[1], Int32.Parse(t[0])));
                }
                reactions.Add(new Reaction(reactants, solution));
            }
            file.Close();

            foreach(var r in reactions)
            {
                reactionDict.Add(r.solution._id, r);
            }
            Console.WriteLine("Cost: " + GetOreCost(reactionDict.GetValueOrDefault("FUEL"), 1, reactionDict, store));
        }

        public static int GetOreCost(Reaction r, int amount, Dictionary<string, Reaction> reactionDict, Dictionary<string, int> store)
        {
            try
            {
                int pooledAmt = store[r.solution._id];
                if (pooledAmt <= amount)
                {
                    amount -= pooledAmt;
                    store[r.solution._id] = 0;
                }
                else
                {
                    store[r.solution._id] -= amount;
                    amount = 0;
                }

                if (amount == 0)
                {
                    return 0;
                }
            } catch (KeyNotFoundException)
            {
                // Just behave normally
            }

            int requiredRuns = (int)Math.Ceiling((float)amount / r.solution._qty);
            /*
            if (requiredRuns > 1)
            {
                // Add surplus to store
                if(store.ContainsKey(r.solution._id))
                {
                    store[r.solution._id] += (amount * requiredRuns) - amount;
                } else
                {
                    store.Add(r.solution._id, (amount * requiredRuns) - amount);
                }
            }
            */

            int cost;
            if (r.reactants.Count == 1 && r.reactants[0]._id == "ORE")
            {
                cost = (r.reactants[0]._qty * requiredRuns);
            } else
            {
                int c = 0;
                foreach(var reactant in r.reactants)
                {
                    Reaction reaction = reactionDict.GetValueOrDefault(reactant._id);
                    int t = GetOreCost(reaction, reactant._qty, reactionDict, store);
                    c += (t * (int)Math.Ceiling((float)amount / r.solution._qty));
                }
                cost =  c;
            }
            return cost;
        }

        #endregion

    }
}
