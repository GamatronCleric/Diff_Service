using Diff_Service.Data;
using System.Net;

namespace Diff_Service
{
    public class DifferService : IDifferService
    {
        /// <summary>
        /// This method adds the data for the left input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddLeftInput(string id, InputData data)
        {
            return DifferServiceMethods.AddInput(id, data, true, new DifferContext());
        }

        /// <summary>
        /// This method adds the data for the right input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddRightInput(string id, InputData data)
        {
            return DifferServiceMethods.AddInput(id, data, false, new DifferContext());
        }

        /// <summary>
        /// This method returns the output data of the specified Id. it will return the DiffResult and 
        /// (in case of a contentmismatch the a list of differences (offsets and lengths).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OutputData CheckInputById(string id)
        {
            return DifferServiceMethods.CheckInput(id, new DifferContext());
        }
    }
}
