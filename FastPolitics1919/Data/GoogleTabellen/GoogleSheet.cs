using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace FastPolitics1919.Data.GoogleTabellen
{
    public class GoogleSheet
    {
        //- Variables
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        //- Active Sheet Information
        public string SheetUrl { get; set; }
        public string SheetTab { get; set; }

        //- Needed for Google access
        private SheetsService Service { get; set; }

        //- Properties
        public bool IgnoreNull { get; set; }
        public bool CompleteFill { get; set; }

        //- Constructor
        public GoogleSheet()
        {
            Init();
        }
        public GoogleSheet(string sheet_url)
        {
            SetSheetUrl(sheet_url);
            Init();
        }
        
        //- Secound Constructor
        private void Init()
        {
            try
            {
                string ApplicationName = "Google-Default Name";
                string json_file_path = "client_secrets.json";
                GoogleCredential credential;
                using (var stream = new FileStream(json_file_path, FileMode.Open, FileAccess.Read))
                    credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
                Service = new SheetsService(new BaseClientService.Initializer() {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }

        //- Update Sheet Information
        public void SetSheetUrl(string sheet_url)
        {
            SheetUrl = sheet_url;
        }
        public void SetSheetTab(string tab_name)
        {
            SheetTab = tab_name;
        }

        //-------------------------------------------------------------------------------
        //- Google Methodes
        //-------------------------------------------------------------------------------
        #region Google Methodes
        //- Sheet Origin
        private SheetOrigin GetCurrentOrigin()
        {
            var origin = new SheetOrigin
            {
                SheetTab = SheetTab,
                SheetUrl = SheetUrl,
                BaseRequest = DateTime.Now,
                LastRequested = DateTime.Now
            };
            return origin;
        }
        //- Read (Single)
        public GoogleCell ReadCell(string cell)
        {
            return ReadCell(GoogleCellCoordinate.FromCell(cell));
        }
        public GoogleCell ReadCell(string column, int row)
        {
            return ReadCell(new GoogleCellCoordinate() { Column = column, Row = row });
        }
        private GoogleCell ReadCell(GoogleCellCoordinate coordinate)
        {
            var range = $"{SheetTab}!" + coordinate.Column + coordinate.Row;
            var request = Service.Spreadsheets.Values.Get(SheetUrl, range);

            GoogleCell cell = new GoogleCell();
            cell.Coordinate = coordinate;
            cell.Origin = GetCurrentOrigin();

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
                foreach (var row in values)
                {
                    cell.OriginalContent = row[0];
                    cell.Content = cell.OriginalContent;
                    return cell;
                }
            
            //- Error, not found
            if (!IgnoreNull)
            {
                Console.WriteLine("No data was found in: " + range);
                return null;
            }
            else
            {
                cell.OriginalContent = "";
                cell.Content = cell.OriginalContent;
                return cell;
            }
        }
        //- Read (Multiple)
        public List<GoogleCell[]> ReadCells(string cell_range)
        {
            string range = $"{SheetTab}!" + cell_range;
            var request = Service.Spreadsheets.Values.Get(SheetUrl, range);

            GoogleCellCoordinate first = GoogleCellCoordinate.FromCell(cell_range.Split(':')[0]);
            if (first.Column == "")
                first.Column = "A";
            if (first.Row == -1)
                first.Row = 1;

            var response = request.Execute();
            IList<IList<object>> values = response.Values;

            List<GoogleCell[]> local = new List<GoogleCell[]>();

            int column_max = GoogleCellCoordinate.CalcColumns(cell_range);
            if (CompleteFill && column_max == -1 && values != null && values.Count > 0)
            {
                int biggest = 0;
                for (int row = 0; row < values.Count; row++)
                {
                    if (values[row].Count > biggest)
                        biggest = values[row].Count;
                }
                column_max = biggest;
            }

            if (values != null && values.Count > 0)
                for (int row = 0; row < values.Count; row++)
                {
                    int intern_length = values[row].Count;
                    if (CompleteFill)
                        intern_length = column_max;
                    GoogleCell[] complete_row = new GoogleCell[intern_length];
                    for (int column = 0; column < values[row].Count; column++)
                    {
                        GoogleCell cell = new GoogleCell();
                        cell.Content = values[row][column];
                        cell.Coordinate = new GoogleCellCoordinate()
                        {
                            Column = GoogleCellCoordinate.ToString(GoogleCellCoordinate.ToInt32(first.Column) + column),
                            Row = first.Row + row
                        };
                        cell.Origin = GetCurrentOrigin();
                        if (cell.Content != null && cell.Content.ToString() != "")
                            complete_row[column] = cell;
                    }
                    local.Add(complete_row);
                }
            if (local.Count == 0)
                Console.WriteLine("No data was found: " + range);
            return local;
        }
        //- Read (Complete Tab)
        public List<GoogleCell[]> ReadFull()
        {
            string range = $"{SheetTab}";
            var request = Service.Spreadsheets.Values.Get(SheetUrl, range);

            GoogleCellCoordinate first = new GoogleCellCoordinate() { Column = "A", Row = 1 };

            var response = request.Execute();
            IList<IList<object>> values = response.Values;

            List<GoogleCell[]> local = new List<GoogleCell[]>();

            int column_max = 0;
            if (CompleteFill && values != null && values.Count > 0)
            {
                int biggest = 0;
                for (int row = 0; row < values.Count; row++)
                {
                    if (values[row].Count > biggest)
                        biggest = values[row].Count;
                }
                column_max = biggest;
            }

            if (values != null && values.Count > 0)
                for (int row = 0; row < values.Count; row++)
                {
                    int intern_length = values[row].Count;
                    if (CompleteFill)
                        intern_length = column_max;
                    GoogleCell[] complete_row = new GoogleCell[intern_length];
                    for (int column = 0; column < values[row].Count; column++)
                    {
                        GoogleCell cell = new GoogleCell();
                        cell.Content = values[row][column];
                        cell.Coordinate = new GoogleCellCoordinate()
                        {
                            Column = GoogleCellCoordinate.ToString(GoogleCellCoordinate.ToInt32(first.Column) + column),
                            Row = first.Row + row
                        };
                        cell.Origin = GetCurrentOrigin();
                        if (cell.Content != null && cell.Content.ToString() != "")
                            complete_row[column] = cell;
                    }
                    local.Add(complete_row);
                }
            if (local.Count == 0)
                Console.WriteLine("No data was found: " + range);
            return local;
        }
        //- Write (Single)
        public void WriteCell(GoogleCell cell)
        {
            WriteCell((string)cell.Content, cell.Coordinate.Column + cell.Coordinate.Row);
        }
        public void WriteCell(string content, GoogleCellCoordinate coordinate)
        {
            WriteCell(content, coordinate.Column + coordinate.Row);
        }
        public void WriteCell(string content, string coordinate)
        {
            string range = $"{SheetTab}!" + coordinate;
            ValueRange valueRange = new ValueRange();

            if (content == null)
                content = "";
            List<object> objectList = new List<object>() { content };
            valueRange.Values = new List<IList<object>> { objectList };

            var updateRequest = Service.Spreadsheets.Values.Update(valueRange, SheetUrl, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();
        }
        //- Write (Multiple)
        public void WriteCells(List<GoogleCell[]> cells)
        {
            if (cells == null && cells.Count == 0)
                return;
            GoogleCell top_left, bottom_right;
            top_left = cells[0][0];
            bottom_right = cells[cells.Count - 1][cells[cells.Count - 1].Length - 1];

            if (bottom_right == null)
            {
                GoogleCell bottom_rightest = null;
                foreach (GoogleCell[] cell in cells)
                {
                    int max_length = 0;
                    for (int i = 0; i < cell.Length; i++)
                    {
                        if (cell[i] != null && i >= max_length)
                        {
                            max_length = i;
                            bottom_rightest = cell[i];
                        }
                    }
                }
                bottom_right = bottom_rightest;
            }
            string range = $"{SheetTab}!" + top_left.Coordinate.Column + top_left.Coordinate.Row + ":" + bottom_right.Coordinate.Column + bottom_right.Coordinate.Row;
            ValueRange valueRange = new ValueRange
            {
                Values = new List<IList<object>> { }
            };

            foreach (GoogleCell[] line in cells)
            {
                List<object> obj = new List<object>();
                for (int i = 0; i < line.Length; i++)
                    if (line[i] != null)
                        obj.Add(line[i].Content);
                valueRange.Values.Add(obj);
            }

            var updateRequest = Service.Spreadsheets.Values.Update(valueRange, SheetUrl, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();
        }
        //- Append (Multiple)
        public void Append(GoogleCell[] cells)
        {
            if (cells == null || cells.Length == 0)
                return;
            GoogleCell left = cells[0];
            GoogleCellCoordinate cor = new GoogleCellCoordinate();
            cor.Column = GoogleCellCoordinate.ToString(GoogleCellCoordinate.ToInt32(left.Coordinate.Column) + cells.Length);

            string range = $"{SheetTab}!" + left.Coordinate.Column + ":" + cor.Column;
            ValueRange valueRange = new ValueRange();
            
            List<object> objectList = new List<object>();
            foreach (GoogleCell cell in cells)
                objectList.Add(cell.Content);

            valueRange.Values = new List<IList<object>> { objectList };

            var updateRequest = Service.Spreadsheets.Values.Append(valueRange, SheetUrl, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();
        }
        //- Copy (Tab)
        public void CopyTabFrom(string sheet_url, int tab_id)
        {
            CopySheetToAnotherSpreadsheetRequest requestBody = new CopySheetToAnotherSpreadsheetRequest
            {
                DestinationSpreadsheetId = SheetUrl
            };
            var request = Service.Spreadsheets.Sheets.CopyTo(requestBody, sheet_url, tab_id);
            SheetProperties response = request.Execute();
        }
        //- Create New Tab
        public void CreateTab()
        {
            CopyTabFrom(SheetUrl, 0);
        }
        #endregion
        //-------------------------------------------------------------------------------
    }
}
