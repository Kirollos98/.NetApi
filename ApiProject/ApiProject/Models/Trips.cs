using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class Trips
    {

        public int Id { get; set; }

        [MaxLength(500)]
        public string TripNum { get; set; }


        [Display(Name = "City From ")]
        public int CitiesFromId { get; set; }

        [ForeignKey("CitiesFromId")]
        //[InverseProperty("CityFromTrips")]
        public virtual Cities CityFrom  { get; set; }


        [Display(Name = "City To ")]
        public int CitiesToId { get; set; }

        [ForeignKey("CitiesToId")]
       // [InverseProperty("CityToTrips")]
        public virtual Cities CityTo { get; set; }   

        public DateTime DepTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public DateTime DepDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public int Price { get; set; }

        public int AvailableSeats { get; set; }

        [Display(Name = "Company Asset")]
        public int CompanyAssetId { get; set; }

        [ForeignKey("CompanyAssetId")]
        public virtual CompanyAssets CompanyAssets { get; set; }




    }
}
