using System.Data;

namespace Epl.View
{
    public interface IEplView
    {
        DataTable SortedPayloadTable { set; }
        string ErrorMessage { set; }
    }
}
