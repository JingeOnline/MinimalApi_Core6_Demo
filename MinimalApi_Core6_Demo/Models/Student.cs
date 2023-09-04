namespace MinimalApi_Core6_Demo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Description { get; set; }

        public Student(int id, string name, bool gender, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Gender = gender;
            this.Description = description;
        }
    }
}
