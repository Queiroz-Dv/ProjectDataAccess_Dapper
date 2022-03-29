using System;

namespace ProjectDataAccess_Dapper.Models
{
    public class CareerItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        // Tipo complexo
        public Course Course { get; set; }
    }
}
