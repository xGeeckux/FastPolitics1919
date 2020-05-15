using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Data.GoogleTabellen
{
    public class GoogleCell
    {
        //- Origin
        public SheetOrigin Origin { get; set; }

        //- Coordinates
        public GoogleCellCoordinate Coordinate { get; set; }

        //- Cell Value
        public object Content { get; set; }
        public object OriginalContent { get; set; }

        //- Update Cell
        public void ReadUpdate()
        {
            var tmp_sheet = new GoogleSheet();
            tmp_sheet.IgnoreNull = true;
            tmp_sheet.SetSheetUrl(Origin.SheetUrl);
            tmp_sheet.SetSheetTab(Origin.SheetTab);
            var tmp_cell = tmp_sheet.ReadCell(Coordinate.Column, Coordinate.Row);
            Origin.LastRequested = tmp_cell.Origin.LastRequested;
            Content = tmp_cell.Content;
        }
        public void WriteUpdate()
        {
            var tmp_sheet = new GoogleSheet();
            tmp_sheet.SetSheetUrl(Origin.SheetUrl);
            tmp_sheet.SetSheetTab(Origin.SheetTab);
            tmp_sheet.WriteCell(this);
            Origin.LastRequested = DateTime.Now;
        }

        //- All cell information
        public string GetCellInformation()
        {
            string hint_null = "'null'";
            string cur_content = (string)Content;
            if (cur_content == "" || cur_content == null)
                cur_content = hint_null;
            string org_contnet = (string)OriginalContent;
            if (org_contnet == "" || org_contnet == null)
                org_contnet = hint_null;
            string origins = $" * Url:{Origin.SheetUrl}, Sheet:{Origin.SheetTab}";
            string cors = $" * Coordinates:\t{Coordinate.Column}{Coordinate.Row}";
            string contents = $" * Content:\t{cur_content}\n * O-Content:\t{org_contnet}";
            string dates = $" * Base:\t{Origin.BaseRequest.ToShortDateString()}, {Origin.BaseRequest.ToLongTimeString()}\n * Last:\t{Origin.LastRequested.ToShortDateString()}, {Origin.LastRequested.ToLongTimeString()}";
            return "/* Information\n" + origins + "\n" + cors + "\n" + contents + "\n" + dates + "\n */";
        }
    }
}
