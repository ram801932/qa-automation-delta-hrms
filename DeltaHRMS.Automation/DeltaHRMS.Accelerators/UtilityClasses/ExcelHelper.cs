using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaHRMS.Accelerators.UtilityClasses
{
    public static class ExcelHelper
    {
        public static DataTable ReadExcel(string fileName, string fileExt, string sheetName)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [" + sheetName + "$]", con); //here we read data from sheet  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return dtexcel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        public static DataTable ReadExcel(string fileName, string sheetName)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;

            filePath = fileName; //get the path of the file  
            fileExt = Path.GetExtension(filePath); //get the file extension  
            if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
            {
                try
                {
                    DataTable dtExcel = new DataTable();
                    dtExcel = ReadExcel(filePath, fileExt, sheetName); //read excel file  
                    return dtExcel;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception("Not a .xls or .xlsx file");
            }
        }
    }
}
