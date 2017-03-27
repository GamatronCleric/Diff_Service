using System.Linq;

namespace Diff_Service.Data
{
    public class DBMethods
    {
        /// <summary>
        /// This method Adds or Updates a Differ record in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="leftInput"></param>
        /// <param name="rightInput"></param>
        public void AddOrUpdate(DifferContext context, int id, 
            string leftInput = null, string rightInput = null)
        {
            if (!context.Differs.Any(d => d.Id == id))
            {
                context.Differs.Add(new Differ() { LeftInput = leftInput, RightInput = rightInput });
            }
            else
            {
                Differ differ = context.Differs.First(d => d.Id == id);
                leftInput = !string.IsNullOrEmpty(leftInput) ? differ.LeftInput = leftInput :
                differ.LeftInput = differ.LeftInput;

                rightInput = !string.IsNullOrEmpty(rightInput) ? differ.RightInput = rightInput : 
                differ.RightInput = differ.RightInput;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// This method retriefs a differ record from the database useing a specified Id.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Differ GetDiffer(DifferContext context, int id)
        {
            if (!context.Differs.Any(d => d.Id == id))
            {
                return null;
            }
            else
            {
                Differ differ = context.Differs.First(d => d.Id == id);
                if (string.IsNullOrEmpty(differ.LeftInput) || string.IsNullOrEmpty(differ.RightInput))
                {
                    return null;
                }
                return differ;
            }
        }
    }
}
