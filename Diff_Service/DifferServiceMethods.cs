using Diff_Service.Data;
using Diff_Service.Data.Models;
using Diff_Service.Models;
using System;
using System.Net;
using System.ServiceModel.Web;

namespace Diff_Service
{
    public class DifferServiceMethods
    {
        public HttpStatusCode AddInput(string id, InputData data, bool leftInput)
        {
            try
            {
                DbMethods dbMethods = new DbMethods(new DifferContext());
                int? inputId = CheckIdValue(id);
                if (!inputId.HasValue || data.Data == null)
                {
                    throw new WebFaultException<string>("Input has no value or data is null", HttpStatusCode.BadRequest);
                }
                if (leftInput)
                {
                    dbMethods.AddOrUpdate(inputId.Value, data.Data);
                }
                else
                {
                    dbMethods.AddOrUpdate(inputId.Value, null, data.Data);
                }
                WebOperationContext ctx = WebOperationContext.Current;
                if (ctx == null)
                    return HttpStatusCode.Created;
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                return ctx.OutgoingResponse.StatusCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OutputData CheckInput(string id)
        {
            try
            {
                int? inputId = CheckIdValue(id);
                if (!inputId.HasValue)
                {
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                Differ diff = new DbMethods(new DifferContext()).GetDiffer(inputId.Value);
                if (diff == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
                OutputData output = new OutputData();
                DifferMethods differMethods = new DifferMethods();
                output.ResultType = differMethods.AreInputsEqual(diff.LeftInput, diff.RightInput).ToString();
                if (output.ResultType == DiffResultType.ContentDoesNotMatch.ToString())
                {
                    output.Diffs = differMethods.ReportDiffs(diff.LeftInput, diff.RightInput);
                }
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        /// <summary>
        /// This method checks if the given Id is valid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int? CheckIdValue(string id)
        {
            int inputId;
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out inputId))
                return null;

            return inputId;
        }

        #endregion
    }
}
