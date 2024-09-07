using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.New
{
	internal class GameResult
	{
		public Guid Id { get; set; }
		public float Time { get; set; }
		public float RemainingFuel { get; set; }
		public DateTime date { get; set; }
	}
}
