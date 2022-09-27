namespace X01Api.Models.ViewModel
{
    public class VkProfileDto
    {
        public int IsAutorized { get; set; }
        public int UserId { get; set; }
        public string FirstName
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        public  int Date { get; set; }
        public  int Expire { get; set; }
    }
}
