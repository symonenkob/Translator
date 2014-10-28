using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    public enum ErrorCode
    {
        [Description("Lex.Ok")]
        Ok = 0,

        [Description("Lex.Unknown lexem: ")]
        LexUnknownLexemFound = 1,

        [Description("Lex.Unassigned value: ")]
        LexUnassignedValueFound = 2,

        [Description("Lex.Invalid identificator. Identificators must start with letter ")]
        LexInvalidIdentificator = 3,

        [Description("Lex.Unknown Identifier: ")]
        LexUnknownIdentifierFound = 4,

        [Description("Lex.Invalid variable declaration. Variables MUST be declared only in second row ")]
        LexInvalidVariableDeclaration = 5,

        [Description("Syn.Invalid variable declaration.")]
        SynInvalidDeclaration,

        [Description("Unexpected end found: ")]
        SynUnexpectedEnd,

        [Description("Unexpected token found: ")]
        SynUnexpectedToken,

        [Description("Unknown error: ")]
        UnknownError
    }
    public class ErrorHandler
    {
        public static string GetErrorDescription(Enum value, string lexem, int row, int? column = null)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                if (column != null)
                    return attributes[0].Description + '\"' + lexem + '\"' + "(row: " + row.ToString() + " col: " +
                           column.ToString() + ")";
                else return attributes[0].Description + '\"' + lexem + '\"' + "(row: " + row.ToString() + ")";
            }
            else
                return value.ToString();
        }
    }
}
