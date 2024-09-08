using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.New
{
	[Serializable]
	internal class FlightResult
	{
		public FlightResult(DateTime _dateTime, double _flightTime, double _remainingFuel, double _totalFuel) 
		{
			dateTime = _dateTime.ToString();
			FlightTime = _flightTime;
			RemainingFuel = _remainingFuel;
			TotalFuel = _totalFuel;
		}
		public string dateTime;
		public double FlightTime;
		public double RemainingFuel;
		public double TotalFuel;
		public double RemainingFuelTons { 
			get
			{
				return RemainingFuel;
			}
		}
		public double RemainingFuelPercent { 
			get {
				return RemainingFuel / TotalFuel * 100;
			} 
		}
		public StartRocketData StartRocketData { get; set; }

	}
}
