using System.Collections.Generic;

namespace FreeCourse.Web.Models.Identit
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname{ get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Sehir { get; set; }
        public IEnumerable<string> GetUserProps()
        {
            yield return UserName;
            yield return Email;
            yield return Sehir;
            yield return Name;
            yield return Surname;
        }
    }
}
