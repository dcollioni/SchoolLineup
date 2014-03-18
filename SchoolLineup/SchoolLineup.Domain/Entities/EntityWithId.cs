namespace SchoolLineup.Domain.Entities
{
    public class EntityWithId
    {
        private string documentId;
        public string DocumentId
        {
            get { return documentId; }
            set
            {
                Id = int.Parse(value.Split('/')[1]);
                documentId = value;
            }
        }

        public int Id { get; set; }
    }
}