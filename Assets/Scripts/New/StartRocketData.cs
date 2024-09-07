using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.New
{
	[Serializable]
	internal class StartRocketData :  IStartData
	{
		public double Time { get; set; }
		public double angle { get; set; }
		public double JetV { get; set; }
		public double StartH { get; set; }
		public double G { get; set; }
		public double Rm { get; set; }
		public double Fm { get; set; }
		public double MaxFF { get; set; }
		public double delta { get; set; }
		public vect RocketPos { get; set; }
		public vect LandV { get; set; }
		public vect v { get; set; }
		public vect ac { get; set; }
	}
}