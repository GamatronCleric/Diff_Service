using Diff_Service.Data;
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
                DBMethods dbMethods = new DBMethods();
                int? inputId = CheckIdValue(id);
                if (!inputId.HasValue || data.Data == null)
                {
                    throw new WebFaultException<string>("Input has no value or data is null", HttpStatusCode.BadRequest);
                }
                if (leftInput)
                {
                    dbMethods.AddOrUpdate(new DifferContext(), inputId.Value, data.Data);
                }
                else
                {
                    dbMethods.AddOrUpdate(new DifferContext(), inputId.Value, null, data.Data);
                }
                return HttpStatusCode.Created;
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
                Differ diff = new DBMethods().GetDiffer(new DifferContext(), inputId.Value);
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
            int inputId = 0;
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out inputId))
                return null;

            return inputId;
        }

        #endregion
    }
}
