using Epl.Model;
using Xunit;

namespace Epl.Test
{
    public class EplUnitTests
    {
        EplModel oEplModel = new EplModel();
        string csvfile = @"football.csv";
        string otherfile = @"football.txt";

        [Fact]
        public void IsFileExistsandValid()
        {
            Assert.True(oEplModel.IsFileExistsandValid(csvfile));
            Assert.False(oEplModel.IsFileExistsandValid(otherfile));
        }
        [Theory]
        [InlineData(new object[] { new string[] { "Team", "P", "W", "L", "D", "F", "-", "A", "Pts" } })]
        public void FilestructureValidate(string[] Headers)
        {
            Assert.True(oEplModel.FilestructureValidate(csvfile, Headers));
        }
        [Fact]
        public void FetchPayload()
        {
            var Payload = oEplModel.FetchPayload(csvfile);
            Assert.True(Payload.Count > 0);
            Assert.Equal(21, Payload.Count);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "Team", "P", "W", "L", "D", "F", "-", "A", "Pts" } })]
        public void Parse2PayloadTable(string[] Headers)
        {
            var Payload = oEplModel.FetchPayload(csvfile);
            Assert.True(Payload.Count > 0);
            Assert.Equal(21, Payload.Count);

            var kvp = oEplModel.ParsePayload(Payload);
            Assert.True(kvp.Count > 0);
            Assert.Equal(20, kvp.Count);

            var oDatatable = oEplModel.Payload2DataTable(Headers, kvp);
            Assert.Equal(9, oDatatable.Columns.Count);
            Assert.Equal(21, oDatatable.Rows.Count);
            Assert.Equal("8. Aston_Villa", oDatatable.Rows[0].ItemArray[0].ToString());
        }
    }
}
