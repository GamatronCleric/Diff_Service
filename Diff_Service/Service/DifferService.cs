using Diff_Service.Data;
using System.Net;

namespace Diff_Service
{
    public class DifferService : IDifferService
    {
        DifferServiceMethods _DifferServiceMethods;

        /// <summary>
        /// This method adds the data for the left input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddLeftInput(string id, InputData data)
        {
            _DifferServiceMethods = new DifferServiceMethods();
            return _DifferServiceMethods.AddInput(id, data, true);
        }

        /// <summary>
        /// This method adds the data for the right input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddRightInput(string id, InputData data)
        {
            _DifferServiceMethods = new DifferServiceMethods();
            return _DifferServiceMethods.AddInput(id, data, false);
        }

        /// <summary>
        /// This method returns the output data of the specified Id. it will return the DiffResult and 
        /// (in case of a contentmismatch the a list of differences (offsets and lengths).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OutputData CheckInputById(string id)
        {
            _DifferServiceMethods = new DifferServiceMethods();
            return _DifferServiceMethods.CheckInput(id);
        }
    }
}
