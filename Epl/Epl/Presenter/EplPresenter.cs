using Epl.Model;
using Epl.View;

namespace Epl.Presenter
{
    public class EplPresenter
    {
        IEplView eplview;
        public EplPresenter(IEplView view)
        {
            eplview = view;
        }
        public void ParsePayloadData(string filename)
        {
            var eplmodel = new EplModel();
            eplview.SortedPayloadTable = eplmodel.SortedPayloadTable(filename);
            if (eplmodel.ErrorMessage != null)
                eplview.ErrorMessage = eplmodel.ErrorMessage;
        }
    }
}
