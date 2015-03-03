using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Translator_1.AscendingParse
{
    static class AscendingTranslator
    {
        public static List<AscOutputRow> Translate(List<string> inputChain)
        {
            bool checker = true;
            bool finished = false;
            List<AscOutputRow> outputRows = new List<AscOutputRow>();

            Stack<string> stack = new Stack<string>();
            int step = 0;

            stack.Push("#");

            do
            {
                if (step == 269)
                {

                }
                string curRelation;
                curRelation = TableConstructor.GetRelation(stack.Peek(), inputChain[0]);
                outputRows.Add(new AscOutputRow()
                {
                    Step = step,
                    InputChain = string.Join(" ", inputChain),
                    Relation = curRelation,
                    Stack = string.Join(" ", stack.Reverse())
                });

                switch (curRelation)
                {
                    case ">":
                    {
                        GetMore(ref stack, ref checker);
                    }
                        break;
                    case "<":
                    case "=":
                    {
                        stack.Push(inputChain.ElementAt(0));
                        inputChain.RemoveAt(0);
                    }
                        break;
                    default:
                        checker = false;
                        break;
                }

                step++;

                if (inputChain.Count == 0)
                {
                    GetMore(ref stack, ref checker);
                    outputRows.Add(new AscOutputRow()
                    {
                        Step = step,
                        InputChain = string.Join(" ", inputChain),
                        Relation = curRelation,
                        Stack = string.Join(" ", stack.Reverse())
                    });
                }
            } while ((inputChain.Count > 0) && stack.Peek() != "<пр>" && checker);
            

            return outputRows;
        }

        private static void GetMore(ref Stack<string> stack, ref bool checker)
        {
            string curRelation;
            string topStack;
            string secondStack;

            List<string> newLexems = new List<string>();

            do
            {
                topStack = stack.Pop();
                newLexems.Add(topStack);
                secondStack = stack.Peek();
                curRelation = TableConstructor.GetRelation(secondStack, topStack);
            } while ((curRelation != "<" || (topStack == "<оп>" && secondStack == ";" && curRelation == "<" && stack.ElementAt(1)=="<сп.оп>")) && checker);

            newLexems.Reverse();
            stack.Push(TableConstructor.SearchRule(newLexems, ref checker));
        }


    }
}
