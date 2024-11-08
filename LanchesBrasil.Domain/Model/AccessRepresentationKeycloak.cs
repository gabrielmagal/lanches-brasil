namespace LanchesBrasil.Commons.Model
{
    public class AccessRepresentationKeycloak
    {
        public bool ManageGroupMembership { get; set; }
        public bool View { get; set; }
        public bool MapRoles { get; set; }
        public bool Impersonate { get; set; }
        public bool Manage { get; set; }
    }
}
