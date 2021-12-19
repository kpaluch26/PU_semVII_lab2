using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_projekt.ElasticModels
{
	public class ElasticModel
	{
		public string Currency { get; set; }
		public string Customer_first_name { get; set; }
		public string Customer_full_name { get; set; }
		public string Customer_gender { get; set; }
		public int Customer_id { get; set; }
	}
}
