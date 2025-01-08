using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
//编辑器
public static class MyEditor 
{
    [MenuItem("我的工具/excel转换成txt")]
    public static void ExportExcelToTxt()
    {
        string assetPath = Application.dataPath + "/_Excel";
        string[] files = Directory.GetFiles(assetPath,"*.xlsx");
        for(int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace('\\', '/');
            //通过文件流读取文件
            using (FileStream fs = File.Open(files[i], FileMode.Open, FileAccess.Read))
            {
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                DataSet data = excelDataReader.AsDataSet();
                DataTable dataTable = data.Tables[0];
                readTableToTxt(files[i], dataTable);
            }
        }
        AssetDatabase.Refresh();
    }
    private static void readTableToTxt(string filePath,DataTable dataTable)
    {
        string filename = Path.GetFileNameWithoutExtension(filePath);
        string path = Application.dataPath + "/Resources/Data/" + filename + ".txt";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using(FileStream fs=new FileStream(path, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                for(int row = 0; row < dataTable.Rows.Count; row++)
                {
                    DataRow dataRow = dataTable.Rows[row];
                    string str = "";
                    for(int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        string val = dataRow[col].ToString();
                        str = str + val + "\t";//每一项tab分割
                    }
                    sw.Write(str);
                    if (row < dataTable.Rows.Count - 1)
                    {
                        sw.WriteLine();
                    }
                }
            }
        }
    }
}
