using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class UserAndRoles
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string LayUser { get; set; }

        public string VolunteerRole { get; set; }

        /*
        public string TeacherRole { get; set; }
        */

        public string PriestRole { get; set; }

        public string ParishAdminRole { get; set; }

        public string AppAdminRole { get; set; }
    }
}