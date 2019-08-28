using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pkpm.Entity.Models
{
    public class CheckPeopleServiceModel : t_bp_People
    {
        public int IsNormal { get; set; }
    }

    public class CheckPeopleWeoXinViewModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<CheckPeopleServiceModel> Data { get; set; }
    }
    public class ZhuangjiFangAnWebServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<tab_pile_programme> Data { get; set; }
    }

    public class ZJSceneTestDetailModel
    {
        /// <summary>
        /// 方案id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }

        public string PROJECTNAME { get; set; }

        /// <summary>
        /// 实验方法0：单桩竖向抗压,1：单桩竖向抗拔, 2：岩石锚杆抗拔,3：自平衡,4：复合地基, 5：浅层平板, 6：深层平板, 7：原位试验_土层 , 8：原位试验_岩基,9：岩基荷载
        /// </summary>
        public int? TEST_TYPE { get; set; }
        /// 测试仪编号
        /// </summary>
        public string MACHINE_ID { get; set; }
        /// <summary>
        /// 试桩编号
        /// </summary>
        public string STAKE_ID { get; set; }
    }

    public class ZJSceneTestDataWebServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<ZJSceneTestDetailModel> Data { get; set; }
    }

    public class GetZhuangJiModel
    {
        /// <summary>
        /// 工程名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }
        /// <summary>
        /// 桩基号
        /// </summary>
        public string jzsynos { get; set; }
        public int? count { get; set; }
        public int? posStart { get; set; }
    }

    public class PeopleAndEquipInProgramme
    {
        public tab_pile_programme ZhuangJi { get; set; }
        public Dictionary<int, string> Peoples { get; set; }
        public Dictionary<int, string> Equips { get; set; }
    }
    public class ZJProjectGPSWebServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public ZJProjectGPSModel Data { get; set; }
    }
    public class ZJProjectGPSModel
    {
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }
        /// <summary>
        /// 桩号
        /// </summary>
        public string STAKE_ID { get; set; }
        /// <summary>
        /// 工程编码
        /// </summary>
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string Area { get; set; }
    }

    public class ZJExceptionWebServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<ZJExceptionModel> Datas { get; set; }
    }
    public class ZJExceptionModel
    {
        public string checknum { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string unitcode { get; set; }
        public string customname { get; set; }
        public string pileno { get; set; }
        public string people { get; set; }
        public string content { get; set; }
        public string handleContent { get; set; }
        public string handlePeople { get; set; }
    }
}