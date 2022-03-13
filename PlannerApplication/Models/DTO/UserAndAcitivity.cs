namespace PlannerApplication.Models.DTO
{
    public class UserAndAcitivity 
    {
        public UserAndAcitivity(planneruser user, List<string> activities)
        {
            Sports = activities;
            User = user;
        }

        public List<string> Sports { get; set; }
        public planneruser User { get; set; }
    }
}
