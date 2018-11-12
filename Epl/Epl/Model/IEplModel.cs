using System.Data;

namespace Epl.Model
{
    public interface IEplModel
    {
        DataTable SortedPayloadTable(string filename);
    }
}
