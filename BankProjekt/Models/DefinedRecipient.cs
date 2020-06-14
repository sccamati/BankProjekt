namespace BankProjekt.Models
{
    public class DefinedRecipient
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string ReceiversName { get; set; }
        public string ReceiversNumber { get; set; }
        public virtual Profile Profile { get; set; }
    }
}