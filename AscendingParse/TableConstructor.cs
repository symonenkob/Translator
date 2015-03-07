using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    internal static class TableConstructor
    {
        public static string[,] Table { get; private set; }
        private static String[][] Grammar = new String[44][];

        public static string GetRelation(string lex1, string lex2)
        {
            string relation = "relation not found";
            for (int i = 0; i < Table.GetLength(0); i++)
            {
                if (Table[i, 0] == lex1)
                {
                    for (int j = 0; j < Table.GetLength(1); j++)
                    {
                        if (Table[0, j] == lex2 && Table[i, j] != null)
                        {
                            relation = Table[i, j];
                            break;
                        }
                    }
                    break;
                }
            }
            return relation;
        }

        static private void allGrTable()
        {
            String[] lex =
            {
                "<сп.об1>", "<сп.об>", "<сп.оп1>", "<сп.оп>", "<оп>", "<сп.ид1>",
                "<сп.ид>", "<врж1>", "<врж>", "<терм1>", "<терм>", "<множ1>", "<множ>", "<перв.в>",
                "<ЛВ1>", "<ЛВ>", "<ЛТ1>", "<ЛТ>", "<ЛМ>", "<знак>", "<оп1>", "<ЛМ1>",

                "pr", "id", "con", "{", "}", "int",
                "read", "write", "do", "to", "by", "while",
                "if", "then",

                ";", ",", "(", ")", "[", "]", "=", "+", "-", "*", "/", "^",
                "or", "and", "!", "<", "<=", "==", "!=", ">=", ">",

                "#"
            };

            Table = new string[lex.Count()+1, lex.Count()+1];

            for (int i = 1; i < lex.Length+1; i++)
            {
                Table[0, i] = lex[i-1];
                Table[i, 0] = lex[i-1];
            }

            for (int i = 1; i < lex.Length; i++)
            {
                Table[i, Table.GetLength(0) - 1] = ">";
                Table[Table.GetLength(0) - 1, i] = "<";
            }
        }

        private static Boolean isTerm(String a)
        {
            if (a.Contains('<') && a.Contains('>')) return false;
            return true;
        }

        private static void print(String s1, String s2, String z)
        {
            for (int i = 0; i < Table.GetLength(0); i++)
                if (Table[i, 0] == s1)
                {
                    for (int j = 0; j < Table.GetLength(0); j++)
                        if (Table[0, j] == s2 && i != 0 && j != 0)
                            Table[i, j] = z;
                }
        }

        private static void equl(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    print(Gr[i][j], Gr[i][j + 1], "=");
            }
        }


        private static String[] lastPl(String s, String[][] Gr)
        {
            String[] a = new String[1100];
            int l = 0;
            for (int i = 0; i < Gr.Length; i++)
            {
                if (Gr[i][0] == s)
                {
                    a[l] = Gr[i][Gr[i].Length - 1];
                    l++;
                    if (!isTerm(Gr[i][Gr[i].Length - 1]) && Gr[i][Gr[i].Length - 1] != Gr[i][0])
                    {
                        String[] a2 = lastPl(Gr[i][Gr[i].Length - 1], Gr);
                        int ll = 0;
                        do
                        {
                            a[l] = a2[ll];
                            l++;
                            ll++;
                        } while (a2[ll] != null);
                    }
                }
            }
            return a;
        }

        private static String[] firstPl(String s, String[][] Gr)
        {
            String[] a = new String[200];
            int l = 0;
            for (int i = 0; i < Gr.Length; i++)
            {
                if (Gr[i][0] == s)
                {
                    a[l] = Gr[i][1];
                    l++;
                    if (!isTerm(Gr[i][1]) && (Gr[i][1] != Gr[i][0]))
                    {
                        String[] a2 = firstPl(Gr[i][1], Gr);
                        int ll = 0;
                        do
                        {
                            a[l] = a2[ll];
                            l++;
                            ll++;
                        } while (a2[ll] != null);
                    }
                }
            }
            return a;
        }


        private static void big(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    if (!isTerm(Gr[i][j]))
                    {
                        String[] lp = lastPl(Gr[i][j], Gr);
                        if (isTerm(Gr[i][j + 1]))
                        {
                            for (int ii = 0; ii < lp.Length; ii++)
                            {
                                print(lp[ii], Gr[i][j + 1], ">");
                            }
                        }
                        else
                        {
                            String[] fp = firstPl(Gr[i][j + 1], Gr);
                            for (int ii = 0; ii < lp.Length; ii++)
                                for (int jj = 0; jj < fp.Length; jj++)
                                    print(lp[ii], fp[jj], ">");
                        }

                    }
            }
        }


        private static void small(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    if (!isTerm(Gr[i][j + 1]))
                    {
                        String[] fp = firstPl(Gr[i][j + 1], Gr);
                        for (int ii = 0; ii < fp.Length; ii++)
                            print(Gr[i][j], fp[ii], "<");
                    }
            }
        }

        public static string SearchRule(List<string> rule, ref bool checker)
        {
            string[] ruleArray = rule.ToArray();

            for (int i = 0; i < Grammar.Length; i++)
            {
                checker = true;
                if (Grammar[i].Length - 1 == ruleArray.Length)
                {
                    int counter = 0;

                    do
                    {
                        if (Grammar[i][counter + 1].Equals(ruleArray[counter]))
                            counter++;
                        else checker = false;
                    } while (checker && counter < ruleArray.Length);
                    if (checker)
                    {
                        return Grammar[i][0];
                    }
                }
            }
            return "";
        }

        public static void Construct()
        {
            Grammar[0] = new[] { "<пр>", "pr", "{", "<сп.об1>", ";", "<сп.оп1>", "}" };
            Grammar[1] = new[] { "<сп.об1>", "<сп.об>" };
            Grammar[2] = new[] { "<сп.об>", "int", "<сп.ид1>" };
            Grammar[3] = new[] { "<сп.об>", "<сп.об>", ";", "int", "<сп.ид1>" };

            Grammar[4] = new[] { "<сп.ид1>", "<сп.ид>" };
            Grammar[5] = new[] { "<сп.ид>", ",", "id" };
            Grammar[6] = new[] { "<сп.ид>", "<сп.ид>", ",", "id" };

            Grammar[7] = new[] { "<сп.оп1>", "<сп.оп>" };
            Grammar[8] = new[] { "<сп.оп>", "<оп>" };
            Grammar[9] = new[] { "<сп.оп>", "<сп.оп>", ";", "<оп>" };

            Grammar[10] = new[] { "<оп>", "read", "(", "<сп.ид1>", ")" };
            Grammar[11] = new[] { "<оп>", "write", "(", "<сп.ид1>", ")" };
            Grammar[12] = new[] { "<оп>", "id", "=", "<врж1>" };
            Grammar[13] = new[]
            {
                "<оп>", "do", "id", "=", "<врж1>", "to", "<врж1>", "by", "<врж1>", "while",
                "<ЛВ1>", "{", "<сп.оп1>", "}"
            };
            Grammar[14] = new[] { "<оп>", "if", "<ЛВ1>","then", "<оп>" };

            Grammar[15] = new[] { "<врж1>", "<врж>" };
            Grammar[16] = new[] { "<врж>", "<врж>", "+", "<терм1>" };
            Grammar[17] = new[] { "<врж>", "<врж>", "-", "<терм1>" };
            Grammar[18] = new[] { "<врж>", "<терм1>" };

            Grammar[19] = new[] { "<терм1>", "<терм>" };
            Grammar[20] = new[] { "<терм>", "<терм>", "*", "<множ1>" };
            Grammar[21] = new[] { "<терм>", "<терм>", "/", "<множ1>" };
            Grammar[22] = new[] { "<терм>", "<множ1>" };

            Grammar[23] = new[] { "<множ1>", "<множ>" };
            Grammar[24] = new[] { "<множ>", "<перв.в>" };
            Grammar[25] = new[] { "<множ>", "<множ>", "^", "<перв.в>" };

            Grammar[26] = new[] { "<перв.в>", "id" };
            Grammar[27] = new[] { "<перв.в>", "con" };
            Grammar[28] = new[] { "<перв.в>", "(", "<врж1>", ")" };

            Grammar[29] = new[] { "<ЛВ1>", "<ЛВ>" };
            Grammar[30] = new[] { "<ЛВ>", "<ЛВ>", "or", "<ЛТ1>" };
            Grammar[31] = new[] { "<ЛВ>", "<ЛТ1>" };

            Grammar[32] = new[] { "<ЛТ1>", "<ЛТ>" };
            Grammar[33] = new[] { "<ЛТ>", "<ЛМ>" };
            Grammar[34] = new[] { "<ЛТ>", "<ЛТ>", "and", "<ЛМ>" };

            Grammar[35] = new[] { "<ЛМ>", "[", "<ЛВ1>", "]" };
            Grammar[36] = new[] { "<ЛМ>", "<врж1>", "<знак>", "<врж1>" };
            Grammar[37] = new[] { "<ЛМ>", "!", "<ЛМ>" };

            Grammar[38] = new[] { "<знак>", "<" };
            Grammar[39] = new[] { "<знак>", "<=" };
            Grammar[40] = new[] { "<знак>", "==" };
            Grammar[41] = new[] { "<знак>", "!=" };
            Grammar[42] = new[] { "<знак>", ">=" };
            Grammar[43] = new[] { "<знак>", ">" };


            allGrTable();
            equl(Grammar);
            big(Grammar);
            small(Grammar);
        }
    }
}
