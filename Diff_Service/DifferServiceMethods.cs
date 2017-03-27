using Diff_Service.Data;
using System;
using System.Net;
using System.ServiceModel.Web;

namespace Diff_Service
{
    public static class DifferServiceMethods
    {
        public static HttpStatusCode AddInput(string id, InputData data, bool leftInput, DifferContext context)
        {
            try
            {
                int? inputId = CheckIdValue(id);
                if (!inputId.HasValue || data.Data == null)
                {
                    throw new WebFaultException<string>("Input has no value or data is null", HttpStatusCode.BadRequest);
                }
                if (leftInput)
                {
                    DBMethods.AddOrUpdate(context, inputId.Value, data.Data);
                }
                else
                {
                    DBMethods.AddOrUpdate(context, inputId.Value, null, data.Data);
                }
                return HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OutputData CheckInput(string id, DifferContext context)
        {
            try
            {
                int? inputId = CheckIdValue(id);
                if (!inputId.HasValue)
                {
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                Differ diff = DBMethods.GetDiffer(context, inputId.Value);
                if (diff == null)
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
                OutputData output = new OutputData();
                output.ResultType = DifferMethods.AreInputsEqual(diff.LeftInput, diff.RightInput).ToString();
                if (output.ResultType == DiffResultType.ContentDoesNotMatch.ToString())
                {
                    output.Diffs = DifferMethods.ReportDiffs(diff.LeftInput, diff.RightInput);
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
