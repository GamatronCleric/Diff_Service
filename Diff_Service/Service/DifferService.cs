using Diff_Service.Interface;
using Diff_Service.Models;
using System.Net;

namespace Diff_Service.Service
{
    public class DifferService : IDifferService
    {
        DifferServiceMethods _differServiceMethods;

        /// <summary>
        /// This method adds the data for the left input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddLeftInput(string id, InputData data)
        {
            _differServiceMethods = new DifferServiceMethods();
            return _differServiceMethods.AddInput(id, data, true);
        }

        /// <summary>
        /// This method adds the data for the right input to the database by calling the AddInput method.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpStatusCode AddRightInput(string id, InputData data)
        {
            _differServiceMethods = new DifferServiceMethods();
            return _differServiceMethods.AddInput(id, data, false);
        }

        /// <summary>
        /// This method returns the output data of the specified Id. it will return the DiffResult and 
        /// (in case of a contentmismatch the a list of differences (offsets and lengths).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OutputData CheckInputById(string id)
        {
            _differServiceMethods = new DifferServiceMethods();
            return _differServiceMethods.CheckInput(id);
        }
    }
}
