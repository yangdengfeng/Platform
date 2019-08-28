using Pkpm.Framework.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.PkpmConfigService
{
    public interface IPkpmConfigService
    {
        string DefaultInst { get; }

        string DefaultInpsect { get; }

        string UploadInstStartWith { get; }

        string InstRoleCode { get; }

        string AdminRoleCode { get; }

        string SuperVisorRoleCode { get; }

        string QrRoleCode { get; }

        string ReportRoleCode { get; }

        string CheckPeopleRoleCode { get; }

        string AllQYYAndJYYCode { get; }

        string WxWebUrl { get; }

        string NoReportNumCanSearchTimeInterval { get; }

       List<string> CTypeItemCodes { get; }

        List<string> AcsTypeItemNames { get; }

        List<string> RecheckTypeItemNames { get; }

        List<string> CTypeStartWiths { get; }

        List<string> AcsPicDataTypes { get; }

        List<string> FixDtTypes { get; }

        List<string> AcsAxisNLabel { get; }

        string CloudFilePathPrefix { get; }

        string PrintedSamplePhrase { get; }

        string IsWcfEnabled { get; }

        string HaveSamplePhrase { get; }

        string CheckedSamplePhrase { get; } 

        string TestedSamplePhrase { get; }

        string ProofreadSamplePhrase { get; }

        string VerifyedSamplePhrase { get; }

        string ApprovaledSamplePhrase { get; }

        string GiveOutSamplePhrase { get; }

        string FileedSamplePhrase { get; }

        string RefundedSamplePhrase { get; }

        string CanceledSamplePhrase { get; }

        string GetSceneDataUrl { get;  }
    }

    public class PkpmConfigService : IPkpmConfigService
    {
        string defaultInst;
        string defaultInpsect; 
        string uploadInstStartWith;

        string instRoleCode;
        string adminRoleCode;
        string superVisorRoleCode;
        string qrRoleCode;
        string reportRoleCode;
        string checkPeopleRoleCode;
        string allQYYAndJYYCode;

        string wxWebUrl;
        string noReportNumCanSearchTimeInterval;
        string getSceneDataUrl;
        string cTypeItemCodes;
        string acsTypeItemNames;
        string recheckTypeItemNames;
        string cTypeStartWiths;
        string acsPicDataTypes;
        string fixDtTypes;
        string acsAxisNLabel;

        string cloudFilePathPrefix; 
        string isWcfEnabled;

        string approvaledSamplePhrase;
        string canceledSamplePhrase;
        string checkedSamplePhrase;  
        string fileedSamplePhrase; 
        string giveOutSamplePhrase;
        string haveSamplePhrase;  
        string printedSamplePhrase;
        string proofreadSamplePhrase; 
        string refundedSamplePhrase; 
        string testedSamplePhrase; 
        string verifyedSamplePhrase;

        public PkpmConfigService()
        {
            defaultInst = ConfigurationManager.AppSettings["SysDefaultInst"];
            defaultInpsect = ConfigurationManager.AppSettings["SysDefaultInspect"];
            uploadInstStartWith = ConfigurationManager.AppSettings["UploadInstStartWith"];

            instRoleCode = ConfigurationManager.AppSettings["InstRoleCode"];
            adminRoleCode = ConfigurationManager.AppSettings["AdminRoleCode"];
            superVisorRoleCode = ConfigurationManager.AppSettings["SuperVisorRoleCode"];
            qrRoleCode = ConfigurationManager.AppSettings["QrRoleCode"];
            reportRoleCode = ConfigurationManager.AppSettings["ReportRoleCode"];
            checkPeopleRoleCode = ConfigurationManager.AppSettings["CheckPeopleRoleCode"];
            allQYYAndJYYCode = ConfigurationManager.AppSettings["AllQYYAndJYYCode"];

            wxWebUrl = ConfigurationManager.AppSettings["WxWebUrl"];
            noReportNumCanSearchTimeInterval = ConfigurationManager.AppSettings["NoReportNumCanSearchTimeInterval"];

            cTypeItemCodes = ConfigurationManager.AppSettings["CReportTypes"];
            acsTypeItemNames = ConfigurationManager.AppSettings["AcsItemNames"] ;
            recheckTypeItemNames = ConfigurationManager.AppSettings["ReCheckItemNames"] ;
            cTypeStartWiths = ConfigurationManager.AppSettings["CReportStartWith"] ;
            acsPicDataTypes = ConfigurationManager.AppSettings["AcsPicDataTypes"] ;
            fixDtTypes = ConfigurationManager.AppSettings["FixedDtTypes"] ;
            acsAxisNLabel = ConfigurationManager.AppSettings["AcsAxisNLabel"] ; 

            cloudFilePathPrefix = ConfigurationManager.AppSettings["CloudPathPrefix"];
            isWcfEnabled = ConfigurationManager.AppSettings["IsWcfEnabled"]; 

            haveSamplePhrase = ConfigurationManager.AppSettings["CollectedSamplePhrase"];
            checkedSamplePhrase = ConfigurationManager.AppSettings["CheckedSamplePhrase"];
            testedSamplePhrase = ConfigurationManager.AppSettings["TestedSamplePhrase"];
            proofreadSamplePhrase = ConfigurationManager.AppSettings["ProofreadSamplePhrase"];
            verifyedSamplePhrase = ConfigurationManager.AppSettings["VerifyedSamplePhrase"];
            approvaledSamplePhrase = ConfigurationManager.AppSettings["ApprovaledSamplePhrase"];
            giveOutSamplePhrase = ConfigurationManager.AppSettings["GiveOutSamplePhrase"];
            fileedSamplePhrase = ConfigurationManager.AppSettings["FileedSamplePhrase"];
            refundedSamplePhrase = ConfigurationManager.AppSettings["RefundedSamplePhrase"];
            canceledSamplePhrase = ConfigurationManager.AppSettings["CanceledSamplePhrase"];
            printedSamplePhrase = ConfigurationManager.AppSettings["PrintedSamplePhrase"];
            getSceneDataUrl = ConfigurationManager.AppSettings["GetSceneDataUrl"];
        }

      

        public List<string> AcsAxisNLabel
        {
            get
            {
                return CommonUtils.SplitStrIntoList(acsAxisNLabel);
            }
        }

        public List<string> AcsPicDataTypes
        {
            get
            {
                return CommonUtils.SplitStrIntoList(acsPicDataTypes);
            }
        }

        public List<string> AcsTypeItemNames
        {
            get
            {
                return CommonUtils.SplitStrIntoList(acsTypeItemNames);
            }
        }

        public string AdminRoleCode
        {
            get
            {
                return adminRoleCode;
            }
        }

        public string AllQYYAndJYYCode
        {
            get
            {
                return allQYYAndJYYCode;
            }
        }

        public string ApprovaledSamplePhrase
        {
            get
            {
                return approvaledSamplePhrase;
            }
        }

        public string CanceledSamplePhrase
        {
            get
            {
                return canceledSamplePhrase;
            }
        }

        public string CheckedSamplePhrase
        {
            get
            {
                return checkedSamplePhrase;
            }
        }

        public string CheckPeopleRoleCode
        {
            get
            {
                return checkPeopleRoleCode;
            }
        }

        public string CloudFilePathPrefix
        {
            get
            {
                return cloudFilePathPrefix;
            }
        }

        public List<string> CTypeItemCodes
        {
            get
            {
                return cTypeItemCodes.Split(',').ToList();
            }
        }

        public List<string> CTypeStartWiths
        {
            get
            {
                return CommonUtils.SplitStrIntoList(cTypeStartWiths);
            }
        }

        public string DefaultInpsect
        {
            get
            {
                return defaultInpsect;
            }
        }

        public string DefaultInst
        {
            get
            {
                return defaultInst;
            }
        }

        public string FileedSamplePhrase
        {
            get
            {
                return fileedSamplePhrase;
            }
        }

        public List<string> FixDtTypes
        {
            get
            {
                return CommonUtils.SplitStrIntoList(fixDtTypes);
            }
        }

        public string GiveOutSamplePhrase
        {
            get
            {
                return giveOutSamplePhrase;
            }
        }

        public string HaveSamplePhrase
        {
            get
            {
                return haveSamplePhrase;
            }
        }

        public string InstRoleCode
        {
            get
            {
                return instRoleCode;
            }
        }

        public string IsWcfEnabled
        {
            get
            {
                return isWcfEnabled;
            }
        }

        public string NoReportNumCanSearchTimeInterval
        {
            get
            {
                return noReportNumCanSearchTimeInterval;
            }
        }

        public string PrintedSamplePhrase
        {
            get
            {
                return printedSamplePhrase;
            }
        }

        public string ProofreadSamplePhrase
        {
            get
            {
                return proofreadSamplePhrase;
            }
        }

        public string QrRoleCode
        {
            get
            {
                return qrRoleCode;
            }
        }

        public  List<string> RecheckTypeItemNames
        {
            get
            {
                return CommonUtils.SplitStrIntoList(recheckTypeItemNames);
            }
        }

        public string RefundedSamplePhrase
        {
            get
            {
                return refundedSamplePhrase;
            }
        }

        public string ReportRoleCode
        {
            get
            {
                return reportRoleCode;
            }
        }

        public string SuperVisorRoleCode
        {
            get
            {
                return superVisorRoleCode;
            }
        }

        public string TestedSamplePhrase
        {
            get
            {
                return testedSamplePhrase;
            }
        }

        public string UploadInstStartWith
        {
            get
            {
                return uploadInstStartWith;
            }
        }

        public string VerifyedSamplePhrase
        {
            get
            {
                return verifyedSamplePhrase;
            }
        }

        public string WxWebUrl
        {
            get
            {
                return wxWebUrl;
            }
        }

        public string GetSceneDataUrl
        {
            get
            {
                return getSceneDataUrl;
            }
        }
    }
}
