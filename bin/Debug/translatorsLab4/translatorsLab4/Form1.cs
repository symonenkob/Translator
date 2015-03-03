using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace translatorsLab4
{
    public partial class Form1 : Form
    {
      

        public void allGrTable()
        {
            String[] lex = {"<сп. об1>", "<сп. об>","<сп. оп1>","<сп. оп>", "<оп>","<сп. ид1>",
                                "<сп. ид>","<тип>","<врж1>","<врж>","<терм1>","<терм>","<множ>",
                                "<ЛВ1>","<ЛВ>","<ЛТ1>","<ЛТ>","<ЛМ>","<знак>",

                               "program", "id", "var", "begin", "end", "int", "string","double",
                               "read", "write", "for", "to", "step", "next",
                               "if", "then", "else", "endif",

                               "NL", ":", ",", "(", ")", "[", "]", "=", "+", "-", "*", "/",
                               "||", "&&", "<", "<=", "==", "!=", ">=", ">",

                               "#"
                            };

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "1/2";

            for (int i = 1; i < lex.Length + 1; i++)           
                dataGridView1.Rows[0].Cells[i].Value = lex[i-1];
            

            for (int i = 1; i < lex.Length + 1; i++)          
                dataGridView1.Rows.Add();            

            for (int i = 1; i < lex.Length + 1; i++)            
                dataGridView1.Rows[i].Cells[0].Value = lex[i - 1];            

            for (int i = 1; i < lex.Length; i++)
            {
                dataGridView1.Rows[i].Cells[lex.Length].Value = ">";
                dataGridView1.Rows[lex.Length].Cells[i].Value = "<";
            }
            dataGridView1.Rows[0].Frozen = true;
 
        }

        public Boolean isTerm(String a)
        {
            if (a.Contains('<') && a.Contains('>')) return false;
            return true;
        }

        public void print(String s1, String s2, String z)
        {
            for (int i = 0; i< dataGridView1.RowCount-1; i++)
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == s1)
                {
                    for (int j = 0; j < dataGridView1.RowCount-1; j++)
                        if (dataGridView1.Rows[0].Cells[j].Value.ToString() == s2)
                            dataGridView1.Rows[i].Cells[j].Value = z;
                }
        }

        public void equl(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    print(Gr[i][j], Gr[i][j+1],"=");
            }
        }


        public String[] lastPl(String s, String[][] Gr)
        {
            String[] a = new String[200]; int l = 0;
            for (int i = 0; i < Gr.Length; i++)
            {
                if (Gr[i][0] == s)
                {
                    a[l] = Gr[i][Gr[i].Length - 1];
                    l++;
                    if (!isTerm(Gr[i][Gr[i].Length - 1]))
                    {
                        String[] a2 = lastPl(Gr[i][Gr[i].Length - 1], Gr); int ll = 0;
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

        public String[] firstPl(String s, String[][] Gr)
        {
            String[] a = new String[200]; int l = 0;
            for (int i = 0; i < Gr.Length; i++)
            {
                if (Gr[i][0] == s)
                {
                    a[l] = Gr[i][1];
                    l++;
                    if (!isTerm(Gr[i][1]) && (Gr[i][1] != Gr[i][0]))
                    {
                        String[] a2 = firstPl(Gr[i][1], Gr); int ll = 0;
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


        public void big(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    if (!isTerm(Gr[i][j]))
                    {
                        String[] lp = lastPl(Gr[i][j], Gr); 
                        if (isTerm(Gr[i][j+1]))
                            {
                                for (int ii = 0; ii < lp.Length; ii++)
                                {
                                //    MessageBox.Show(lp[ii].ToString());
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


        public void small(String[][] Gr)
        {
            for (int i = 0; i < Gr.Length; i++)
            {
                for (int j = 1; j < Gr[i].Length - 1; j++)
                    if (!isTerm(Gr[i][j+1]))
                    {
                        String[] fp = firstPl(Gr[i][j+1],Gr); 
                        for (int ii = 0; ii < fp.Length; ii++)
                            print(Gr[i][j], fp[ii], "<");
                    }
            }
        }




        public Form1()
        {
            InitializeComponent();
/*            
            String[][] readGr = new String[4][];
            readGr[0] = new String [6] {"<оператор>" , "read", "(", "<сп1>", ")", ";"};
            readGr[1] = new String[2] { "<сп1>", "<сп. id>" };
            readGr[2] = new String[2] { "<сп. id>", "id" };
            readGr[3] = new String[4] { "<сп. id>", "<сп. id>", ",", "id" };

            String[][] vrjGr = new String[8][];
            vrjGr[0] = new String[2] { "<E1>", "<E>" };
            vrjGr[1] = new String[4] { "<E>", "<E>" , "+", "<T1>"};
            vrjGr[2] = new String[2] { "<E>", "<T1>" };
            vrjGr[3] = new String[2] { "<T1>", "<T>" };
            vrjGr[4] = new String[4] { "<T>", "<T>", "*", "<F>" };
            vrjGr[5] = new String[2] { "<T>", "<F>" };
            vrjGr[6] = new String[4] { "<F>", "(", "<E1>", ")" };
            vrjGr[7] = new String[2] { "<F>", "id" };
*/
            String[][] allGr = new String[42][];
            allGr[0] = new String[12] {"<прогр>", "program", "id", "NL", "var", "<сп. об1>","NL","begin",
                                        "NL", "<сп. оп1>", "NL", "end"};
            allGr[1] = new String[2] {"<сп. об1>","<сп. об>"};
            allGr[2] = new String[4] {"<сп. об>", "<сп. ид1>", ":", "<тип>"};
            allGr[3] = new String[6] { "<сп. об>", "<сп. об>", "NL", "<сп. ид1>", ":", "<тип>" };
            allGr[4] = new String[2] { "<сп. ид1>","<сп. ид>" };
            allGr[5] = new String[2] { "<сп. ид>", "id" };
            allGr[6] = new String[4] { "<сп. ид>", "<сп. ид>" , ",", "id" };
            allGr[7] = new String[2] { "<тип>", "int" };
            allGr[8] = new String[2] { "<тип>", "string" };
            allGr[9] = new String[2] { "<тип>", "double" };

            allGr[10] = new String[2] { "<сп. оп1>", "<сп. оп>" };
            allGr[11] = new String[2] { "<сп. оп>", "<оп>" };
            allGr[12] = new String[4] { "<сп. оп>", "<сп. оп>" , "NL", "<оп>"};

            allGr[13] = new String[5] { "<оп>", "read", "(", "<сп. ид1>", ")" };
            allGr[14] = new String[5] { "<оп>", "write", "(", "<сп. ид1>", ")"  };
            allGr[15] = new String[4] { "<оп>", "id", "=", "<врж1>" };
            allGr[16] = new String[13] { "<оп>", "for", "id", "=", "<врж1>", "to", "<врж1>", "step", "<врж1>", "NL",
                                            "<сп. оп1>", "NL", "next" };
            allGr[17] = new String[12] { "<оп>", "if", "<ЛВ1>", "then", "NL", "<сп. оп1>", "NL", "else", "NL", "<сп. оп1>", "NL", "endif" };

            allGr[18] = new String[2] { "<врж1>", "<врж>" };
            allGr[19] = new String[4] { "<врж>", "<врж>", "+", "<терм1>" };
            allGr[20] = new String[4] { "<врж>", "<врж>", "-", "<терм1>" };
            allGr[21] = new String[2] { "<врж>", "<терм1>" };
            allGr[22] = new String[2] { "<терм1>", "<терм>" };
            allGr[23] = new String[4] { "<терм>", "<терм>", "*", "<множ>" };
            allGr[24] = new String[4] { "<терм>", "<терм>", "/", "<множ>" };
            allGr[25] = new String[2] { "<терм>", "<множ>" };
            allGr[26] = new String[4] { "<множ>", "(", "<врж1>", ")" };
            allGr[27] = new String[2] { "<множ>", "id" };

            allGr[28] = new String[2] { "<ЛВ1>", "<ЛВ>" };
            allGr[29] = new String[4] { "<ЛВ>", "<ЛВ>", "||", "<ЛТ1>" };
            allGr[30] = new String[2] { "<ЛВ>", "<ЛТ1>" };
            allGr[31] = new String[2] { "<ЛТ1>", "<ЛТ>" };
            allGr[32] = new String[2] { "<ЛТ>", "<ЛМ>" };
            allGr[33] = new String[4] { "<ЛТ>", "<ЛТ>", "&&", "<ЛМ>" };
            allGr[34] = new String[4] { "<ЛМ>", "[", "<ЛВ1>", "]" };
            allGr[35] = new String[4] { "<ЛМ>", "<врж1>", "<знак>", "<врж1>"};
            allGr[36] = new String[2] { "<знак>", "<"};
            allGr[37] = new String[2] { "<знак>", "<="};
            allGr[38] = new String[2] { "<знак>", "==" };
            allGr[39] = new String[2] { "<знак>", "!=" };
            allGr[40] = new String[2] { "<знак>", ">=" };
            allGr[41] = new String[2] { "<знак>", ">" };


      //      table();
            allGrTable();
            equl(allGr);
            big(allGr);
            small(allGr);

        }
    }
}
