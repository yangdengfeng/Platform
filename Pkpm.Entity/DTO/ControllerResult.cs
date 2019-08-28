using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.DTO
{
    public class ControllerResult
    {
        public static ControllerResult SuccResult
        {
            get
            {
                return new ControllerResult()
                {
                    IsSucc = true,
                    ErroMsg = string.Empty
                };
            }
        }

        public static ControllerResult FailResult
        {
            get
            {
                return new ControllerResult()
                {
                    IsSucc = false,
                    ErroMsg = string.Empty
                };
            }
        }

        public static ControllerResult FailResultWithErrorMsg(string errorMsg)
        {
            return new ControllerResult()
            {
                IsSucc = false,
                ErroMsg = errorMsg
            };
        }

        private ControllerResult()
        {

        }

        public bool IsSucc { get; set; }
        public string ErroMsg { get; set; }
        public string AdditonMsg { get; set; }
    }
}
