using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    public class Parser
    {
        string excelFilePath;
        string dbFilePath;
        public void CreateDBFile()
        {
            var json = JsonConvert.SerializeObject(this.ImportOriginalList());
            File.WriteAllText(@"db.txt", json);
        }

        public Parser(string excelfilePath, string dbFilePath)
        {
            this.excelFilePath = excelfilePath;
            this.dbFilePath = dbFilePath;
        }
        public List<Threat> DeserializeDBFile()
        {
            var json = File.ReadAllText(dbFilePath);
            return JsonConvert.DeserializeObject<List<Threat>>(json);
        }
        public Dictionary<string, string> UpdateDBFile()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<Threat> originThreats = this.ImportOriginalList();
            List<Threat> currentThreats = DeserializeDBFile();
            List<Threat> currentThreatsCopy = currentThreats.GetRange(0, currentThreats.Count);
            foreach (var item in currentThreats)
            {
                if (originThreats.Where(s => s.ID == item.ID).Count() > 0)
                {
                    if (!Threat.IsSameThreats(item, originThreats.Where(s => s.ID == item.ID).First()))
                    {
                        dictionary.Add(item.ID, Threat.FindDifference(item, originThreats.Where(s => s.ID == item.ID).First()));
                        currentThreatsCopy[currentThreats.IndexOf(item)] = originThreats.Where(s => s.ID == item.ID).First();
                    }
                }
            }
            if (dictionary.Count > 0)
            {
                var path = JsonConvert.SerializeObject(currentThreatsCopy);
                File.WriteAllText(dbFilePath, path);
                return dictionary;
            }
            else
            {
                throw new Exception("Обновление не удалось. База данных актуальна.");
            }
        }
        public List<Threat> ImportOriginalList()
        {
            List<Threat> threats = new List<Threat>();
            XSSFWorkbook workBook;
            using (FileStream file = new FileStream(this.excelFilePath, FileMode.Open, FileAccess.Read))
            {
                workBook = new XSSFWorkbook(file);
            }
            ISheet sheet = workBook.GetSheet("Sheet");
            for (int row = 2; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null)
                {
                    Threat threat = new Threat(sheet.GetRow(row).GetCell(0).NumericCellValue.ToString(),
                        sheet.GetRow(row).GetCell(1).StringCellValue.Replace("\r\n", " "),
                        sheet.GetRow(row).GetCell(2).StringCellValue.Replace("\r\n", " "),
                        sheet.GetRow(row).GetCell(3).StringCellValue,
                        sheet.GetRow(row).GetCell(4).StringCellValue,
                        Convert.ToBoolean(sheet.GetRow(row).GetCell(5).NumericCellValue),
                        Convert.ToBoolean(sheet.GetRow(row).GetCell(6).NumericCellValue),
                        Convert.ToBoolean(sheet.GetRow(row).GetCell(7).NumericCellValue));
                    threats.Add(threat);
                }
            }
            return threats;
        }
    }
}
