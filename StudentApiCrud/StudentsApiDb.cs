using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace StudentApiCrud
{
    public class StudentsApiDb : DbContext
    {
        public StudentsApiDb(DbContextOptions<StudentsApiDb> options) : base(options) { }
        public DbSet<Student> Students => Set<Student>();

    }
}
