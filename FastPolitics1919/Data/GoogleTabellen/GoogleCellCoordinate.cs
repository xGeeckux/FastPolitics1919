using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.GoogleTabellen
{
    public class GoogleCellCoordinate
    {
        //- Variables
        public string Column { get; set; }
        public int Row { get; set; }

        public string GetRange()
        {
            if (Row == -1)
                return Column + "";
            else
                return Column + Row;
        }

        //- To Coordinate
        public static GoogleCellCoordinate FromCell(string cell)
        {
            string chars = "";
            string ints = "";
            for (int i = 0; i < cell.Length; i++)
            {
                if (char.IsLetter(cell[i]))
                    chars += cell[i];
                else
                    ints += cell[i];
            }

            if (ints == "")
                ints = "-1";

            return new GoogleCellCoordinate() { Column = chars, Row = Convert.ToInt32(ints) };
        }

        //- Convert from string to int
        public static int ToInt32(string column)
        {
            int sum = 0;
            for (int n = column.Length; n > 0; n--)
            {
                char single = column[Math.Abs(n - column.Length)];
                sum += ToSingleInt32(single) * (int)Math.Pow(26, n - 1);
            }

            return sum;
            /* 
             * => HIFZ = int(H) * 26^3 + int(I) * 26^2 + int(F) * 26^1 + int(Z) * 26^0
             */
        }
        private static int ToSingleInt32(char single_char)
        {
            switch (single_char)
            {
                default:
                    return 0;
                case 'A':
                    return 1;
                case 'B':
                    return 2;
                case 'C':
                    return 3;
                case 'D':
                    return 4;
                case 'E':
                    return 5;
                case 'F':
                    return 6;
                case 'G':
                    return 7;
                case 'H':
                    return 8;
                case 'I':
                    return 9;
                case 'J':
                    return 10;
                case 'K':
                    return 11;
                case 'L':
                    return 12;
                case 'M':
                    return 13;
                case 'N':
                    return 14;
                case 'O':
                    return 15;
                case 'P':
                    return 16;
                case 'Q':
                    return 17;
                case 'R':
                    return 18;
                case 'S':
                    return 19;
                case 'T':
                    return 20;
                case 'U':
                    return 21;
                case 'V':
                    return 22;
                case 'W':
                    return 23;
                case 'X':
                    return 24;
                case 'Y':
                    return 25;
                case 'Z':
                    return 26;
            }
        }
        //- Convert from int to string
        public static string ToString(int column)
        {
            string local = "";

            int num_of_secound = 0;
            int num_of_third = 0;
            int num_of_fourth = 0;

            while (column > 26)
            {
                num_of_secound++;
                column -= 26;
            }
            while (num_of_secound > 26)
            {
                num_of_third++;
                num_of_secound -= 26;
            }
            while (num_of_third > 26)
            {
                num_of_fourth++;
                num_of_third -= 26;
            }

            if (num_of_fourth != 0)
                local += ToSingleString(num_of_fourth);
            if (num_of_third != 0)
                local += ToSingleString(num_of_third);
            if (num_of_secound != 0)
                local += ToSingleString(num_of_secound);
            if (column != 0)
                local += ToSingleString(column);

            return local;
        }
        private static char ToSingleString(int single_char)
        {
            switch (single_char)
            {
                default:
                    return default(char);
                case 1:
                    return 'A';
                case 2:
                    return 'B';
                case 3:
                    return 'C';
                case 4:
                    return 'D';
                case 5:
                    return 'E';
                case 6:
                    return 'F';
                case 7:
                    return 'G';
                case 8:
                    return 'H';
                case 9:
                    return 'I';
                case 10:
                    return 'J';
                case 11:
                    return 'K';
                case 12:
                    return 'L';
                case 13:
                    return 'M';
                case 14:
                    return 'N';
                case 15:
                    return 'O';
                case 16:
                    return 'P';
                case 17:
                    return 'Q';
                case 18:
                    return 'R';
                case 19:
                    return 'S';
                case 20:
                    return 'T';
                case 21:
                    return 'U';
                case 22:
                    return 'V';
                case 23:
                    return 'W';
                case 24:
                    return 'X';
                case 25:
                    return 'Y';
                case 26:
                    return 'Z';
            }
        }

        public static int CalcColumns(string range)
        {
            string[] cors = range.Split(':');
            if (cors.Length != 2)
                return -1;
            string first = FilterChars(cors[0]);
            string secound = FilterChars(cors[1]);

            if (first.Length > 0 && secound.Length > 0)
                return Math.Abs(ToInt32(first) - ToInt32(secound)) + 1;
            return -1;
        }
        private static string FilterChars(string text)
        {
            string only_chars = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetter(text[i]))
                    break;
                only_chars += text[i];
            }
            return only_chars;
        }
    }
}
