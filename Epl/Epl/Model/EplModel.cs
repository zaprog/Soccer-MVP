using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Epl.Model
{
    public class EplModel : IEplModel
    {
        private int _dashIndex = 0;
        private string _dashValue = string.Empty;
        public string ErrorMessage { get; set; }

        public DataTable SortedPayloadTable(string filename)
        {
            DataTable oDataTable = null;
            if (IsFileExistsandValid(filename)) //validate the file extension
            {
                try
                {
                    string[] Headers = { "Team", "P", "W", "L", "D", "F", "-", "A", "Pts" };
                    if (FilestructureValidate(filename, Headers)) // validate file structure
                    {
                        var Payload = FetchPayload(filename);
                        var fileparse = ParsePayload(Payload);
                        oDataTable = Payload2DataTable(Headers, fileparse);
                    }
                    else
                        ErrorMessage = "Invalid file format, please check the csv for data integrity.";
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            return oDataTable;
        }

        public bool IsFileExistsandValid(string filename)
        {
            if (Path.GetExtension(filename) == ".csv")
                return true;
            else
                return false;
        }

        public bool FilestructureValidate(string filename, string[] Headers)
        {
            // read the file and validate the headers and columns
            var Payload = File.ReadAllLines(filename);
            return Enumerable.SequenceEqual(Payload[0].Split(','), Headers);
        }

        public List<string[]> FetchPayload(string filename)
        {
            //read file and skip the header
            var Filedata = File.ReadAllLines(filename);
            return Filedata.Skip(1).Select(x => x.Split(',')).ToList();
        }

        public List<KeyValuePair<int, string[]>> ParsePayload(List<string[]> Payload)
        {
            var kvp = new List<KeyValuePair<int, string[]>>();
            for (int i = 0; i < Payload.Count; i++)
            {
                var index = Array.IndexOf(Payload[i], "-");
                if (index != -1)
                {
                    var stringparse = Math.Abs(Convert.ToInt32(Payload[i][index - 1]) - Convert.ToInt32(Payload[i][index + 1]));
                    kvp.Add(new KeyValuePair<int, string[]>(stringparse, Payload[i]));
                }
                else
                {
                    _dashIndex = i;
                    _dashValue = string.Join("", Payload[i]);
                }

            }
            return kvp;
        }

        public DataTable Payload2DataTable(string[] Headers, List<KeyValuePair<int, string[]>> kvp)
        {
            // sort kvp ascending order
            var sorted_kvp = kvp.OrderBy(keys => keys.Key);

            // create datacolumns based on payload header
            var odatatable = new DataTable();
            odatatable.TableName = "EPL-TABLE";
            for (int i = 0; i < Headers.Length; i++)
            {
                var dc = new DataColumn(Headers[i].ToString(), typeof(string));
                odatatable.Columns.Add(dc);
            }

            var x = 0; // this is to validate empty cells or Dashes
            foreach (var key in sorted_kvp)
            {
                x++;
                if (x == _dashIndex)
                {
                    odatatable.Rows.Add(key.Value);
                    odatatable.Rows.Add(_dashValue);
                }
                else
                    odatatable.Rows.Add(key.Value);

            }
            return odatatable;
        }
    }
}
