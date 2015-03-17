using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    static class RpnCalculator
    {
        public static int Calculate(string rpnExpression)
        {
            List<string> rpn = rpnExpression.Split(' ').ToList();
            string[] operations = { "+", "-", "*", "/", "^", };
            int result = 0, i = 0;
            bool finished = false;
            while (i<rpn.Count)
            {
                string curLex = rpn[i];

                if (operations.Contains(curLex))
                {
                    string operand1 = null, operand2 = null;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (!operations.Contains(rpn[j]))
                        {
                            if (operand1 == null)
                            {
                                operand1 = rpn[j];
                                rpn.RemoveAt(j);
                                i--;
                            }
                            else
                            {
                                operand2 = rpn[j];
                                rpn.RemoveAt(j);
                                i--;
                                break;
                            }
                        }
                    }
                    rpn[i] = DoOperation(int.Parse(operand2), int.Parse(operand1), curLex).ToString();
                }

                i++;
            }

            return int.Parse(rpn[0]);
        }

        private static int DoOperation(int a, int b, string operation)
        {
            switch (operation)
            {
                case "*":
                    return a*b;
                    break;
                case "/":
                    return a/b;
                    break;
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                    break;
                case "^":
                    return (int)Math.Pow(a, b);
                    break;
                default:
                    return 0;
                    break;
            }
            return 0;
        }
    }
}
