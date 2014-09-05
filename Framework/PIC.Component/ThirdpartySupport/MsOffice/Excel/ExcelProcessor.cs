using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using Excel = Aspose.Cells;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// Excel处理器
    /// </summary>
    public class ExcelProcessor : IDisposable
    {
        #region 成员属性

        private string _filePath;
        private string _extProp = "Excel 8.0";

        Excel.Workbook _wb;

        /// <summary>
        /// Excel工作册
        /// </summary>
        public Excel.Workbook Workbook
        {
            get
            {
                if (_wb == null && !String.IsNullOrEmpty(_filePath))
                {
                    _wb = new Excel.Workbook(_filePath);
                }

                return _wb;
            }
        }

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtProp
        {
            get { return _extProp; }
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string t_connstr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties={1}", _filePath, _extProp);

                return t_connstr;
            }
        }

        #endregion

        #region 构造析构函数

        internal ExcelProcessor(string filePath)
        {
            this._filePath = filePath;
        }

        internal ExcelProcessor(string filePath, string extProp)
            : this(filePath)
        {
            this._extProp = extProp;
        }

        ~ExcelProcessor()
        {
            this.Close();
        }

        #endregion

        #region 公共函数

        #region 数据操作

        /// <summary>
        /// 获取OleDb连接
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetOleDbConnection()
        {
            OleDbConnection conn = new OleDbConnection(this.ConnectionString);

            return conn;
        }

        /// <summary>
        /// 获取第一个工作表DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();

            foreach (Excel.Worksheet ws in Workbook.Worksheets)
            {
                DataTable tdt = GetDataTable(ws);

                if (tdt != null)
                {
                    ds.Tables.Add(tdt);
                }
            }

            return ds;
        }

        /// <summary>
        /// 获取默认第一个DataTable
        /// </summary>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            Excel.Worksheet ws = Workbook.Worksheets[0];

            return GetDataTable(ws);
        }

        /// <summary>
        /// 根据工作表名获取 DataTable
        /// </summary>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sheetname)
        {
            Excel.Worksheet ws = Workbook.Worksheets[sheetname];

            return GetDataTable(ws);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public DataTable GetDataTable(Excel.Worksheet ws)
        {
            if (ws.Cells.Rows.Count > 0 && ws.Cells.MaxColumn > 0)
            {
                DataTable dt = ws.Cells.ExportDataTable(0, 0, ws.Cells.MaxRow + 1, ws.Cells.MaxColumn + 1, true);

                return dt;
            }

            return null;
        }

        /// <summary>
        /// 获取工作表名列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSheetNames()
        {
            List<string> sns = new List<string>();

            foreach (Excel.Worksheet ws in Workbook.Worksheets)
            {
                sns.Add(ws.Name);
            }

            return sns;
        }

        /// <summary>
        /// 获取第一个工作表名
        /// </summary>
        /// <returns></returns>
        public string GetFirstSheetName()
        {
            IList<string> sns = GetSheetNames();

            if (sns.Count > 0)
            {
                return sns[0].TrimEnd('$');
            }

            return null;
        }

        /// <summary>
        /// 获取第一个工作表
        /// </summary>
        /// <returns></returns>
        public Excel.Worksheet GetFirstSheet()
        {
            string sname = GetFirstSheetName();

            if (!String.IsNullOrEmpty(sname))
            {
                return GetSheet(sname);
            }

            return null;
        }

        /// <summary>
        /// 获取一个工作表
        /// </summary>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public Excel.Worksheet GetSheet(string sheetname)
        {
            Excel.Worksheet s = Workbook.Worksheets[sheetname] as Excel.Worksheet;

            return s;
        }

        /// <summary>
        /// 获取所有工作表
        /// </summary>
        /// <returns></returns>
        public IList<Excel.Worksheet> GetAllSheets()
        {
            IList<Excel.Worksheet> wss = new List<Excel.Worksheet>();

            System.Collections.IEnumerator wsenum = Workbook.Worksheets.GetEnumerator();

            while (wsenum.MoveNext())
            {
                Excel.Worksheet tws = wsenum.Current as Excel.Worksheet;
                if (tws != null)
                {
                    wss.Add(tws);
                }
            }

            return wss;
        }

        /// <summary>
        /// 添加一个工作表
        /// </summary>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public Excel.Worksheet AddSheet(string sheetname)
        {
            Excel.Worksheet s = Workbook.Worksheets.Add(sheetname);
            s.Name = sheetname;

            return s;
        }

        /// <summary>
        /// 删除一个工作表
        /// </summary>
        public void DelSheet(string sheetname)
        {
            // Workbook.Worksheets.DeleteName(sheetname);

            Workbook.Worksheets.Names.Remove(sheetname);
        }

        /// <summary>
        /// 重命名工作表
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        public Excel.Worksheet RenameSheet(string oldname, string newname)
        {
            Excel.Worksheet s = GetSheet(oldname);

            return RenameSheet(s, newname);
        }

        /// <summary>
        /// 重命名工作表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        public Excel.Worksheet RenameSheet(Excel.Worksheet sheet, string newname)
        {
            sheet.Name = newname;

            return sheet;
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        public void SetCellValue(string sheetname, int rowIndex, int columnIndex, object value)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            SetCellValue(ws, rowIndex, columnIndex, value);
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        public void SetCellValue(Excel.Worksheet ws, int rowIndex, int columnIndex, object value)
        {
            ws.Cells[rowIndex, columnIndex].PutValue(value);
        }

        /// <summary>
        /// 获取Excel范围
        /// </summary>
        /// <param name="sheetname"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="startColumnIndex"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="endColumnIndex"></param>
        /// <returns></returns>
        public Excel.Range GetRange(string sheetname, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            return GetRange(ws, startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);
        }

        /// <summary>
        /// 获取Excel范围
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="startColumnIndex"></param>
        /// <param name="endRowIndex"></param>
        /// <param name="endColumnIndex"></param>
        /// <returns></returns>
        public Excel.Range GetRange(Excel.Worksheet ws, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
        {
            Excel.Range range = ws.Cells.CreateRange(startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);

            return range;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="rowIndex2"></param>
        /// <param name="columnIndex2"></param>
        public void UniteCells(Excel.Worksheet ws, int rowIndex1, int columnIndex1, int rowIndex2, int columnIndex2)
        {
            Excel.Range range = GetRange(ws, rowIndex1, columnIndex1, rowIndex2, columnIndex2);

            range.Merge();
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="rowIndex2"></param>
        /// <param name="columnIndex2"></param>
        public void UniteCells(string sheetname, int rowIndex1, int columnIndex1, int rowIndex2, int columnIndex2)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            UniteCells(ws, rowIndex1, columnIndex1, rowIndex2, columnIndex2);
        }

        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用
        /// </summary>
        /// <param name="sheetname"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="startColumnIndex"></param>
        public void InsertTable(DataTable dt, string sheetname, int startRowIndex, int startColumnIndex)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            InsertTable(dt, ws, startRowIndex, startColumnIndex);
        }

        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="startColumnIndex"></param>
        public void InsertTable(DataTable dt, Excel.Worksheet ws, int startRowIndex, int startColumnIndex)
        {
            ws.Cells.ImportDataTable(dt, true, startRowIndex, startColumnIndex);
        }

        /// <summary>
        /// 获取拥有批注的Cell
        /// </summary>
        /// <returns></returns>
        public IList<ExcelCell> GetCellsWithComment()
        {
            string sheetname = GetFirstSheetName();

            return GetCellsWithComment(sheetname);
        }
        
        /// <summary>
        /// 获取拥有批注的Cell
        /// </summary>
        /// <returns></returns>
        public IList<ExcelCell> GetCellsWithComment(string sheetname)
        {
            IList<ExcelCell> cells = new List<ExcelCell>();

            Excel.Worksheet ws = GetSheet(sheetname);
            IList<Excel.Comment> cms = GetAllComments(ws);

            foreach (Excel.Comment tcm in cms)
            {
                Excel.Cell cell = ws.Cells[tcm.Row, tcm.Column];
                if (cell != null)
                {
                    cells.Add(new ExcelCell(cell.Row, cell.Column, ws.Name, GetText(cell), GetText(tcm)));
                }
            }

            return cells;
        }

        /// <summary>
        /// 获取Excel单元格
        /// </summary>
        /// <param name="sheetname"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <returns></returns>
        public IList<ExcelCell> GetCells(int rowIndex1, int columnIndex1, int rowIndex2, int columnIndex2)
        {
            string sname = this.GetFirstSheetName();

            return GetCells(sname, rowIndex1, columnIndex1, rowIndex2, columnIndex2);
        }

        /// <summary>
        /// 获取Excel单元格
        /// </summary>
        /// <param name="sheetname"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <returns></returns>
        public IList<ExcelCell> GetCells(string sheetname, int rowIndex1, int columnIndex1, int rowIndex2, int columnIndex2)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            IList<ExcelCell> cellList = new List<ExcelCell>();

            for (int i = rowIndex1; i < rowIndex2; i++)
            {
                for (int j = columnIndex1; j < columnIndex2; j++)
                {
                    Excel.Cell cell = ws.Cells[i, j];

                    cellList.Add(new ExcelCell(i, j, ws.Name, GetText(cell), GetText(ws.Comments[i, j])));
                }
            }

            return cellList;
        }

        /// <summary>
        /// 获取指定工作表所有批注
        /// </summary>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public IList<Excel.Comment> GetAllComments(string sheetname)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            return GetAllComments(ws);
        }

        /// <summary>
        /// 获取指定工作表所有批注
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public IList<Excel.Comment> GetAllComments(Excel.Worksheet ws)
        {
            IList<Excel.Comment> cms = new List<Excel.Comment>();

            System.Collections.IEnumerator cmenum = ws.Comments.GetEnumerator();

            while (cmenum.MoveNext())
            {
                Excel.Comment tcm = cmenum.Current as Excel.Comment;
                if (tcm != null)
                {
                    cms.Add(tcm);
                }
            }

            return cms;
        }

        /// <summary>
        /// 获取批注
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="rowIndex2"></param>
        /// <param name="columnIndex2"></param>
        public Excel.Comment GetComment(string sheetname, int rowIndex1, int columnIndex1)
        {
            Excel.Worksheet ws = GetSheet(sheetname);

            return GetComment(ws, rowIndex1, columnIndex1);
        }

        /// <summary>
        /// 获取批注
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="columnIndex1"></param>
        /// <param name="rowIndex2"></param>
        /// <param name="columnIndex2"></param>
        public Excel.Comment GetComment(Excel.Worksheet ws, int rowIndex, int columnIndex)
        {
            return ws.Comments[rowIndex, columnIndex];
        }

        /// <summary>
        /// 获取批注
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        public string GetCommentText(int rowIndex, int columnIndex)
        {
            Excel.Worksheet ws = GetFirstSheet();

            return GetCommentText(ws, rowIndex, columnIndex);
        }

        /// <summary>
        /// 获取批注文本
        /// </summary>
        /// <param name="sheetname"></param>
        /// <param name="rowIndex1"></param>
        /// <param name="rowIndex2"></param>
        /// <returns></returns>
        public string GetCommentText(string sheetname, int rowIndex, int columnIndex)
        {
            Excel.Comment comment = GetComment(sheetname, rowIndex, columnIndex);

            return GetCommentText(comment);
        }

        /// <summary>
        /// 获取批注文本
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public string GetCommentText(Excel.Worksheet ws, int rowIndex, int columnIndex)
        {
            Excel.Comment comment = GetComment(ws, rowIndex, columnIndex);

            return GetCommentText(comment);
        }

        /// <summary>
        /// 获取批注文本
        /// </summary>
        /// <returns></returns>
        public string GetCommentText(Excel.Comment comment)
        {
            return GetText(comment);
        }

        /// <summary>
        /// 获取文字内容
        /// </summary>
        /// <returns></returns>
        public string GetText(Excel.Cell cell)
        {
            return cell.Value == null ? null : cell.Value.ToString();
        }

        /// <summary>
        /// 获取文字内容
        /// </summary>
        /// <returns></returns>
        public string GetText(Excel.Comment comment)
        {
            return comment.Note == null ? null : comment.Note.ToString();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public bool SaveFile()
        {
            if (!String.IsNullOrEmpty(FilePath))
            {
                try
                {
                    Workbook.Save(FilePath);
                    return true;
                }
#if debug
                catch (Exception ex)
#else
                catch
#endif
                {
                }
            }

            return false;
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <returns></returns>
        public bool SaveFileAs(string filename)
        {
            try
            {
                Workbook.Save(filename);

                return true;
            }
#if debug
                catch (Exception ex)
#else
            catch
#endif
            { }

            return false;
        }

        #endregion

        #endregion

        #region 私有函数

        /// <summary>
        /// 关闭应用
        /// </summary>
        private void Close()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }

    /// <summary>
    /// Excel单元格属性
    /// </summary>
    public class ExcelCell
    {
        #region 成员函数

        private int _rowIndex = 0;

        /// <summary>
        /// RowIndex位置
        /// </summary>
        public int RowIndex
        {
            get { return _rowIndex; }
        }


        private int _columnIndex = 0;

        /// <summary>
        /// ColumnIndex位置
        /// </summary>
        public int ColumnIndex
        {
            get { return _columnIndex; }
        }

        private string _sheetName = String.Empty;

        /// <summary>
        /// 工作表名
        /// </summary>
        public string SheetName
        {
            get { return _sheetName; }
        }

        private string _common = String.Empty;

        /// <summary>
        /// 获取批注字符
        /// </summary>
        public string Comment
        {
            get
            {
                return _common;
            }
        }

        private string _text = String.Empty;

        /// <summary>
        /// 获取内容
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
        }

        #endregion

        #region 构造函数

        public ExcelCell()
        {

        }

        public ExcelCell(int rowIndex, int colIndex, string sheetName, string text, string common)
        {
            this._rowIndex = rowIndex;
            this._columnIndex = colIndex;
            this._sheetName = sheetName;
            this._common = common;
            this._text = text;
        }

        public ExcelCell(int rowIndex, int colIndex, string sheetName, string text)
            : this(rowIndex, colIndex, sheetName, text, String.Empty)
        {
        }

        #endregion
    }
}
