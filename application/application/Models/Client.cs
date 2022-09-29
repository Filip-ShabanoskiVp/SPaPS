using System;
using System.Collections.Generic;

namespace application.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientActivities = new HashSet<ClientActivity>();
        }

        public long ClientId { get; set; }
        public string UserId { get; set; } = null!;
        public int ClientTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdNo { get; set; } = null!;
        public int CityId { get; set; }
        public int? CountryId { get; set; }
        public DateTime? DateEstablished { get; set; }
        public int? NoOfEmployees { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<ClientActivity> ClientActivities { get; set; }
    }
}
