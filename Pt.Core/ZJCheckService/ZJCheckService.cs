using Pkpm.Framework.PkpmConfigService;
using QZWebService.ServiceModel;
using ServiceStack;

namespace Pkpm.Core.ZJCheckService
{

    public class ZJCheckService : IZJCheckService
    {
        IPkpmConfigService pkpmconfigService;
        string GetSceneDataUrl;
        public ZJCheckService(IPkpmConfigService pkpmconfigService)
        {
            this.pkpmconfigService = pkpmconfigService;

            GetSceneDataUrl = pkpmconfigService.GetSceneDataUrl;
        }
        /// <summary>
        /// 获取桩基检测方案信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetZJCheckListResponse GetZJCheck(GetZJCheckList model)
        {
            var client = new JsonServiceClient(GetSceneDataUrl);
            GetZJCheckList request = new GetZJCheckList()
            {
                CheckUnitName = model.CheckUnitName,
                CheckEquip = model.CheckEquip,
                CheckPeople = model.CheckPeople,
                Area = model.Area,
                Report = model.Report,
                ZX = model.ZX,
                posStart = model.posStart,
                count = model.count,
                ProjectName = model.ProjectName,
                StartDate = model.StartDate,
                EndDate =model.EndDate
            };
            return client.Get(request);


        }

        public GetZJCheckByIdResponse GetZJCheckById(GetZJCheckById model)
        {
            var client = new JsonServiceClient(GetSceneDataUrl);
            return client.Get(model);
        }

    }
}
