using QZWebService.ServiceModel;

namespace Pkpm.Core.ZJCheckService
{
    public interface IZJCheckService
    {
        GetZJCheckListResponse GetZJCheck(GetZJCheckList model);
        GetZJCheckByIdResponse GetZJCheckById(GetZJCheckById model);
    }
}
