using System.Linq;

namespace Diff_Service.Data
{
    public class DBMethods
    {
        private IDifferContext _Context;

        public DBMethods(IDifferContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// This method Adds or Updates a Differ record in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="leftInput"></param>
        /// <param name="rightInput"></param>
        public void AddOrUpdate(int id, string leftInput = null, string rightInput = null)
        {
            if (!_Context.Differs.Any(d => d.Id == id))
            {
                _Context.Differs.Add(new Differ() { LeftInput = leftInput, RightInput = rightInput });
            }
            else
            {
                Differ differ = _Context.Differs.First(d => d.Id == id);
                leftInput = !string.IsNullOrEmpty(leftInput) ? differ.LeftInput = leftInput :
                differ.LeftInput = differ.LeftInput;

                rightInput = !string.IsNullOrEmpty(rightInput) ? differ.RightInput = rightInput : 
                differ.RightInput = differ.RightInput;
            }
            _Context.SaveChanges();
        }

        /// <summary>
        /// This method retriefs a differ record from the database useing a specified Id.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Differ GetDiffer(int id)
        {
            if (!_Context.Differs.Any(d => d.Id == id))
            {
                return null;
            }
            else
            {
                Differ differ = _Context.Differs.First(d => d.Id == id);
                if (string.IsNullOrEmpty(differ.LeftInput) || string.IsNullOrEmpty(differ.RightInput))
                {
                    return null;
                }
                return differ;
            }
        }
    }
}
