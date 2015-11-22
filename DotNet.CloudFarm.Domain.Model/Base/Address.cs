using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotNet.CloudFarm.Domain.Model
{
    public class Address
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ParentCode { get; set; }

        public string FullName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Address> Children { get; set; }

        [JsonIgnore]
        public virtual Address Parent { get; set; }
    }

    public class AddressEfMap : EntityTypeConfiguration<Address>
    {
        public AddressEfMap()
        {
            ToTable("Address");

            HasKey(c => c.Code);
            Property(c => c.Code).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //树形
            HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentCode)
                .WillCascadeOnDelete(false);
        }
    }
}
