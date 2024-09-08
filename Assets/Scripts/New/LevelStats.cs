using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Assets.Scripts.New
{
	[System.Serializable]
	class Serialization<T>
	{
		public List<T> data;
	}

	[Serializable]
	internal class LevelStats
	{
		static string path = Application.persistentDataPath + "/LevelsStats.json";
		static LevelStats()
		{
			if (File.Exists(path))
				LoadSetings();
			else
			{
				Debug.Log("Не удалось найти файл");
				History = new ();
				SaveSetings();
			}
		}
		static public List<FlightResult> History { get; set; }
		static public void SaveSetings()
		{
			string jsonData = JsonUtility.ToJson(new Serialization<FlightResult> { data = History });
			Debug.Log(jsonData);
			File.WriteAllText(path, jsonData);
			//File.WriteAllText(Application.persistentDataPath + "/LevelsStats.json", jsonData);
		} 

		static public void LoadSetings()
		{
			Debug.Log(File.Exists(path));
			string jsonData = File.ReadAllText(path);
			Debug.Log(jsonData);
			History = JsonUtility.FromJson<Serialization<FlightResult>>(jsonData).data;
			SaveSetings();
			if (History == null)
			{
				History = new List<FlightResult>();
			}
		}

		static public FlightResult BestTimeFlight()
		{
			double min_t = History.Min(a => a.FlightTime);
			return History.FirstOrDefault(a => a.FlightTime == min_t);
		}

		static public FlightResult BestFuelFlight()
		{
			double max_f = History.Min(a => a.RemainingFuelPercent);
			return History.FirstOrDefault(a => a.FlightTime == max_f);
		}
		static public string Discription()
		{
			return
				$"Самое короткое время посадки: {BestTimeFlight().FlightTime} {BestTimeFlight() is null}/n" +
				$"Рекорд по экономии топлива: {BestFuelFlight()} {BestFuelFlight() is null}/n ";
		}
	}
}
