namespace EntityFrameworkProject.ViewModels.EmployeeViewModels
{
    public class EmployeeListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }
    }
}
